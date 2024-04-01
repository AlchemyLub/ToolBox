using Microsoft.Extensions.DependencyInjection;

namespace FloralHub.ToolBox.Decorating.UnitTests;

/// <summary>
/// Тесты для <see cref="ServiceCollectionExtensions.Decorate{TInterface, TImplemetation}"/>
/// </summary>
public class DecorateTests
{
    private const string DecoratorPath = $"{Service.Result}->{nameof(ServiceDecorator)}";
    private const string AnotherDecoratorPath = $"{AnotherService.Result}->{nameof(ServiceDecorator)}";

    private const string GenericIntDecoratorPath =
        $"{GenericService<int>.Result}->{nameof(GenericServiceDecorator<int>)}";
    private const string AnotherGenericIntDecoratorPath =
        $"{AnotherGenericService<int>.Result}->{nameof(GenericServiceDecorator<int>)}";

    private const string DoubleGenericIntDecoratorPath =
        $"{DoubleGenericService<int, int>.Result}->{nameof(DoubleGenericServiceDecorator<int, int>)}";
    private const string DoubleAnotherGenericIntDecoratorPath =
        $"{AnotherDoubleGenericService<int, int>.Result}->{nameof(DoubleGenericServiceDecorator<int, int>)}";

    /// <summary>
    /// Проверяет выбрасывание исключения, когда нечего декорировать
    /// </summary>
    [Fact]
    public void Decorate_WithoutRegisteredService_Exception()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        Func<IServiceCollection> result = serviceCollection.Decorate<IService, ServiceDecorator>;

        Assert.Throws<DecoratingServicesNotFoundException<IService>>(result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда сервис зарегистрирован через интерфейс и реализацию
    /// </summary>
    [Fact]
    public void Decorate_WithDefaultRegisteredService_CorrectResult()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IService, Service>();
        serviceCollection.Decorate<IService, ServiceDecorator>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IService service = serviceProvider.GetRequiredService<IService>();

        string result = service.GetName();

        Assert.Equal(DecoratorPath, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда несколько сервисов зарегистрированы через интерфейс и реализацию
    /// </summary>
    [Fact]
    public void Decorate_WithDefaultRegisteredServices_CorrectResult()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IService, Service>();
        serviceCollection.AddTransient<IService, AnotherService>();
        serviceCollection.Decorate<IService, ServiceDecorator>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IEnumerable<IService> services = serviceProvider.GetRequiredService<IEnumerable<IService>>();

        IEnumerable<string> result = services.Select(static service => service.GetName());

        Assert.Equal(new List<string> { DecoratorPath, AnotherDecoratorPath }, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда сервис зарегистрирован через фабрику
    /// </summary>
    [Fact]
    public void Decorate_WithSingleRegisteredServices_CorrectResult()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddTransient<IService>(_ => new Service());
        services.Decorate<IService, ServiceDecorator>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IService service = serviceProvider.GetRequiredService<IService>();

        string result = service.GetName();

        Assert.Equal(DecoratorPath, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда несколько сервисов зарегистрированы через фабрику
    /// </summary>
    [Fact]
    public void Decorate_WithSingleRegisteredService_CorrectResult()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IService>(_ => new Service());
        serviceCollection.AddTransient<IService>(_ => new AnotherService());
        serviceCollection.Decorate<IService, ServiceDecorator>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IEnumerable<IService> services = serviceProvider.GetRequiredService<IEnumerable<IService>>();

        IEnumerable<string> result = services.Select(static service => service.GetName());

        Assert.Equal(new List<string> { DecoratorPath, AnotherDecoratorPath }, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда сервис зарегистрирован через передачу типов
    /// </summary>
    [Fact]
    public void Decorate_WithGenericTypedRegisteredService_CorrectResult()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(typeof(IGenericService<>), typeof(GenericService<>));
        serviceCollection.Decorate(typeof(IGenericService<>), typeof(GenericServiceDecorator<>));
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IGenericService<int> service = serviceProvider.GetRequiredService<IGenericService<int>>();

        string result = service.GetName();

        Assert.Equal(GenericIntDecoratorPath, result);
    }

    /// <summary>
    /// Проверяет корректную работу декоратора, когда несколько сервисов зарегистрированы через передачу типов
    /// </summary>
    [Fact]
    public void Decorate_WithGenericTypedRegisteredServices_CorrectResult()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(typeof(IGenericService<>), typeof(GenericService<>));
        serviceCollection.AddTransient(typeof(IGenericService<>), typeof(AnotherGenericService<>));
        serviceCollection.Decorate(typeof(IGenericService<>), typeof(GenericServiceDecorator<>));
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IEnumerable<IGenericService<int>> services = serviceProvider.GetRequiredService<IEnumerable<IGenericService<int>>>();

        IEnumerable<string> result = services.Select(static service => service.GetName());

        Assert.Equal(new List<string> { GenericIntDecoratorPath, AnotherGenericIntDecoratorPath }, result);
    }
}
