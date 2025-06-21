namespace AlchemyLab.ToolBox.Decorating.UnitTests.Services.Abstractions;

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
/// <typeparam name="T">Тестовый тип</typeparam>
internal interface IGenericService<T>
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
/// <typeparam name="T">Тестовый тип</typeparam>
/// <typeparam name="TU">Тестовый тип</typeparam>
internal interface IDoubleGenericService<T, TU>
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
internal interface IMultiplyService;
