namespace AlchemyLab.ToolBox.Common.Collections.Extensions;

/// <summary>
/// Методы расширения для <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Пытается привести коллекцию к массиву без аллокаций
    /// </summary>
    [Pure]
    public static T[] AsArray<T>(this IEnumerable<T> source) =>
        source switch
        {
            null => throw new ArgumentNullException(nameof(source)),
            T[] array => array,
            IReadOnlyCollection<T> roc => roc.Count is 0 ? [] : source.ToArray(),
            _ => source.ToArray()
        };

    /// <summary>
    /// Пытается привести коллекцию к списку без аллокаций
    /// </summary>
    [Pure]
    public static List<T> AsList<T>(this IEnumerable<T> source) =>
        source switch
        {
            null => throw new ArgumentNullException(nameof(source)),
            List<T> list => list,
            IReadOnlyCollection<T> roc => roc.Count is 0 ? [] : source.ToList(),
            _ => source.ToList()
        };

    /// <summary>
    /// Безопасно получает <see cref="ReadOnlySpan{T}"/> из коллекции.
    /// Если <paramref name="source"/> — массив или список, возвращает его <see cref="ReadOnlySpan{T}"/> без копирования;
    /// иначе — копирует в новый массив и возвращает <see cref="ReadOnlySpan{T}"/> по нему
    /// </summary>
    [Pure]
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source switch
        {
            T[] array => array,
            List<T> list => CollectionsMarshal.AsSpan(list),
            ICollection<T> coll => CopyToSpan(coll),
            _ => source.ToArray()
        };

        static ReadOnlySpan<T> CopyToSpan(ICollection<T> collection)
        {
            if (collection.Count is 0)
            {
                return [];
            }

            T[] array = new T[collection.Count];
            collection.CopyTo(array, 0);
            return array;
        }
    }

    /// <summary>
    /// Проверяет, есть ли в последовательности повторяющиеся элементы
    /// </summary>
    [Pure]
    public static bool HasDuplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        comparer ??= EqualityComparer<T>.Default;

        HashSet<T> seen = new(comparer);

        foreach (T item in source)
        {
            if (!seen.Add(item))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Возвращает все элементы, которые встречаются более одного раза (повторы).
    /// Гарантируется порядок первого вхождения каждого дубликата
    /// </summary>
    [Pure]
    public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        comparer ??= EqualityComparer<T>.Default;

        HashSet<T> seen = source is ICollection<T> collection
            ? new(collection.Count, comparer)
            : new(comparer);

        foreach (T item in source)
        {
            if (seen.Add(item))
            {
                continue;
            }

            yield return item;
            seen.Remove(item);
        }
    }

    /// <summary>
    /// Выполняет заданное действие для каждого элемента последовательности и возвращает элементы без изменений.
    /// Полезен для отладки, логирования или побочных эффектов внутри LINQ-цепочек.
    /// </summary>
    [Pure]
    public static IEnumerable<T> Inspect<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        return Iterator();

        IEnumerable<T> Iterator()
        {
            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }
    }

    /// <summary>
    /// Разбивает последовательность на группы, начиная новую группу при выполнении условия.
    /// Условие применяется ко всем элементам, включая первый в каждой группе.
    /// </summary>
    [Pure]
    public static IEnumerable<IEnumerable<T>> SplitWhen<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return Iterator();

        IEnumerable<IEnumerable<T>> Iterator()
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                yield break;
            }

            while (true)
            {
                T first = enumerator.Current;

                IEnumerable<T> group = GroupIterator(enumerator, first, predicate);
                yield return group;

                if (!enumerator.MoveNext())
                {
                    yield break;
                }
            }
        }

        IEnumerable<T> GroupIterator(
            IEnumerator<T> enumerator,
            T first,
            Func<T, bool> splitCondition)
        {
            yield return first;

            while (enumerator.MoveNext())
            {
                if (splitCondition(enumerator.Current))
                {
                    yield break;
                }

                yield return enumerator.Current;
            }
        }
    }

    /// <summary>
    /// Разбивает последовательность на группы подряд идущих элементов с одинаковым ключом
    /// </summary>
    /// <remarks>
    /// Пример использования (разбиение последовательности по городам):
    /// <code>
    /// var data = new[]
    /// {
    ///     new { Name = "Ivan", City = "Moscow" },
    ///     new { Name = "Dmitry", City = "Moscow" },
    ///     new { Name = "Petr", City = "SPb" },
    ///     new { Name = "Anna", City = "Moscow" }
    /// };
    /// 
    /// var chunks = data.ChunkBy(x => x.City);
    /// 
    /// Результат:
    /// [{ Name = Ivan, City = Moscow }, { Name = "Dmitry", City = "Moscow" }],
    /// [{ Name = Petr, City = SPb }],
    /// [{ Name = Anna, City = Moscow }]
    /// </code>
    /// </remarks>
    [Pure]
    public static IEnumerable<IEnumerable<TSource>> ChunkBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);

        comparer ??= EqualityComparer<TKey>.Default;

        return ChunkIterator();

        IEnumerable<IEnumerable<TSource>> ChunkIterator()
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                yield break;
            }

            TKey currentKey = keySelector(enumerator.Current);
            IEnumerable<TSource> currentChunk = ChunkItems(enumerator, currentKey);

            while (enumerator.MoveNext())
            {
                TKey key = keySelector(enumerator.Current);

                if (comparer.Equals(currentKey, key))
                {
                }
                else
                {
                    yield return currentChunk;
                    currentKey = key;
                    currentChunk = ChunkItems(enumerator, currentKey);
                }
            }

            yield return currentChunk;
        }

        IEnumerable<TSource> ChunkItems(IEnumerator<TSource> enumerator, TKey key)
        {
            do
            {
                yield return enumerator.Current;
            }
            while (enumerator.MoveNext() && comparer.Equals(key, keySelector(enumerator.Current)));
        }
    }

    /// <summary>
    /// Разбивает последовательность на чанки с использованием кастомного предиката
    /// </summary>
    /// <remarks>
    /// Пример использования (разбиение при изменении температуры >5°):
    /// <code>
    /// var temps = new[]
    /// {
    ///     new { Time = "10:00", Value = 20.0 },
    ///     new { Time = "10:05", Value = 20.5 },
    ///     new { Time = "10:10", Value = 15.0 } // Резкое изменение
    /// };
    ///
    /// var chunks = temps.ChunkBy((prev, curr) => Math.Abs(curr.Value - prev.Value) > 5.0);
    ///
    /// Результат:
    /// [{ Time = 10:00, Value = 20.0 }, { Time = 10:05, Value = 20.5 }]
    /// [{ Time = 10:10, Value = 15.0 }]
    /// </code>
    /// </remarks>
    [Pure]
    public static IEnumerable<IEnumerable<TSource>> ChunkBy<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, TSource, bool> chunkChangePredicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(chunkChangePredicate);

        return ChunkIterator();

        IEnumerable<IEnumerable<TSource>> ChunkIterator()
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                yield break;
            }

            IEnumerable<TSource> currentChunk = CurrentChunk(enumerator, chunkChangePredicate);

            while (enumerator.MoveNext())
            {
                yield return currentChunk;
                currentChunk = CurrentChunk(enumerator, chunkChangePredicate);
            }

            yield return currentChunk;
        }

        IEnumerable<TSource> CurrentChunk(IEnumerator<TSource> enumerator, Func<TSource, TSource, bool> predicate)
        {
            TSource prevItem = enumerator.Current;
            yield return prevItem;

            while (enumerator.MoveNext())
            {
                TSource currentItem = enumerator.Current;
                if (predicate(prevItem, currentItem))
                {
                    yield break;
                }

                yield return currentItem;
                prevItem = currentItem;
            }
        }
    }

    /// <summary>
    /// Объединяет два словаря с обработкой конфликтов через резолвер
    /// </summary>
    /// <param name="first">Исходный словарь</param>
    /// <param name="second">Словарь для объединения</param>
    /// <param name="resolver">Функция разрешения конфликтов</param>
    /// <param name="comparer">Компаратор для ключей (если <see langword="null"/>, используется стандартный компаратор)</param>
    /// <remarks>
    /// Пример использования:
    /// <code>
    /// var mergedDictionary = dictionary1.Merge(
    ///     dictionary2,
    ///     (oldVal, newVal) => oldVal + newVal,
    ///     StringComparer.OrdinalIgnoreCase);
    /// </code>
    /// </remarks>
    [Pure]
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        this IDictionary<TKey, TValue> first,
        IDictionary<TKey, TValue> second,
        Func<TValue, TValue, TValue> resolver,
        IEqualityComparer<TKey>? comparer = null)
        where TKey : notnull
    {
        comparer ??= EqualityComparer<TKey>.Default;
        Dictionary<TKey, TValue> result = new(first, comparer);

        foreach (KeyValuePair<TKey, TValue> kvp in second)
        {
            ref TValue? valRef = ref CollectionsMarshal.GetValueRefOrAddDefault(result, kvp.Key, out bool exists);

            valRef = exists && valRef is not null ? resolver(valRef, kvp.Value) : kvp.Value;
        }

        return result;
    }

    /// <summary>
    /// Получает значение по ключу или добавляет новое через фабрику
    /// </summary>
    /// <remarks>
    /// Пример использования:
    /// <code>
    /// users.GetOrAdd(
    ///     "user_123",
    ///     key =>
    ///     {
    ///         var user = userService.GetOrCreate(key, newUser);
    ///         return user;
    ///     });
    /// </code>
    /// </remarks>
    [Pure]
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue> valueFactory) where TKey : notnull
    {
        ref TValue? valRef = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out bool exists);

        if (!exists || valRef is null)
        {
            valRef = valueFactory();
        }

        return valRef;
    }

    /// <summary>
    /// Получает значение по ключу или добавляет новое через фабрику
    /// </summary>
    /// <remarks>
    /// Пример использования:
    /// <code>
    /// users.GetOrAdd(
    ///     "user_123",
    ///     async (key, ct) =>
    ///     {
    ///         var user = await userService.GetOrCreate(key, newUser, ct);
    ///         return user;
    ///     },
    ///     cancellationToken);
    /// </code>
    /// </remarks>
    [Pure]
    public static async Task<TValue> GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, CancellationToken, Task<TValue>> asyncValueFactory,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (dictionary.TryGetValue(key, out TValue? value))
        {
            return value;
        }

        cancellationToken.ThrowIfCancellationRequested();
        value = await asyncValueFactory(key, cancellationToken).ConfigureAwait(false);

        dictionary[key] = value;
        return value;
    }

    /// <summary>
    /// Атомарно изменяет значение в словаре
    /// </summary>
    /// <param name="dict">Исходный словарь</param>
    /// <param name="key">Ключ по которому находится изменяемое значение</param>
    /// <param name="update">Функция обновления</param>
    /// <remarks>
    /// Пример использования:
    /// <code>
    /// counters.Compute(
    ///     "user_123",
    ///     (key, current) =>
    ///     {
    ///         var user = userService.Get(key);
    ///         return user.Id + 1;
    ///     });
    /// </code>
    /// </remarks>
    public static void Compute<TKey, TValue>(
        this Dictionary<TKey, TValue> dict,
        TKey key,
        Func<TKey, TValue?, TValue> update)
        where TKey : notnull
    {
        ref TValue valRef = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);

        if (Unsafe.IsNullRef(ref valRef))
        {
            dict[key] = update(key, default);
        }
        else
        {
            valRef = update(key, valRef);
        }
    }

    /// <summary>
    /// Атомарно изменяет значение в словаре
    /// </summary>
    /// <param name="dict">Исходный словарь</param>
    /// <param name="key">Ключ по которому находится изменяемое значение</param>
    /// <param name="asyncUpdate">Асинхронная функция обновления</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <remarks>
    /// Пример использования:
    /// <code>
    /// await counters.Compute(
    ///     "user_123",
    ///     async (key, current, ct) =>
    ///     {
    ///         var user = await userService.Get(key, ct);
    ///         return user.Id + 1;
    ///     },
    ///     cancellationToken);
    /// </code>
    /// </remarks>
    public static async Task Compute<TKey, TValue>(
        this Dictionary<TKey, TValue> dict,
        TKey key,
        Func<TKey, TValue?, CancellationToken, Task<TValue>> asyncUpdate,
        CancellationToken cancellationToken = default) where TKey : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (dict.TryGetValue(key, out TValue? value))
        {
            TValue newValue = await asyncUpdate(key, value, cancellationToken).ConfigureAwait(false);
            dict[key] = newValue;
        }
    }

    /// <summary>
    /// Находит различия между двумя последовательностями и возвращает элементы,
    /// которые присутствуют только в первой или только во второй.
    /// </summary>
    /// <typeparam name="T">Тип элементов последовательностей.</typeparam>
    /// <param name="first">Первая последовательность (например, старые значения).</param>
    /// <param name="second">Вторая последовательность (например, новые значения).</param>
    /// <param name="comparer">
    /// Компаратор для сравнения элементов.
    /// Если не указан, используется <see cref="EqualityComparer{T}.Default"/>.
    /// </param>
    /// <returns>
    /// Кортеж из двух списков:
    /// <list type="bullet">
    /// <item><description><c>OnlyInFirst</c> — элементы, присутствующие только в <paramref name="first"/>.</description></item>
    /// <item><description><c>OnlyInSecond</c> — элементы, присутствующие только в <paramref name="second"/>.</description></item>
    /// </list>
    /// </returns>
    public static (List<T> OnlyInFirst, List<T> OnlyInSecond) Diff<T>(
        this IEnumerable<T> first,
        IEnumerable<T> second,
        IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        HashSet<T> setFirst = new(first, comparer);
        HashSet<T> setSecond = new(second, comparer);

        List<T> onlyInFirst = setFirst.Except(setSecond, comparer).AsList();
        List<T> onlyInSecond = setSecond.Except(setFirst, comparer).AsList();

        return (onlyInFirst, onlyInSecond);
    }

    /// <summary>
    /// Возвращает case-insensitive обертку над словарем
    /// </summary>
    [Pure]
    public static Dictionary<string, TValue> AsCaseInsensitive<TValue>(this Dictionary<string, TValue> source) =>
        new(source, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Проверяет, является ли коллекция <see langword="null"/> или пуста
    /// </summary>
    [Pure]
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => source is null || !source.Any();
}
