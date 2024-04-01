namespace FloralHub.ToolBox.Decorating.UnitTests.Services;

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
/// <param name="service"><see cref="IGenericService{T}"/></param>
public sealed class GenericServiceDecorator<T>(IGenericService<T> service) : IGenericService<T>
{
    /// <inheritdoc />
    public string GetName() => $"{service.GetName()}->{nameof(GenericServiceDecorator<T>)}";
}

/// <summary>
/// Тестовый декоратор
/// </summary>
/// <param name="service"><see cref="IDoubleGenericService{T, TU}"/></param>
internal sealed class DoubleGenericServiceDecorator<T, TU>(IDoubleGenericService<T, TU> service)
    : IDoubleGenericService<T, TU>
{
    /// <inheritdoc />
    public string GetName() => $"{service.GetName()}->{nameof(DoubleGenericServiceDecorator<T, TU>)}";
}
