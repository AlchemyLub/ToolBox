namespace AlchemyLab.ToolBox.Common;

/// <summary>
/// Структура, аналогичная <see cref="Stopwatch"/> для избежания аллокаций
/// </summary>
/// <remarks>
/// Вытянуто из недр .NET <see href="https://source.dot.net/#Microsoft.Extensions.Http/ValueStopwatch.cs"/>
/// </remarks>
public readonly struct ValueStopwatch
{
    private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

    private readonly long startTimestamp;

    /// <summary>
    /// Флаг отражающий активность таймера
    /// </summary>
    public bool IsActive => startTimestamp is not 0;

    private ValueStopwatch(long startTimestamp) => this.startTimestamp = startTimestamp;

    /// <summary>
    /// Запустить новый таймер
    /// </summary>
    public static ValueStopwatch StartNew() => new(Stopwatch.GetTimestamp());

    /// <summary>
    /// Получить информацию о только сколько прошло времени
    /// </summary>
    public TimeSpan GetElapsedTime()
    {
        // Start timestamp can't be zero in an initialized ValueStopwatch. It would have to be literally the first thing executed when the machine boots to be 0.
        // So it being 0 is a clear indication of default(ValueStopwatch)
        if (!IsActive)
        {
            throw new InvalidOperationException("An uninitialized, or 'default', ValueStopwatch cannot be used to get elapsed time.");
        }

        long end = Stopwatch.GetTimestamp();
        long timestampDelta = end - startTimestamp;
        long ticks = (long)(TimestampToTicks * timestampDelta);
        return new TimeSpan(ticks);
    }
}
