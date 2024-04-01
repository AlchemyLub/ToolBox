namespace FloralHub.ToolBox.Decorating;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Декорирует сервисы, имплементирующие <typeparamref name="TInterface"/>
    /// </summary>
    /// <typeparam name="TInterface">Интерфейс декорируемого сервиса</typeparam>
    /// <typeparam name="TDecorator">Реализация декоратора</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    /// <exception cref="DecoratingServicesNotFoundException"/>
    public static IServiceCollection Decorate<TInterface, TDecorator>(this IServiceCollection services)
        where TInterface : class
        where TDecorator : class, TInterface
    {
        Type interfaceType = typeof(TInterface);

        List<ServiceDescriptor> wrappedDescriptors = services
            .Where(t => t.ServiceType == interfaceType)
            .ToList();

        if (wrappedDescriptors.Count == 0)
        {
            throw new DecoratingServicesNotFoundException(interfaceType);
        }

        foreach (ServiceDescriptor wrappedDescriptor in wrappedDescriptors)
        {
            Func<IServiceProvider, object> factory = CreateFactory(
                typeof(TDecorator),
                wrappedDescriptor);

            services.Replace(ServiceDescriptor.Describe(
                    interfaceType,
                    factory,
                wrappedDescriptor.Lifetime));
        }

        return services;
    }

    /// <summary>
    /// Декорирует сервисы, имплементирующие интерфейс с переданным типом
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="interfaceType">Интерфейс декорируемого сервиса</param>
    /// <param name="decoratorType">Реализация декоратора</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    /// <exception cref="DecoratingServicesNotFoundException"/>
    public static IServiceCollection Decorate(
        this IServiceCollection services,
        Type interfaceType,
        Type decoratorType)
    {
        List<ServiceDescriptor> wrappedDescriptors = services
            .Where(t => t.ServiceType == interfaceType)
            .ToList();

        if (wrappedDescriptors.Count == 0)
        {
            throw new DecoratingServicesNotFoundException(interfaceType);
        }

        foreach (ServiceDescriptor wrappedDescriptor in wrappedDescriptors)
        {
            Func<IServiceProvider, object> factory = CreateFactory(
                decoratorType,
                wrappedDescriptor);

            services.Replace(ServiceDescriptor.Describe(
                    interfaceType,
                    factory,
                wrappedDescriptor.Lifetime));
        }

        return services;
    }

    ///// <summary>
    ///// Декорирует сервисы, имплементирующие интерфейс <typeparamref name="TService"/> используя фабрику
    ///// </summary>
    ///// <param name="services"><see cref="IServiceCollection"/></param>
    ///// <param name="decoratorFactory">Фабрика инициализирующая декоратор</param>
    ///// <returns><see cref="IServiceCollection"/></returns>
    ///// <exception cref="DecoratingServicesNotFoundException"/>
    //public static IServiceCollection Decorate<TService>(
    //    this IServiceCollection services,
    //    Func<TService, TService> decoratorFactory)
    //    where TService : notnull
    //{
    //    List<ServiceDescriptor> wrappedDescriptors = services
    //        .Where(t => t.ServiceType == interfaceType)
    //        .ToList();

    //    if (wrappedDescriptors.Count == 0)
    //    {
    //        throw new DecoratingServicesNotFoundException(interfaceType);
    //    }

    //    foreach (ServiceDescriptor wrappedDescriptor in wrappedDescriptors)
    //    {
    //        Func<IServiceProvider, object> factory = CreateFactory(
    //            decoratorType,
    //            wrappedDescriptor);

    //        services.Replace(ServiceDescriptor.Describe(
    //            interfaceType,
    //            factory,
    //            wrappedDescriptor.Lifetime));
    //    }

    //    return services;
    //}

    ///// <summary>
    ///// Декорирует сервисы, имплементирующие интерфейс <typeparamref name="TService"/> используя переданную имплементацию
    ///// </summary>
    ///// <param name="services"><see cref="IServiceCollection"/></param>
    ///// <param name="decorator">Имплементация декоратора</param>
    ///// <returns><see cref="IServiceCollection"/></returns>
    ///// <exception cref="DecoratingServicesNotFoundException"/>
    //public static IServiceCollection Decorate<TService>(this IServiceCollection services, TService decorator)
    //    where TService : notnull
    //{
    //    List<ServiceDescriptor> wrappedDescriptors = services
    //        .Where(t => t.ServiceType == interfaceType)
    //        .ToList();

    //    if (wrappedDescriptors.Count == 0)
    //    {
    //        throw new DecoratingServicesNotFoundException(interfaceType);
    //    }

    //    foreach (ServiceDescriptor wrappedDescriptor in wrappedDescriptors)
    //    {
    //        Func<IServiceProvider, object> factory = CreateFactory(
    //            decoratorType,
    //            wrappedDescriptor);

    //        services.Replace(ServiceDescriptor.Describe(
    //            interfaceType,
    //            factory,
    //            wrappedDescriptor.Lifetime));
    //    }

    //    return services;
    //}

    private static Func<IServiceProvider, object> CreateFactory(
        Type decoratorType,
        ServiceDescriptor currentDescriptor)
    {
        if (currentDescriptor.ImplementationInstance is not null)
        {
            if (currentDescriptor.ServiceType.IsGenericTypeDefinition)
            {
                Type[] genericArguments = currentDescriptor.ServiceType.GetGenericArguments();
                Type closedDecorator = decoratorType.MakeGenericType(genericArguments);

                return serviceProvider => ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    closedDecorator,
                    currentDescriptor.ImplementationInstance);
            }

            return serviceProvider => ActivatorUtilities.CreateInstance(
                serviceProvider,
                decoratorType,
                currentDescriptor.ImplementationInstance);
        }

        if (currentDescriptor.ImplementationFactory is not null)
        {
            return serviceProvider => ActivatorUtilities.CreateInstance(
                serviceProvider,
                decoratorType,
                currentDescriptor.ImplementationFactory(serviceProvider));
        }

        // TODO: Тут надо сделать IF для дженерика.
        if (currentDescriptor.ImplementationType is not null)
        {
            return serviceProvider =>
            {
                object service = ActivatorUtilities.GetServiceOrCreateInstance(
                    serviceProvider,
                    currentDescriptor.ImplementationType);

                return ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    decoratorType,
                    service);
            };
        }

        throw new NotImplementedException("Нужная корректная ошибка!");
    }

    private static Func<IServiceProvider, object> CreateFactory(
        Func<object, IServiceProvider, object> decoratorFactory,
        ServiceDescriptor currentDescriptor)
    {
        if (currentDescriptor.ImplementationInstance is not null)
        {
            return serviceProvider =>
                decoratorFactory(currentDescriptor.ImplementationInstance, serviceProvider);
        }

        if (currentDescriptor.ImplementationFactory is not null)
        {
            return serviceProvider =>
                decoratorFactory(currentDescriptor.ImplementationFactory(serviceProvider), serviceProvider);
        }

        if (currentDescriptor.ImplementationType is not null)
        {
            return serviceProvider =>
            {
                object service = ActivatorUtilities.GetServiceOrCreateInstance(
                    serviceProvider,
                    currentDescriptor.ImplementationType);

                return decoratorFactory(service, serviceProvider);
            };
        }

        throw new NotImplementedException("Нужная корректная ошибка!");
    }

    public static IServiceCollection Replace(
        this IServiceCollection collection,
        ServiceDescriptor descriptor)
    {
        int count = collection.Count;
        for (int index = 0; index < count; ++index)
        {
            if (collection[index].ServiceType == descriptor.ServiceType && Equals(collection[index].ServiceKey, descriptor.ServiceKey))
            {
                collection.RemoveAt(index);
                break;
            }
        }
        collection.Add(descriptor);
        return collection;
    }
}
