using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.Addition.DynamicallyCompiled;

/// <summary>
/// Compares methods of compiling dymanic functions.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD, Column.IterationCount, Column.WarmupCount)]
[MemoryDiagnoser]
public class Compilation
{
    [Benchmark]
    public void DynamicMethodILStaticDelegate() => AddMethodFactory.BuildDynamicMethodIL(DelegateType.Static);

    [Benchmark(Baseline = true)]
    public void DynamicMethodILInstanceDelegate() => AddMethodFactory.BuildDynamicMethodIL(DelegateType.Instance);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(10)]
    public void DynamicTypeNewModule() => AdderTypeFactory.BuildTypeIL(buildModule: true, DelegateType.Instance);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(10)]
    public void DynamicTypeExistingModule() => AdderTypeFactory.BuildTypeIL(buildModule: false, DelegateType.Instance);

    [Benchmark]
    public void ExpressionTree() => AddMethodFactory.BuildDynamicMethodExpressionTree();
}
