namespace FloralHub.ToolBox.Decorating.UnitTests;

/// <summary>
/// Тесты для <see cref="ServiceCollectionExtensions.Decorate{TInterface, TImplemetation}"/> при декорировании обобщённых типов
/// </summary>
public partial class DecorateTests
{
    /// <summary>
    /// Проверяет корректную работу декоратора, когда обобщённый сервис зарегистрирован через передачу типов
    /// </summary>
    [Fact]
    public void Decorate_WithGenericTypedRegisteredService_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IGenericService<int>, GenericService<int>>();
            services.Decorate(typeof(IGenericService<>), typeof(GenericServiceDecorator<>));
        });
        IGenericService<int> service = serviceProvider.GetRequiredService<IGenericService<int>>();

        string result = service.GetName();

        Assert.IsType<GenericServiceDecorator<int>>(service);
        Assert.Equal(GenericResult, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда несколько обобщённых сервисов зарегистрированы через передачу типов
    /// </summary>
    [Fact]
    public void Decorate_WithGenericTypedRegisteredServices_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IGenericService<int>, GenericService<int>>();
            services.AddTransient<IGenericService<int>, AnotherGenericService<int>>();
            services.Decorate(typeof(IGenericService<>), typeof(GenericServiceDecorator<>));
        });
        IEnumerable<IGenericService<int>> services =
            serviceProvider.GetRequiredService<IEnumerable<IGenericService<int>>>();

        IEnumerable<string> result = services.Select(static service => service.GetName());

        Assert.All(
            services,
            service => Assert.IsType<GenericServiceDecorator<int>>(service));
        Assert.Equal(
            new List<string> { GenericResult, AnotherGenericResult },
            result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора,
    /// когда обобщённый сервис c несколькими параметрами зарегистрирован через передачу типов
    /// </summary>
    [Fact]
    public void Decorate_WithDoubleGenericTypedRegisteredService_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IDoubleGenericService<int, string>, DoubleGenericService<int, string>>();
            services.Decorate(
                typeof(IDoubleGenericService<,>),
                typeof(DoubleGenericServiceDecorator<,>));
        });
        IDoubleGenericService<int, string> service =
            serviceProvider.GetRequiredService<IDoubleGenericService<int, string>>();

        string result = service.GetName();

        Assert.IsType<DoubleGenericServiceDecorator<int, string>>(service);
        Assert.Equal(DoubleGenericResult, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора,
    /// когда несколько обобщённых сервисов c несколькими параметрами зарегистрированы через передачу типов
    /// </summary>
    [Fact]
    public void Decorate_WithDoubleGenericTypedRegisteredServices_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IDoubleGenericService<int, string>, DoubleGenericService<int, string>>();
            services.AddTransient<IDoubleGenericService<int, string>, AnotherDoubleGenericService<int, string>>();
            services.Decorate(
                typeof(IDoubleGenericService<,>),
                typeof(DoubleGenericServiceDecorator<,>));
        });
        IEnumerable<IDoubleGenericService<int, string>> services =
            serviceProvider.GetRequiredService<IEnumerable<IDoubleGenericService<int, string>>>();

        IEnumerable<string> result = services.Select(static service => service.GetName());

        Assert.All(
            services,
            service => Assert.IsType<DoubleGenericServiceDecorator<int, string>>(service));
        Assert.Equal(
            new List<string> { DoubleGenericResult, AnotherDoubleGenericResult },
            result);
    }
}
