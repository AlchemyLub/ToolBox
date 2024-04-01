namespace FloralHub.ToolBox.Decorating.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при отсутствии зарегистрированных имплементаций сервиса
/// </summary>
internal class DecoratingServicesNotFoundException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DecoratingServicesNotFoundException"/>
    /// </summary>
    internal DecoratingServicesNotFoundException(Type interfaceType)
        : base($"Отсутствуют зарегистрированные имплементации интерфейса {interfaceType}")
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DecoratingServicesNotFoundException"/>
    /// </summary>
    protected internal DecoratingServicesNotFoundException(string message) : base(message)
    {
    }
}

/// <summary>
/// Исключение, выбрасываемое при отсутствии зарегистрированных имплементаций <typeparamref name="TInterface"/>
/// </summary>
/// <typeparam name="TInterface">Контракт декорируемого сервиса</typeparam>
internal sealed class DecoratingServicesNotFoundException<TInterface> : DecoratingServicesNotFoundException
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DecoratingServicesNotFoundException{TInterface}"/>
    /// </summary>
    internal DecoratingServicesNotFoundException()
        : base($"Отсутствуют зарегистрированные имплементации интерфейса {typeof(TInterface)}")
    {
    }
}
