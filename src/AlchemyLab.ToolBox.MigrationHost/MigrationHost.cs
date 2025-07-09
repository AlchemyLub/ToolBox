namespace AlchemyLab.ToolBox.MigrationHost;

/// <summary>
/// Предоставляет готовый хост для выполнения миграций EF Core
/// </summary>
public sealed class MigrationHost<TContext> where TContext : DbContext
{
    private readonly IHost host;
    private readonly ILogger<MigrationHost<TContext>> logger;

    public MigrationHost(string connectionString, Action<DbContextOptionsBuilder> optionsAction)
    {
        host = CreateHost(connectionString, optionsAction);
        logger = host.Services.GetRequiredService<ILogger<MigrationHost<TContext>>>();
    }

    /// <summary>
    /// Применяет все ожидающие миграции
    /// </summary>
    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Начинаем применение миграций для {ContextName}", typeof(TContext).Name);

        using IServiceScope scope = host.Services.CreateScope();
        TContext context = scope.ServiceProvider.GetRequiredService<TContext>();

        IEnumerable<string> pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
        List<string> pendingList = pendingMigrations.ToList();

        if (pendingList.Count == 0)
        {
            logger.LogInformation("Миграции не найдены");
            return;
        }

        logger.LogInformation(
            "Найдено {Count} ожидающих миграций: {Migrations}",
            pendingList.Count,
            string.Join(", ", pendingList));

        await context.Database.MigrateAsync(cancellationToken);

        logger.LogInformation("Миграции успешно применены");
    }

    /// <summary>
    /// Получает список всех миграций
    /// </summary>
    public async Task<MigrationInfo> GetMigrationsInfoAsync(CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = host.Services.CreateScope();
        TContext context = scope.ServiceProvider.GetRequiredService<TContext>();

        IEnumerable<string> appliedMigrations = await context.Database.GetAppliedMigrationsAsync(cancellationToken);
        IEnumerable<string> pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
        IEnumerable<string> allMigrations = context.Database.GetMigrations();

        return new(
            appliedMigrations.ToList(),
            pendingMigrations.ToList(),
            allMigrations.ToList());
    }

    /// <summary>
    /// Откатывает к указанной миграции
    /// </summary>
    public async Task RollbackToAsync(string targetMigration, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Откатываемся к миграции {Migration}", targetMigration);

        using IServiceScope scope = host.Services.CreateScope();
        TContext context = scope.ServiceProvider.GetRequiredService<TContext>();

        await context.Database.MigrateAsync(targetMigration, cancellationToken);

        logger.LogInformation("Откат выполнен успешно");
    }

    private static IHost CreateHost(string connectionString, Action<DbContextOptionsBuilder> optionsAction) =>
        Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<TContext>(optionsAction);
            })
            .Build();

    public void Dispose() => host.Dispose();
}
