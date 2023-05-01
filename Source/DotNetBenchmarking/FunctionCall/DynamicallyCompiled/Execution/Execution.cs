using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.DynamicallyCompiled;

/// <summary>
/// Compares methods of calling dymanic functions (late binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class Execution
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        _addDynamicMethodStatic = AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Static);
        _addDynamicMethodInstance = AddMethodFactory.GetFromDynamicMethod(DelegateInstanceType.Instance);

        _addTypeMethodStatic = AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Static);
        _addTypeMethodInstance = AdderTypeFactory.GetAdd(buildModule: true, DelegateInstanceType.Instance);

        _addExpressionTreeBuilt = AddMethodFactory.GetFromExpressionTree(useLambda: false);
        _addExpressionTreeFromLambda = AddMethodFactory.GetFromExpressionTree(useLambda: true);

        _loopDynamicMethodStatic = AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.StaticDelegate);
        _loopDynamicMethodInstance = AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.InstanceDelegate);
        _loopDynamicMethodEmbedded = AddLoopMethodFactory.GetFromDynamicMethod(AddImplementation.Embedded);

        _loopExpressionTreeStatic = AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.StaticDelegate);
        _loopExpressionTreeInstance = AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.InstanceDelegate);
        _loopExpressionTreeEmbedded = AddLoopMethodFactory.GetFromExpressionTree(AddImplementation.Embedded);
    }
    private TakesTwoIntsReturnsInt _addDynamicMethodStatic = null!, _addDynamicMethodInstance = null!,
        _addTypeMethodStatic = null!, _addTypeMethodInstance = null!,
        _addExpressionTreeBuilt = null!, _addExpressionTreeFromLambda = null!;

    private TakesAnInt _loopDynamicMethodStatic = null!, _loopDynamicMethodInstance = null!, _loopDynamicMethodEmbedded = null!,
        _loopExpressionTreeStatic = null!, _loopExpressionTreeInstance = null!, _loopExpressionTreeEmbedded = null!;

    [Params(1_000, 10_000, 100_000)]
    public int Loops { get; set; }

    /// <summary>
    /// Implementation not using functions.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void NoFunction()
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int _ = a + i;
        }
    }

    /// <summary>
    /// Implementation as a static method call.
    /// </summary>
    [Benchmark]
    public void StaticFunction() => StaticFunction(Loops);

    private static void StaticFunction(int loops)
    {
        int a = 1;

        for (int i = 0; i < loops; i++)
        {
            int _ = a + i;
        }
    }

    /// <summary>
    /// Implementation as an external loop invoking a static delegate built from a DynamicMethod.
    /// </summary>
    [Benchmark]
    public void DynamicMethodStaticDelegate() => Execute(_addDynamicMethodStatic);

    private void Execute(TakesTwoIntsReturnsInt add)
    {
        int a = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            add(a, i);
        }
    }

    /// <summary>
    /// Implementation as an external loop invoking an instance delegate built from a DynamicMethod.
    /// </summary>
    [Benchmark]
    public void DynamicMethodInstanceDelegate() => Execute(_addDynamicMethodInstance);

    /// <summary>
    /// Implementation as an external loop invoking a static delegate built from a dynamically built type.
    /// </summary>
    [Benchmark]
    public void DynamicTypeStaticDelegate() => Execute(_addTypeMethodStatic);

    /// <summary>
    /// Implementation as an external loop invoking an instance delegate built from a dynamically built type.
    /// </summary>
    [Benchmark]
    public void DynamicTypeInstanceDelegate() => Execute(_addTypeMethodInstance);

    /// <summary>
    /// Implementation as an external loop invoking a delegate manually built from an expression tree.
    /// </summary>
    [Benchmark]
    public void ExpressionTreeBuilt() => Execute(_addExpressionTreeBuilt);

    /// <summary>
    /// Implementation as an external loop invoking a delegate built from an expression tree generated from a lambda expression.
    /// </summary>
    [Benchmark]
    public void ExpressionTreeFromLambda() => Execute(_addExpressionTreeFromLambda);

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from a DynamicMethod invoking a static add delegate.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodStatic() => _loopDynamicMethodStatic(Loops);

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from a DynamicMethod invoking an instance add delegate.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodInstance() => _loopDynamicMethodInstance(Loops);

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from a DynamicMethod invoking embedding the addition.
    /// </summary>
    [Benchmark]
    public void LoopDynamicMethodEmbedded() => _loopDynamicMethodEmbedded(Loops);

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from an expression tree invoking a static add delegate.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeStatic() => _loopExpressionTreeStatic(Loops);

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from an expression tree invoking an instance add delegate.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeInstance() => _loopExpressionTreeInstance(Loops);

    /// <summary>
    /// Implementation as an invocation of a loop delegate built from an expression tree invoking embedding the addition.
    /// </summary>
    [Benchmark]
    public void LoopExpressionTreeEmbedded() => _loopExpressionTreeEmbedded(Loops);
}
