namespace FloralHub.ToolBox.Decorating.UnitTests.Services;

/// <inheritdoc cref="IService"/>
internal sealed class AnotherService : IService
{
    internal const string Result = $"{nameof(IService)}->{nameof(AnotherService)}";

    /// <inheritdoc />
    public string GetName() => Result;
}

/// <inheritdoc cref="IGenericService{T}"/>
internal sealed class AnotherGenericService<T> : IGenericService<T>
{
    internal const string Result = $"{nameof(IGenericService<T>)}->{nameof(AnotherGenericService<T>)}";

    /// <inheritdoc />
    public string GetName() => Result;
}

/// <inheritdoc cref="IDoubleGenericService{T, TU}"/>
internal sealed class AnotherDoubleGenericService<T, TU> : IDoubleGenericService<T, TU>
{
    internal const string Result =
        $"{nameof(IDoubleGenericService<T, TU>)}->{nameof(AnotherDoubleGenericService<T, TU>)}";

    /// <inheritdoc />
    public string GetName() => Result;
}
