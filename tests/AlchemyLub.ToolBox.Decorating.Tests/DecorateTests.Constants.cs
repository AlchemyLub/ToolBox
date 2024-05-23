namespace AlchemyLub.ToolBox.Decorating.UnitTests;

/// <summary>
/// Константы для тестов на <see cref="ServiceCollectionExtensions.Decorate{TInterface,TDecorator}"/>
/// </summary>
public partial class DecorateTests
{
    private const string SimpleResult = $"{Service.Result}->{nameof(ServiceDecorator)}";
    private const string AnotherSimpleResult = $"{AnotherService.Result}->{nameof(ServiceDecorator)}";

    private const string GenericServiceName = nameof(GenericService<int>);
    private const string AnotherGenericServiceName = nameof(AnotherGenericService<int>);
    private const string DoubleGenericServiceName = nameof(DoubleGenericService<int, string>);
    private const string DoubleAnotherGenericServiceName = nameof(AnotherDoubleGenericService<int, string>);

    private static readonly string GenericArgument = $"[{typeof(int)}]";
    private static readonly string GenericArguments = $"[{typeof(int)},{typeof(string)}]";
    private const string GenericDecoratorName = nameof(GenericServiceDecorator<int>);
    private const string DoubleGenericDecoratorName = nameof(DoubleGenericServiceDecorator<int, string>);

    private static readonly string GenericResult =
        $"{GenericServiceName}{GenericArgument}->{GenericDecoratorName}{GenericArgument}";
    private static readonly string AnotherGenericResult =
        $"{AnotherGenericServiceName}{GenericArgument}->{GenericDecoratorName}{GenericArgument}";

    private static readonly string DoubleGenericResult =
        $"{DoubleGenericServiceName}{GenericArguments}->{DoubleGenericDecoratorName}{GenericArguments}";
    private static readonly string AnotherDoubleGenericResult =
        $"{DoubleAnotherGenericServiceName}{GenericArguments}->{DoubleGenericDecoratorName}{GenericArguments}";
}
