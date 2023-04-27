using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall;

/// <summary>
/// Compares methods of calling dymanic looping functions (late binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class DynamicLoopExecution
{
    [Params(1_000, 10_000, 100_000)]
    public int Loops { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _addLoopStatic = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.StaticDelegate);
        _addLoopInstance = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
        _addLoopEmbedded = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.Embedded);
    }
    private AddLoop _addLoopStatic = null!, _addLoopInstance = null!, _addLoopEmbedded = null!;

    [Benchmark(Baseline = true)]
    public void StaticDelegate()
    {
        AddLoop addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.StaticDelegate);
        addLoop(a: 1, b: 1, Loops);
    }

    [Benchmark]
    public void InstanceDelegate()
    {
        AddLoop addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
        addLoop(a: 1, b: 1, Loops);
    }

    [Benchmark]
    public void Embedded()
    {
        AddLoop addLoop = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.Embedded);
        addLoop(a: 1, b: 1, Loops);
    }

    [Benchmark]
    public void PreCompiledStaticDelegate()
    {
        _addLoopStatic(a: 1, b: 1, Loops);
    }

    [Benchmark]
    public void PreCompiledInstanceDelegate()
    {
        _addLoopInstance(a: 1, b: 1, Loops);
    }

    [Benchmark]
    public void PreCompiledEmbedded()
    {
        _addLoopEmbedded(a: 1, b: 1, Loops);
    }
}
