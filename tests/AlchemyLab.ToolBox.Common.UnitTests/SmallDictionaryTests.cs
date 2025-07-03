namespace AlchemyLab.ToolBox.Common.UnitTests;

public class SmallDictionaryTests
{
    [Fact]
    public void Add_And_ContainsKey_Work_Correctly()
    {
        using SmallDictionary<int, string> dict = new();

        dict.Add(1, "One");
        dict.Add(2, "Two");

        Assert.True(dict.ContainsKey(1));
        Assert.True(dict.ContainsKey(2));
        Assert.False(dict.ContainsKey(3));
    }

    [Fact]
    public void GetValueRef_Returns_Correct_Reference()
    {
        using SmallDictionary<string, int> dict = new();
        dict.Add("test", 42);

        ref int value = ref dict.GetValueRef("test");
        value = 100; // Меняем значение напрямую

        Assert.Equal(100, dict.GetValueRef("test"));
    }

    [Fact]
    public void ToArray_Returns_Correct_Elements()
    {
        using SmallDictionary<int, bool> dict = new();
        dict.Add(1, true);
        dict.Add(2, false);

        KeyValuePair<int, bool>[] array = dict.ToArray();

        Assert.Equal(2, array.Length);
        Assert.Contains(new(1, true), array);
        Assert.Contains(new(2, false), array);
    }

    [Fact]
    public void Transition_To_Dictionary_When_Exceeded()
    {
        using SmallDictionary<int, int> dict = new();

        // Добавляем 17 элементов (16 - лимит массива)
        for (int i = 0; i < 17; i++)
        {
            dict.Add(i, i * 2);
        }

        // Проверяем, что перешли в режим словаря
        Assert.Equal(17, dict.Count);
        Assert.Equal(32, dict.GetValueRef(16)); // 16*2
    }

    [Fact]
    public void Dispose_Clears_Resources()
    {
        SmallDictionary<string, object> dict = new()
        {
            { "obj", new() }
        };
        dict.Dispose();

        Assert.Throws<ObjectDisposedException>(() => dict.Add("obj", new()));
    }

    [Fact]
    public void Zero_Capacity_Works()
    {
        using SmallDictionary<int, string> dict = new();
        dict.Add(1, "test");

        Assert.Equal("test", dict.GetValueRef(1));
    }

    [Fact]
    public void ToDictionary_With_Custom_Comparer()
    {
        using SmallDictionary<string, int> dict = new();
        dict.Add("a", 1);

        Dictionary<string, int> result = dict.ToDictionary(StringComparer.OrdinalIgnoreCase);

        Assert.True(result.ContainsKey("A"));
    }

    [Fact]
    public void Enumerator_Works_After_Transition()
    {
        using SmallDictionary<int, int> dict = new();
        // Инициируем переход
        for (int i = 0; i < 20; i++)
        {
            dict.Add(i, i);
        }

        int count = 0;
        foreach (KeyValuePair<int, int> _ in dict)
        {
            count++;
        }

        Assert.Equal(20, count);
    }
}
