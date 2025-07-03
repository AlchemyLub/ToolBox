namespace AlchemyLab.ToolBox.Resilience.Retry;

/// <summary>
/// Настройки по умолчанию для retry операций
/// </summary>
public static class RetryDefaults
{
    /// <summary>
    /// Количество попыток по умолчанию
    /// </summary>
    public static int DefaultRetryCount { get; set; } = 3;

    /// <summary>
    /// Базовая задержка между попытками
    /// </summary>
    public static TimeSpan DefaultDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Максимальная задержка между попытками
    /// </summary>
    public static TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Настройки перезапуска по умолчанию
    /// </summary>
    public static readonly RetryStrategyOptions DefaultOptions = new()
    {
        MaxRetryAttempts = DefaultRetryCount,
        BackoffType = DelayBackoffType.Exponential,
        Delay = DefaultDelay,
        MaxDelay = MaxDelay,
        UseJitter = true
    };

    /// <summary>
    /// Настройки бесконечных перезапусков
    /// </summary>
    public static readonly RetryStrategyOptions DefaultInfinityOptions = new()
    {
        MaxRetryAttempts = int.MaxValue,
        BackoffType = DelayBackoffType.Exponential,
        Delay = DefaultDelay,
        MaxDelay = MaxDelay,
        UseJitter = true
    };

    /// <summary>
    /// Политика по умолчанию для обычных операций
    /// </summary>
    public static ResiliencePipeline DefaultPipeline { get; set; } = CreateDefaultPipeline();

    /// <summary>
    /// Политика бесконечных повторов
    /// </summary>
    public static ResiliencePipeline InfinityPipeline { get; set; } = CreateInfinityPipeline();

    private static ResiliencePipeline CreateDefaultPipeline() => CreateRetryPipeline();

    private static ResiliencePipeline CreateInfinityPipeline() =>
        CreateRetryPipeline(new()
        {
            MaxRetryAttempts = int.MaxValue,
            BackoffType = DelayBackoffType.Exponential,
            Delay = DefaultDelay,
            MaxDelay = MaxDelay,
            UseJitter = true
        });

    private static ResiliencePipeline CreateRetryPipeline(RetryStrategyOptions? options = null) =>
        new ResiliencePipelineBuilder()
            .AddRetry(options ?? DefaultOptions)
            .Build();
}
