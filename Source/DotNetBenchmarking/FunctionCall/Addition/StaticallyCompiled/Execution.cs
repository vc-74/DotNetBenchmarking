using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.Addition.StaticallyCompiled;

/// <summary>
/// Compares methods of calling non dymanic functions (early binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class Execution
{
    [Params(1_000)]
    public int Loops { get; set; }

    [Benchmark(Baseline = true)]
    public void NoFunction()
    {
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = _one + _one;
        }
    }

    // Using a non const field to avoid aggressive inlining
    private int _one = 1;

    [Benchmark]
    public void StaticMethod()
    {
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            AddStatic(1, 1);
        }
    }

    private static int AddStatic(int a, int b) => a + b;

    [Benchmark]
    public void InstanceMethod()
    {
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            AddInstance(1, 1);
        }
    }

    private int AddInstance(int a, int b) => a + b;

    [Benchmark]
    public void VirtualMethod()
    {
        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            AddInstanceVirtual(1, 1);
        }
    }

    protected virtual int AddInstanceVirtual(int a, int b) => a + b;

    [Benchmark]
    public void StaticLocalFunction()
    {
        static int LocalAddStatic(int a, int b) => a + b;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LocalAddStatic(1, 1);
        }
    }

    [Benchmark]
    public void InstanceLocalFunction()
    {
        int LocalAddInstance(int a, int b) => a + b;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LocalAddInstance(1, 1);
        }
    }

    [Benchmark]
    public void InstanceLocalFunctionCapture()
    {
        int LocalAdd(int a) => a + _one;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LocalAdd(1);
        }
    }

    [Benchmark]
    public void Lambda()
    {
        Func<int, int, int> LambdaAdd = (a, b) => a + b;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LambdaAdd(1, 1);
        }
    }

    [Benchmark]
    public void LambdaCapture()
    {
        Func<int, int> LambdaAdd = a => a + _one;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = LambdaAdd(1);
        }
    }

    [Benchmark]
    public void DelegateStaticMethod()
    {
        TakesTwoIntsReturnsInt StaticDelegateAdd = AddStatic;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = StaticDelegateAdd(1, 1);
        }
    }

    [Benchmark]
    public void DelegateInstanceMethod()
    {
        TakesOneIntReturnsInt InstanceDelegateAdd = AddInstance;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = InstanceDelegateAdd(1);
        }
    }

    private int AddInstance(int a) => a + _one;

    /// <summary>
    /// Prototype for functions taking one integer in parameter and returning an integer.
    /// </summary>
    public delegate int TakesOneIntReturnsInt(int a);

    [Benchmark]
    public void DelegateVirtualMethod()
    {
        TakesOneIntReturnsInt InstanceDelegateAdd = AddInstanceVirtual;

        int loops = Loops;

        for (int i = 0; i < loops; i++)
        {
            int sum = InstanceDelegateAdd(1);
        }
    }

    protected virtual int AddInstanceVirtual(int a) => a + _one;
}
