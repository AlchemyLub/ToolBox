namespace FloralHub.ToolBox.Decorating.UnitTests;

/// <summary>
/// Тесты для <see cref="ServiceCollectionExtensions.Decorate{TInterface, TImplemetation}"/> при декорировании простых типов
/// </summary>
public partial class DecorateTests
{
    /// <summary>
    /// Проверяет выбрасывание исключения, когда нечего декорировать
    /// </summary>
    [Fact]
    public void Decorate_WithoutRegisteredService_Exception() =>
        Assert.Throws<DecoratingServicesNotFoundException>(() => ConfigureProvider(services =>
            services.Decorate<IService, ServiceDecorator>()));

    /// <summary>
    /// Проверяет корректную работу декоратора, когда сервис зарегистрирован через интерфейс и реализацию
    /// </summary>
    [Fact]
    public void Decorate_WithDefaultRegisteredService_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddSingleton<IService, Service>();
            services.Decorate<IService, ServiceDecorator>();
        });
        IService service = serviceProvider.GetRequiredService<IService>();

        string result = service.GetName();

        Assert.IsType<ServiceDecorator>(service);
        Assert.Equal(SimpleResult, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда несколько сервисов зарегистрированы через интерфейс и реализацию
    /// </summary>
    [Fact]
    public void Decorate_WithDefaultRegisteredServices_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IService, Service>();
            services.AddTransient<IService, AnotherService>();
            services.Decorate<IService, ServiceDecorator>();
        });
        IEnumerable<IService> services = serviceProvider.GetRequiredService<IEnumerable<IService>>();

        IEnumerable<string> result = services.Select(static service => service.GetName());

        Assert.All(services, service => Assert.IsType<ServiceDecorator>(service));
        Assert.Equal(new List<string> { SimpleResult, AnotherSimpleResult }, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда сервис зарегистрирован через фабрику
    /// </summary>
    [Fact]
    public void Decorate_WithFactoryRegisteredServices_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IService>(_ => new Service());
            services.Decorate<IService, ServiceDecorator>();
        });
        IService service = serviceProvider.GetRequiredService<IService>();

        string result = service.GetName();

        Assert.IsType<ServiceDecorator>(service);
        Assert.Equal(SimpleResult, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда несколько сервисов зарегистрированы через фабрику
    /// </summary>
    [Fact]
    public void Decorate_WithFactoryRegisteredService_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IService>(_ => new Service());
            services.AddTransient<IService>(_ => new AnotherService());
            services.Decorate<IService, ServiceDecorator>();
        });
        IEnumerable<IService> services = serviceProvider.GetRequiredService<IEnumerable<IService>>();

        IEnumerable<string> result = services.Select(static service => service.GetName());

        Assert.All(services, service => Assert.IsType<ServiceDecorator>(service));
        Assert.Equal(new List<string> { SimpleResult, AnotherSimpleResult }, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора с множественной вложенностью,
    /// если сервис зарегистрирован через интерфейс и реализацию
    /// </summary>
    [Fact]
    public void MultiplyDecorate_WithSingleRegisteredService_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddSingleton<IMultiplyService, MultiplyService>();
            services.Decorate<IMultiplyService, MultiplyServiceDecorator>();
            services.Decorate<IMultiplyService, AnotherMultiplyServiceDecorator>();
        });

        IMultiplyService service = serviceProvider.GetRequiredService<IMultiplyService>();

        AnotherMultiplyServiceDecorator anotherDecorator = Assert.IsType<AnotherMultiplyServiceDecorator>(service);
        MultiplyServiceDecorator decorator = Assert.IsType<MultiplyServiceDecorator>(anotherDecorator.InnerService);
        Assert.IsType<MultiplyService>(decorator.InnerService);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора с множественной вложенностью,
    /// если несколько сервисов зарегистрировано через интерфейс и реализацию
    /// </summary>
    [Fact]
    public void MultiplyDecorate_WithSingleRegisteredServices_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddSingleton<IMultiplyService, MultiplyService>();
            services.AddSingleton<IMultiplyService, AnotherMultiplyService>();
            services.Decorate<IMultiplyService, MultiplyServiceDecorator>();
            services.Decorate<IMultiplyService, AnotherMultiplyServiceDecorator>();
        });

        IEnumerable<IMultiplyService> services = serviceProvider.GetRequiredService<IEnumerable<IMultiplyService>>();

        // TODO: Сократить!
        Assert.All(
            services,
            service => Assert.IsType<AnotherMultiplyServiceDecorator>(service));
        IEnumerable<IMultiplyService> anotherMultiplyServiceDecorators =
            services.Select(t => ((AnotherMultiplyServiceDecorator)t).InnerService);
        Assert.All(
            anotherMultiplyServiceDecorators,
            service => Assert.IsType<MultiplyServiceDecorator>(service));
        IEnumerable<IMultiplyService> multiplyServiceDecorators =
            anotherMultiplyServiceDecorators.Select(t => ((MultiplyServiceDecorator)t).InnerService);
        Assert.Contains(multiplyServiceDecorators, t => t is AnotherMultiplyService);
        Assert.Contains(multiplyServiceDecorators, t => t is MultiplyService);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора с множественной вложенностью,
    /// если сервис зарегистрирован через фабрику
    /// </summary>
    [Fact]
    public void MultiplyDecorate_WithFactoryRegisteredServices_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IMultiplyService>(_ => new MultiplyService());
            services.Decorate<IMultiplyService, MultiplyServiceDecorator>();
            services.Decorate<IMultiplyService, AnotherMultiplyServiceDecorator>();
        });

        IMultiplyService service = serviceProvider.GetRequiredService<IMultiplyService>();

        AnotherMultiplyServiceDecorator anotherDecorator = Assert.IsType<AnotherMultiplyServiceDecorator>(service);
        MultiplyServiceDecorator decorator = Assert.IsType<MultiplyServiceDecorator>(anotherDecorator.InnerService);
        Assert.IsType<MultiplyService>(decorator.InnerService);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора с множественной вложенностью,
    /// если несколько сервисов зарегистрировано через фабрику
    /// </summary>
    [Fact]
    public void MultiplyDecorate_WithFactoryRegisteredService_CorrectResult()
    {
        ServiceProvider serviceProvider = ConfigureProvider(services =>
        {
            services.AddTransient<IMultiplyService>(_ => new MultiplyService());
            services.AddTransient<IMultiplyService>(_ => new AnotherMultiplyService());
            services.Decorate<IMultiplyService, MultiplyServiceDecorator>();
            services.Decorate<IMultiplyService, AnotherMultiplyServiceDecorator>();
        });

        IEnumerable<IMultiplyService> services = serviceProvider.GetRequiredService<IEnumerable<IMultiplyService>>();

        // TODO: Сократить!
        Assert.All(
            services,
            service => Assert.IsType<AnotherMultiplyServiceDecorator>(service));
        IEnumerable<IMultiplyService> anotherMultiplyServiceDecorators =
            services.Select(t => ((AnotherMultiplyServiceDecorator)t).InnerService);
        Assert.All(
            anotherMultiplyServiceDecorators,
            service => Assert.IsType<MultiplyServiceDecorator>(service));
        IEnumerable<IMultiplyService> multiplyServiceDecorators =
            anotherMultiplyServiceDecorators.Select(t => ((MultiplyServiceDecorator)t).InnerService);
        Assert.Contains(multiplyServiceDecorators, t => t is AnotherMultiplyService);
        Assert.Contains(multiplyServiceDecorators, t => t is MultiplyService);
    }
}
