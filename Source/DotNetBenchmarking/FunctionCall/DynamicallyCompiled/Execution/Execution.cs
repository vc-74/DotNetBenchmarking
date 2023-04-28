using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.Addition.DynamicallyCompiled;

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
        _addDynamicMethodStatic = AddMethodFactory.GetFromDynamicMethod(DelegateType.Static);
        _addDynamicMethodInstance = AddMethodFactory.GetFromDynamicMethod(DelegateType.Instance);

        _addTypeMethodStatic = AdderTypeFactory.GetAdd(buildModule: true, DelegateType.Static);
        _addTypeMethodInstance = AdderTypeFactory.GetAdd(buildModule: true, DelegateType.Instance);

        _addExpressionTreeBuilt = AddMethodFactory.GetFromExpressionTree(useLambda: false);
        _addExpressionTreeFromLambda = AddMethodFactory.GetFromExpressionTree(useLambda: true);

        _loopDynamicMethodStatic = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.StaticDelegate);
        _loopDynamicMethodInstance = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.InstanceDelegate);
        _loopDynamicMethodEmbedded = AddLoopMethodFactory.GetFromDynamicMethod(AddLoopMethodFactory.AddImplementation.Embedded);
    }
    private TakesTwoIntsReturnsInt _addDynamicMethodStatic = null!, _addDynamicMethodInstance = null!,
        _addTypeMethodStatic = null!, _addTypeMethodInstance = null!,
        _addExpressionTreeBuilt = null!, _addExpressionTreeFromLambda = null!;

    private AddLoop _loopDynamicMethodInstance = null!, _loopDynamicMethodStatic = null!, _loopDynamicMethodEmbedded = null!;

    [Params(1_000, 10_000, 100_000)]
    public int Loops { get; set; }

    [Benchmark(Baseline = true)]
    public void NoFunction()
    {
        int a = 1, b = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = a + b;
        }
    }

    [Benchmark]
    public void StaticFunction() => StaticFunction(Loops);

    private static void StaticFunction(int loops)
    {
        int a = 1, b = 1;

        for (int i = 0; i < loops; i++)
        {
            int sum = a + b;
        }
    }

    [Benchmark]
    public void DynamicMethodStaticDelegate() => Execute(_addDynamicMethodStatic);

    private void Execute(TakesTwoIntsReturnsInt add)
    {
        int a = 1, b = 1;
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            add(a, b);
        }
    }

    [Benchmark]
    public void DynamicMethodInstanceDelegate() => Execute(_addDynamicMethodInstance);

    [Benchmark]
    public void DynamicTypeStaticDelegate() => Execute(_addTypeMethodStatic);

    [Benchmark]
    public void DynamicTypeInstanceDelegate() => Execute(_addTypeMethodInstance);

    [Benchmark]
    public void ExpressionTreeBuilt() => Execute(_addExpressionTreeBuilt);

    [Benchmark]
    public void ExpressionTreeFromLambda() => Execute(_addExpressionTreeFromLambda);

    [Benchmark]
    public void LoopDynamicMethodStatic() => _loopDynamicMethodStatic(Loops);

    [Benchmark]
    public void LoopDynamicMethodInstance() => _loopDynamicMethodInstance(Loops);

    [Benchmark]
    public void LoopDynamicMethodEmbedded() => _loopDynamicMethodEmbedded(Loops);
}
