namespace AlchemyLab.ToolBox.MigrationHost.Models;

/// <summary>
/// Информация о состоянии миграций базы данных
/// </summary>
public sealed record MigrationInfo(
    IReadOnlyList<string> Applied,
    IReadOnlyList<string> Pending,
    IReadOnlyList<string> All)
{
    /// <summary>
    /// Проверяет, есть ли ожидающие миграции
    /// </summary>
    public bool HasPendingMigrations => Pending.Count > 0;

    /// <summary>
    /// Проверяет, применены ли все миграции
    /// </summary>
    public bool IsUpToDate => Pending.Count is 0;
}
