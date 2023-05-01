using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled.Add;

/// <summary>
/// Compares methods of compiling a function adding two integers.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD, Column.IterationCount, Column.WarmupCount)]
[MemoryDiagnoser]
public class Compilation
{
    /// <summary>
    /// Compiles a static delegate using a dynamic method.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void DynamicMethodStaticDelegate() => AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Static);

    /// <summary>
    /// Compiles an instance delegate using a dynamic method.
    /// </summary>
    [Benchmark]
    public void DynamicMethodInstanceDelegate() => AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Instance);

    /// <summary>
    /// Compiles a static delegate using a dynamically created type in a new module.
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeNewModuleStaticDelegate() => AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Static);

    /// <summary>
    /// Compiles an instance delegate using a dynamically created type in a new module.
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeNewModuleInstanceDelegate() => AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Instance);

    /// <summary>
    /// Compiles a static delegate using a dynamically created type in an existing module.
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeExistingModuleStaticDelegate() => AdderTypeFactory.GetAdd(buildModule: false, DelegateInstanceType.Static);

    /// <summary>
    /// Compiles an instance delegate using a dynamically created type in an existing module.
    /// </summary>
    /// <remarks>Using low warmup/iterations to avoid creating too many types which slows down the process very quickly.</remarks>
    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(3)]
    public void DynamicTypeExistingModuleInstanceDelegate() => AdderTypeFactory.GetAdd(buildModule: false, DelegateInstanceType.Instance);

    /// <summary>
    /// Compiles an instance delegate using a expression tree manually built.
    /// </summary>
    [Benchmark]
    public void ExpressionTreeBuilt() => AddMethodFactory.GetFromExpressionTree(useLambda: false);

    /// <summary>
    /// Compiles an instance delegate using a expression tree built from a lambda expression.
    /// </summary>
    [Benchmark]
    public void ExpressionTreeFromLambda() => AddMethodFactory.GetFromExpressionTree(useLambda: true);
}
