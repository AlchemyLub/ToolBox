namespace AlchemyLub.ToolBox.Decorating.UnitTests.Services;

/// <inheritdoc cref="IService"/>
internal sealed class AnotherService : IService
{
    internal const string Result = nameof(AnotherService);

    /// <inheritdoc />
    public string GetName() => Result;
}

/// <inheritdoc cref="IGenericService{T}"/>
internal sealed class AnotherGenericService<T> : IGenericService<T>
{
    internal const string Result = nameof(AnotherGenericService<T>);

    /// <inheritdoc />
    public string GetName() => $"{Result}[{typeof(T)}]";
}

/// <inheritdoc cref="IDoubleGenericService{T, TU}"/>
internal sealed class AnotherDoubleGenericService<T, TU> : IDoubleGenericService<T, TU>
{
    internal const string Result = nameof(AnotherDoubleGenericService<T, TU>);

    /// <inheritdoc />
    public string GetName() => $"{Result}[{typeof(T)},{typeof(TU)}]";
}

/// <inheritdoc cref="IMultiplyService"/>
internal sealed class AnotherMultiplyService : IMultiplyService;
