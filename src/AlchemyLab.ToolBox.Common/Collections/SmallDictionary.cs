namespace AlchemyLab.ToolBox.Common.Collections;

public struct SmallDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IDisposable where TKey : IEquatable<TKey>
{
    private const int RentArrayLength = 16;

    private bool isDisposed;
    private bool isArrayFromPool;

    [ThreadStatic]
    private static Dictionary<TKey, TValue>? cachedDictionary;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Pair[] RentArray() => ArrayPool<Pair>.Shared.Rent(RentArrayLength);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Dictionary<TKey, TValue> RentDictionary()
    {
        Dictionary<TKey, TValue> result = cachedDictionary ?? new(64);
        cachedDictionary = null;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReturnArray(Pair[] array, int length)
    {
        array.AsSpan(0, length).Clear();
        ArrayPool<Pair>.Shared.Return(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReturnDictionary(Dictionary<TKey, TValue> dictionary)
    {
        dictionary.Clear();
        cachedDictionary = dictionary;
    }

    public readonly int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ThrowIfDisposed();
            return count;
        }
    }

    private Pair[] array;
    private int count;
    private Dictionary<TKey, TValue>? dictionary;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SmallDictionary()
    {
        array = RentArray();
        isArrayFromPool = true;
        count = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SmallDictionary(int capacity)
    {
        if (capacity < RentArrayLength)
        {
            array = RentArray();
            isArrayFromPool = true;
        }
        else
        {
            array = [];
            isArrayFromPool = false;
            dictionary = RentDictionary();
        }

        count = 0;
    }

    public void Add(TKey key, TValue value, bool replaceIfExists = false)
    {
        ThrowIfDisposed();

        if (dictionary is not null)
        {
            if (replaceIfExists)
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
        else if (count < array.Length)
        {
            // проверка дубликата
            ref TValue exists = ref GetValueRef(key);

            if (Unsafe.IsNullRef(in exists))
            {
                array[count] = new Pair(key, value);
                count++;
            }
            else
            {
                switch (replaceIfExists)
                {
                    case true:
                        exists = value;
                        break;
                    default:
                        Throw("An item with the same key has already been added");
                        break;
                }
            }
        }
        else
        {
            dictionary = RentDictionary();

            foreach (ref Pair pair in array.AsSpan(0, count))
            {
                dictionary[pair.Key] = pair.Value;
            }

            if (isArrayFromPool)
            {
                ReturnArray(array, count);
            }

            array = [];
            isArrayFromPool = false;

            if (replaceIfExists)
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
                count++;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool ContainsKey(TKey key)
    {
        ThrowIfDisposed();

        ref readonly TValue value = ref GetValueRef(key);

        return !Unsafe.IsNullRef(in value);
    }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        if (isArrayFromPool && array.Length > 0)
        {
            ReturnArray(array, count);
        }

        if (dictionary is not null)
        {
            ReturnDictionary(dictionary);
        }

        isDisposed = true;
        array = [];
        dictionary = null;
        count = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Enumerator GetEnumerator()
    {
        ThrowIfDisposed();

        return new(array, dictionary, count);
    }

    public readonly ref TValue GetValueRef(TKey key)
    {
        ThrowIfDisposed();

        if (count is 0)
        {
            return ref Unsafe.NullRef<TValue>();
        }

        if (dictionary is not null)
        {
            return ref CollectionsMarshal.GetValueRefOrNullRef(dictionary, key);
        }

        foreach (ref Pair pair in array.AsSpan(0, count))
        {
            if (pair.Key.Equals(key))
            {
                return ref pair.Value;
            }
        }

        return ref Unsafe.NullRef<TValue>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        ThrowIfDisposed();

        ref readonly TValue exists = ref GetValueRef(key);

        if (Unsafe.IsNullRef(in exists))
        {
            value = default;
            return false;
        }

        value = exists;
        return true;
    }

    public readonly Dictionary<TKey, TValue> ToDictionary(IEqualityComparer<TKey>? comparer = null)
    {
        ThrowIfDisposed();

        if (dictionary is not null)
        {
            return new(dictionary, comparer);
        }

        Dictionary<TKey, TValue> result = new(count, comparer);

        foreach (ref Pair pair in array.AsSpan(0, count))
        {
            result[pair.Key] = pair.Value;
        }

        return result;
    }

    public readonly List<KeyValuePair<TKey, TValue>> ToList()
    {
        ThrowIfDisposed();

        List<KeyValuePair<TKey, TValue>> list = new(count);

        if (dictionary is not null)
        {
            list.EnsureCapacity(dictionary.Count);

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                list.Add(pair);
            }
        }
        else
        {
            list.EnsureCapacity(count);
            foreach (ref Pair pair in array.AsSpan(0, count))
            {
                list.Add(new(pair.Key, pair.Value));
            }
        }

        return list;
    }

    public readonly KeyValuePair<TKey, TValue>[] ToArray()
    {
        ThrowIfDisposed();

        if (dictionary is not null)
        {
            KeyValuePair<TKey, TValue>[] arr = new KeyValuePair<TKey, TValue>[dictionary.Count];

            int i = 0;

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                arr[i++] = pair;
            }

            return arr;
        }
        else
        {
            KeyValuePair<TKey, TValue>[] arr = new KeyValuePair<TKey, TValue>[count];

            Span<Pair> span = array.AsSpan(0, count);

            for (int i = 0; i < count; i++)
            {
                ref Pair pair = ref span[i];

                arr[i] = new(pair.Key, pair.Value);
            }

            return arr;
        }
    }

    public readonly ref TValue this[TKey key]
    {
        get
        {
            ThrowIfDisposed();

            ref TValue value = ref GetValueRef(key);

            if (Unsafe.IsNullRef(in value))
            {
                Throw("Key not found");
            }

            return ref value;
        }
    }

    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public readonly KeyValuePair<TKey, TValue> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!isArray)
                {
                    return dictionaryEnumerator.Current;
                }

                Pair pair = arrayEnumerator.Current;
                return new(pair.Key, pair.Value);
            }
        }

        private readonly bool isArray;
        private ArraySegment<Pair>.Enumerator arrayEnumerator;
        private Dictionary<TKey, TValue>.Enumerator dictionaryEnumerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(Pair[] array, Dictionary<TKey, TValue>? dictionary, int count)
        {
            if (dictionary is null)
            {
                isArray = true;
                arrayEnumerator = new ArraySegment<Pair>(array, 0, count).GetEnumerator();
                dictionaryEnumerator = default;
            }
            else
            {
                isArray = false;
                arrayEnumerator = default;
                dictionaryEnumerator = dictionary.GetEnumerator();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => isArray
            ? arrayEnumerator.MoveNext()
            : dictionaryEnumerator.MoveNext();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (isArray)
            {
                arrayEnumerator.Dispose();
            }
            else
            {
                dictionaryEnumerator.Dispose();
            }
        }

        readonly void IEnumerator.Reset()
        {
        }

        readonly object IEnumerator.Current => Current;
    }

    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal struct Pair(TKey key, TValue value)
    {
        public readonly TKey Key = key;
        public TValue Value = value;
    }

    [DoesNotReturn]
    private static void Throw(string message) => throw new(message);

    private readonly void ThrowIfDisposed()
    {
        if (isDisposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    readonly IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
