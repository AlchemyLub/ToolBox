namespace AlchemyLub.ToolBox.Decorating.UnitTests.Services;

/// <summary>
/// Тестовый декоратор
/// </summary>
/// <param name="service"><see cref="IService"/></param>
internal sealed class ServiceDecorator(IService service) : IService
{
    /// <inheritdoc />
    public string GetName() => $"{service.GetName()}->{nameof(ServiceDecorator)}";
}

/// <summary>
/// Тестовый декоратор
/// </summary>
/// <typeparam name="T">Тестовый тип</typeparam>
/// <param name="service"><see cref="IGenericService{T}"/></param>
internal sealed class GenericServiceDecorator<T>(IGenericService<T> service) : IGenericService<T>
{
    /// <inheritdoc />
    public string GetName() => $"{service.GetName()}->{nameof(GenericServiceDecorator<T>)}[{typeof(T)}]";
}

/// <summary>
/// Тестовый декоратор
/// </summary>
/// <typeparam name="T">Тестовый тип</typeparam>
/// <typeparam name="TU">Тестовый тип</typeparam>
/// <param name="service"><see cref="IDoubleGenericService{T, TU}"/></param>
internal sealed class DoubleGenericServiceDecorator<T, TU>(IDoubleGenericService<T, TU> service)
    : IDoubleGenericService<T, TU>
{
    /// <inheritdoc />
    public string GetName() =>
        $"{service.GetName()}->{nameof(DoubleGenericServiceDecorator<T, TU>)}[{typeof(T)},{typeof(TU)}]";
}

/// <summary>
/// Тестовый декоратор
/// </summary>
/// <param name="service"><see cref="IMultiplyService"/></param>
internal sealed class MultiplyServiceDecorator(IMultiplyService service) : IMultiplyService
{
    public IMultiplyService InnerService { get; } = service;
}

/// <summary>
/// Тестовый декоратор
/// </summary>
/// <param name="service"><see cref="IMultiplyService"/></param>
internal sealed class AnotherMultiplyServiceDecorator(IMultiplyService service) : IMultiplyService
{
    public IMultiplyService InnerService { get; } = service;
}
