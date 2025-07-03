namespace AlchemyLab.ToolBox.Resilience.Retry.Extensions;

/// <summary>
/// Методы расширения для добавления возможности повторного выполнения функций и действий
/// </summary>
public static class RetryFuncExtensions
{
    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="retryOptions">Настройки повторов. Если null, используются настройки по умолчанию</param>
    //public static TResult Retry<TResult>(this Func<TResult> func, RetryStrategyOptions? retryOptions = null)
    //{
    //    ResiliencePipeline? pipeline = null;

    //    if (retryOptions is null)
    //    {
    //        pipeline = new ResiliencePipelineBuilder()
    //            .AddRetry(RetryDefaults.DefaultOptions)
    //            .Build();
    //    }

    //    pipeline ??= RetryDefaults.DefaultPipeline;

    //    return pipeline.Execute(func);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="retryOptions">Настройки повторов. Если null, используются настройки по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    //public static async Task<TResult> Retry<TResult>(
    //    this Func<Task<TResult>> func,
    //    RetryStrategyOptions? retryOptions = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    ResiliencePipeline? pipeline = null;

    //    if (retryOptions is null)
    //    {
    //        pipeline = new ResiliencePipelineBuilder()
    //            .AddRetry(RetryDefaults.DefaultOptions)
    //            .Build();
    //    }

    //    pipeline ??= RetryDefaults.DefaultPipeline;

    //    return await pipeline.ExecuteAsync(async ct => await func(), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="retryOptions">Настройки повторов. Если null, используются настройки по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    //public static async Task<TResult> Retry<TResult>(
    //    this Func<CancellationToken, Task<TResult>> func,
    //    RetryStrategyOptions? retryOptions = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    ResiliencePipeline? pipeline = null;

    //    if (retryOptions is null)
    //    {
    //        pipeline = new ResiliencePipelineBuilder()
    //            .AddRetry(RetryDefaults.DefaultOptions)
    //            .Build();
    //    }

    //    pipeline ??= RetryDefaults.DefaultPipeline;

    //    return await pipeline.ExecuteAsync(async ct => await func(ct), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <param name="action">Действие для выполнения</param>
    ///// <param name="retryOptions">Настройки повторов. Если null, используются настройки по умолчанию</param>
    //public static void Retry(this Action action, RetryStrategyOptions? retryOptions = null)
    //{
    //    ResiliencePipeline? pipeline = null;

    //    if (retryOptions is null)
    //    {
    //        pipeline = new ResiliencePipelineBuilder()
    //            .AddRetry(RetryDefaults.DefaultOptions)
    //            .Build();
    //    }

    //    pipeline ??= RetryDefaults.DefaultPipeline;

    //    pipeline.Execute(action);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <param name="func">Асинхронное действие для выполнения</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача выполнения действия</returns>
    //public static Task Retry(this Func<Task> func, ResiliencePipeline? pipeline = null, CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T">Тип параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg">Параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <returns>Результат выполнения функции</returns>
    //public static TResult Retry<T, TResult>(this Func<T, TResult> func, T arg, ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.Execute(() => func(arg));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T">Тип параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg">Параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача с результатом выполнения функции</returns>
    //public static Task<TResult> Retry<T, TResult>(
    //    this Func<T, Task<TResult>> func,
    //    T arg,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T">Тип параметра действия</typeparam>
    ///// <param name="action">Действие для выполнения</param>
    ///// <param name="arg">Параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    //public static void Retry<T>(this Action<T> action, T arg, ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    pipeline.Execute(() => action(arg));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T">Тип параметра действия</typeparam>
    ///// <param name="func">Асинхронное действие для выполнения</param>
    ///// <param name="arg">Параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача выполнения действия</returns>
    //public static Task Retry<T>(
    //    this Func<T, Task> func,
    //    T arg,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <returns>Результат выполнения функции</returns>
    //public static TResult Retry<T1, T2, TResult>(
    //    this Func<T1, T2, TResult> func,
    //    T1 arg1,
    //    T2 arg2,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.Execute(() => func(arg1, arg2));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача с результатом выполнения функции</returns>
    //public static Task<TResult> Retry<T1, T2, TResult>(
    //    this Func<T1, T2, Task<TResult>> func,
    //    T1 arg1,
    //    T2 arg2,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <param name="action">Действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    //public static void Retry<T1, T2>(
    //    this Action<T1, T2> action,
    //    T1 arg1,
    //    T2 arg2,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    pipeline.Execute(() => action(arg1, arg2));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <param name="func">Асинхронное действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача выполнения действия</returns>
    //public static Task Retry<T1, T2>(
    //    this Func<T1, T2, Task> func,
    //    T1 arg1,
    //    T2 arg2,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <returns>Результат выполнения функции</returns>
    //public static TResult Retry<T1, T2, T3, TResult>(
    //    this Func<T1, T2, T3, TResult> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.Execute(() => func(arg1, arg2, arg3));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача с результатом выполнения функции</returns>
    //public static Task<TResult> Retry<T1, T2, T3, TResult>(
    //    this Func<T1, T2, T3, Task<TResult>> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2, arg3), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра действия</typeparam>
    ///// <param name="action">Действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="arg3">Третий параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    //public static void Retry<T1, T2, T3>(
    //    this Action<T1, T2, T3> action,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    pipeline.Execute(() => action(arg1, arg2, arg3));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра действия</typeparam>
    ///// <param name="func">Асинхронное действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="arg3">Третий параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача выполнения действия</returns>
    //public static Task Retry<T1, T2, T3>(
    //    this Func<T1, T2, T3, Task> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2, arg3), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="arg4">Четвертый параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <returns>Результат выполнения функции</returns>
    //public static TResult Retry<T1, T2, T3, T4, TResult>(
    //    this Func<T1, T2, T3, T4, TResult> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.Execute(() => func(arg1, arg2, arg3, arg4));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="arg4">Четвертый параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача с результатом выполнения функции</returns>
    //public static Task<TResult> Retry<T1, T2, T3, T4, TResult>(
    //    this Func<T1, T2, T3, T4, Task<TResult>> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2, arg3, arg4), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра действия</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра действия</typeparam>
    ///// <param name="action">Действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="arg3">Третий параметр действия</param>
    ///// <param name="arg4">Четвертый параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    //public static void Retry<T1, T2, T3, T4>(
    //    this Action<T1, T2, T3, T4> action,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    pipeline.Execute(() => action(arg1, arg2, arg3, arg4));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра действия</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра действия</typeparam>
    ///// <param name="func">Асинхронное действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="arg3">Третий параметр действия</param>
    ///// <param name="arg4">Четвертый параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача выполнения действия</returns>
    //public static Task Retry<T1, T2, T3, T4>(
    //    this Func<T1, T2, T3, T4, Task> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2, arg3, arg4), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра функции</typeparam>
    ///// <typeparam name="T5">Тип пятого параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="arg4">Четвертый параметр функции</param>
    ///// <param name="arg5">Пятый параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <returns>Результат выполнения функции</returns>
    //public static TResult Retry<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    T5 arg5,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.Execute(() => func(arg1, arg2, arg3, arg4, arg5));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра функции</typeparam>
    ///// <typeparam name="T5">Тип пятого параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="arg4">Четвертый параметр функции</param>
    ///// <param name="arg5">Пятый параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача с результатом выполнения функции</returns>
    //public static async Task<TResult> Retry<T1, T2, T3, T4, T5, TResult>(
    //    this Func<T1, T2, T3, T4, T5, Task<TResult>> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    T5 arg5,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return await pipeline.ExecuteAsync(async ct => await func(arg1, arg2, arg3, arg4, arg5), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра функции</typeparam>
    ///// <typeparam name="T5">Тип пятого параметра функции</typeparam>
    ///// <typeparam name="T6">Тип шестого параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="arg4">Четвертый параметр функции</param>
    ///// <param name="arg5">Пятый параметр функции</param>
    ///// <param name="arg6">Шестой параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <returns>Результат выполнения функции</returns>
    //public static TResult Retry<T1, T2, T3, T4, T5, T6, TResult>(
    //    this Func<T1, T2, T3, T4, T5, T6, TResult> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    T5 arg5,
    //    T6 arg6,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.Execute(() => func(arg1, arg2, arg3, arg4, arg5, arg6));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра функции</typeparam>
    ///// <typeparam name="T2">Тип второго параметра функции</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра функции</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра функции</typeparam>
    ///// <typeparam name="T5">Тип пятого параметра функции</typeparam>
    ///// <typeparam name="T6">Тип шестого параметра функции</typeparam>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="arg1">Первый параметр функции</param>
    ///// <param name="arg2">Второй параметр функции</param>
    ///// <param name="arg3">Третий параметр функции</param>
    ///// <param name="arg4">Четвертый параметр функции</param>
    ///// <param name="arg5">Пятый параметр функции</param>
    ///// <param name="arg6">Шестой параметр функции</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача с результатом выполнения функции</returns>
    //public static Task<TResult> Retry<T1, T2, T3, T4, T5, T6, TResult>(
    //    this Func<T1, T2, T3, T4, T5, T6, Task<TResult>> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    T5 arg5,
    //    T6 arg6,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2, arg3, arg4, arg5, arg6), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра действия</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра действия</typeparam>
    ///// <typeparam name="T5">Тип пятого параметра действия</typeparam>
    ///// <typeparam name="T6">Тип шестого параметра действия</typeparam>
    ///// <param name="action">Действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="arg3">Третий параметр действия</param>
    ///// <param name="arg4">Четвертый параметр действия</param>
    ///// <param name="arg5">Пятый параметр действия</param>
    ///// <param name="arg6">Шестой параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    //public static void Retry<T1, T2, T3, T4, T5, T6>(
    //    this Action<T1, T2, T3, T4, T5, T6> action,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    T5 arg5,
    //    T6 arg6,
    //    ResiliencePipeline? pipeline = null)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    pipeline.Execute(() => action(arg1, arg2, arg3, arg4, arg5, arg6));
    //}

    ///// <summary>
    ///// Выполняет функцию с повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="T1">Тип первого параметра действия</typeparam>
    ///// <typeparam name="T2">Тип второго параметра действия</typeparam>
    ///// <typeparam name="T3">Тип третьего параметра действия</typeparam>
    ///// <typeparam name="T4">Тип четвертого параметра действия</typeparam>
    ///// <typeparam name="T5">Тип пятого параметра действия</typeparam>
    ///// <typeparam name="T6">Тип шестого параметра действия</typeparam>
    ///// <param name="func">Асинхронное действие для выполнения</param>
    ///// <param name="arg1">Первый параметр действия</param>
    ///// <param name="arg2">Второй параметр действия</param>
    ///// <param name="arg3">Третий параметр действия</param>
    ///// <param name="arg4">Четвертый параметр действия</param>
    ///// <param name="arg5">Пятый параметр действия</param>
    ///// <param name="arg6">Шестой параметр действия</param>
    ///// <param name="pipeline">Политика повторов. Если null, используется политика по умолчанию</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача выполнения действия</returns>
    //public static Task Retry<T1, T2, T3, T4, T5, T6>(
    //    this Func<T1, T2, T3, T4, T5, T6, Task> func,
    //    T1 arg1,
    //    T2 arg2,
    //    T3 arg3,
    //    T4 arg4,
    //    T5 arg5,
    //    T6 arg6,
    //    ResiliencePipeline? pipeline = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    pipeline ??= RetryDefaults.DefaultPipeline;
    //    return pipeline.ExecuteAsync(async ct => await func(arg1, arg2, arg3, arg4, arg5, arg6), cancellationToken);
    //}

    ///// <summary>
    ///// Выполняет функцию с бесконечными повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <returns>Результат выполнения функции</returns>
    //public static TResult RetryInfinite<TResult>(this Func<TResult> func) => func.Retry(RetryDefaults.DefaultInfinityOptions);

    ///// <summary>
    ///// Выполняет функцию с бесконечными повторными попытками при возникновении исключений
    ///// </summary>
    ///// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
    ///// <param name="func">Функция для выполнения</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача с результатом выполнения функции</returns>
    //public static Task<TResult> RetryInfinite<TResult>(this Func<Task<TResult>> func, CancellationToken cancellationToken = default) =>
    //    func.Retry(RetryDefaults.InfinityPipeline, cancellationToken);

    ///// <summary>
    ///// Выполняет действие с бесконечными повторными попытками при возникновении исключений
    ///// </summary>
    ///// <param name="action">Действие для выполнения</param>
    //public static void RetryInfinite(this Action action) => action.Retry(RetryDefaults.InfinityPipeline);

    ///// <summary>
    ///// Выполняет действие с бесконечными повторными попытками при возникновении исключений
    ///// </summary>
    ///// <param name="func">Асинхронное действие для выполнения</param>
    ///// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    ///// <returns>Задача выполнения действия</returns>
    //public static Task RetryInfinite(this Func<Task> func, CancellationToken cancellationToken = default) =>
    //    func.Retry(RetryDefaults.InfinityPipeline, cancellationToken);
}
