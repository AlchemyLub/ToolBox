namespace AlchemyLab.ToolBox.Benchmarks.Common;

[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByJob)]
[MeanColumn, MemoryDiagnoser]
public class SmallDictionaryBenchmarks
{
    [Params(5, 10, 20, 50)]
    public int Count { get; set; }

    private int[] accessKeys = [];
    private int[] writeKeys = [];

    [Benchmark(Baseline = true)]
    public int Dictionary()
    {
        Dictionary<int, int> dic = new();
        foreach (int key in writeKeys)
        {
            dic.Add(key, key + key);
        }

        int result = 0;
        foreach (int key in accessKeys) // access test
        {
            result += dic[key];
        }

        foreach (KeyValuePair<int, int> pair in dic) // foreach test
        {
            result += pair.Value;
        }

        return result;
    }

    [Benchmark(Description = "SmallDictionary")]
    public int SmallDictionary()
    {
        using SmallDictionary<int, int> localCache = new();
        foreach (int key in writeKeys)
        {
            localCache.Add(key, key + key);
        }

        int result = 0;
        foreach (int key in accessKeys)
        {
            result += localCache[key];
        }

        foreach (KeyValuePair<int, int> pair in localCache)
        {
            result += pair.Value;
        }

        return result;
    }

    [GlobalSetup]
    public void Setup()
    {
        accessKeys = Enumerable.Range(0, Count).ToArray();
        writeKeys = new int[accessKeys.Length];

        Array.Copy(accessKeys, writeKeys, accessKeys.Length);

        Random.Shared.Shuffle(accessKeys);
        Random.Shared.Shuffle(writeKeys);
    }
}
