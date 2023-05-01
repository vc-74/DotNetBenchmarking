using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled.Loop;

/// <summary>
/// Compares methods of compiling a function implementing a loop adding two integers.
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class Compilation
{
    /// <summary>
    /// Compiles a delegate using a dynamic method invoking a static delegate.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodStatic() => AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.StaticDelegate);

    /// <summary>
    /// Compiles a delegate using a dynamic method invoking an instance delegate.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodInstance() => AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.InstanceDelegate);

    /// <summary>
    /// Compiles a delegate using a dynamic method embedding the addition.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodEmbedded() => AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.Embedded);

    /// <summary>
    /// Compiles a delegate using an expression tree invoking a static delegate.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeStatic() => AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.StaticDelegate);

    /// <summary>
    /// Compiles a delegate using an expression tree invoking an instance delegate.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeInstance() => AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.InstanceDelegate);

    /// <summary>
    /// Compiles a delegate using an expression tree embedding the addition.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeEmbedded() => AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.Embedded);
}
