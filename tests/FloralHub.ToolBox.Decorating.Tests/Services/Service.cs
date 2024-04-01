namespace FloralHub.ToolBox.Decorating.UnitTests.Services;

/// <inheritdoc cref="IService"/>
internal sealed class Service : IService
{
    internal const string Result = $"{nameof(IService)}->{nameof(Service)}";

    /// <inheritdoc />
    public string GetName() => Result;
}

/// <inheritdoc cref="IGenericService{T}"/>
internal sealed class GenericService<T> : IGenericService<T>
{
    internal const string Result = $"{nameof(IGenericService<T>)}->{nameof(GenericService<T>)}";

    /// <inheritdoc />
    public string GetName() => Result;
}

/// <inheritdoc cref="IDoubleGenericService{T, TU}"/>
internal sealed class DoubleGenericService<T, TU> : IDoubleGenericService<T, TU>
{
    internal const string Result = $"{nameof(IDoubleGenericService<T, TU>)}->{nameof(DoubleGenericService<T, TU>)}";

    /// <inheritdoc />
    public string GetName() => Result;
}
