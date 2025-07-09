namespace AlchemyLab.ToolBox.Benchmarks.Common;

[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByJob)]
[MeanColumn, MemoryDiagnoser]
public class SmallDictionaryBenchmarks
{
}
