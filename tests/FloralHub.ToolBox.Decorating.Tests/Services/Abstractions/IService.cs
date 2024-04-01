namespace FloralHub.ToolBox.Decorating.UnitTests.Services.Abstractions;

/// <summary>
/// Тестовый сервис.
/// </summary>
internal interface IService
{
    /// <summary>
    /// Получить имя сервиса
    /// </summary>
    /// <returns>Тестовое имя</returns>
    internal string GetName();
}

/// <summary>
/// Тестовый сервис.
/// </summary>
public interface IGenericService<T>
{
    /// <summary>
    /// Получить имя сервиса
    /// </summary>
    /// <returns>Тестовое имя</returns>
    internal string GetName();
}

/// <summary>
/// Тестовый сервис.
/// </summary>
public interface IDoubleGenericService<T, TU>
{
    /// <summary>
    /// Получить имя сервиса
    /// </summary>
    /// <returns>Тестовое имя</returns>
    internal string GetName();
}
