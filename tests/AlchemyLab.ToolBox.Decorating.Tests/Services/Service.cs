namespace AlchemyLab.ToolBox.Decorating.UnitTests.Services;

/// <inheritdoc cref="IService"/>
internal sealed class Service : IService
{
    internal const string Result = nameof(Service);

    /// <inheritdoc />
    public string GetName() => Result;
}

/// <inheritdoc cref="IGenericService{T}"/>
internal sealed class GenericService<T> : IGenericService<T>
{
    internal const string Result = nameof(GenericService<T>);

    /// <inheritdoc />
    public string GetName() => $"{Result}[{typeof(T)}]";
}

/// <inheritdoc cref="IDoubleGenericService{T, TU}"/>
internal sealed class DoubleGenericService<T, TU> : IDoubleGenericService<T, TU>
{
    internal const string Result = nameof(DoubleGenericService<T, TU>);

    /// <inheritdoc />
    public string GetName() => $"{Result}[{typeof(T)},{typeof(TU)}]";
}

/// <inheritdoc cref="IMultiplyService"/>
internal sealed class MultiplyService : IMultiplyService;
