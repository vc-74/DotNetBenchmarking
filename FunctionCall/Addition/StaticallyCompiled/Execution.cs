using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarking.FunctionCall.Addition.StaticallyCompiled;

/// <summary>
/// Compares methods of calling non dymanic functions (early binding).
/// </summary>
[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[HideColumns(Column.Job, Column.Error, Column.RatioSD)]
[MemoryDiagnoser]
public class Execution
{
    [Params(10_000)]
    public int Loops { get; set; }

    [Benchmark(Baseline = true)]
    public void NoFunction()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = a + b;
        }
    }

    [Benchmark]
    public void StaticMethod()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = AddStatic(a, b);
        }
    }

    private static int AddStatic(int a, int b) => a + b;

    [Benchmark]
    public void InstanceMethod()
    {
        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = AddInstance(a, b);
        }
    }

    private int AddInstance(int a, int b) => a + b;

    [Benchmark]
    public void StaticLocalFunction()
    {
        static int LocalAdd(int a, int b) => a + b;

        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = LocalAdd(a, b);
        }
    }

    [Benchmark]
    public void InstanceLocalFunction()
    {
        int LocalAdd(int a, int b) => a + b;

        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = LocalAdd(a, b);
        }
    }

    [Benchmark]
    public void InstanceLocalFunctionCapture()
    {
        int LocalAdd(int a) => a + _b;

        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            int c = LocalAdd(a);
        }
    }

    // Not a const to avoid compiler optimizations
    private int _b = 2;

    [Benchmark]
    public void Lambda()
    {
        Func<int, int, int> lambda = (a, b) => a + b;

        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            const int b = 2;
            int c = lambda(a, b);
        }
    }

    [Benchmark]
    public void LambdaCapture()
    {
        Func<int, int> lambda = a => a + _b;

        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            int c = lambda(a);
        }
    }

    [Benchmark]
    public void DelegateStaticMethod()
    {
        const int b = 2;
        TakesTwoIntsReturnsInt myDel = AddStatic;

        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            int c = myDel(a, b);
        }
    }

    /// <summary>
    /// Prototype for functions taking one integer in parameter and returning an integer.
    /// </summary>
    public delegate int TakesOneIntReturnsInt(int a);

    [Benchmark]
    public void DelegateInstanceMethod()
    {
        TakesOneIntReturnsInt myDel = AddInstance;

        int loops = Loops;
        for (int a = 0; a < loops; a++)
        {
            int c = myDel(a);
        }
    }
    private int AddInstance(int a) => a + _b;
}
