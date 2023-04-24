using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Compares methods of compiling a dynamic loop calling an integer add implementation.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class DynamicLoopCompilation
{
    [Benchmark]
    public void StaticDelegate()
    {
        AddIntegers addIntegers = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.StaticDelegate);
    }

    [Benchmark]
    public void InstanceDelegate()
    {
        AddIntegers addIntegers = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
    }

    [Benchmark]
    public void Embedded()
    {
        AddIntegers addIntegers = AddLoopMethodFactory.Build(AddLoopMethodFactory.AddImplementation.Embedded);
    }
}
