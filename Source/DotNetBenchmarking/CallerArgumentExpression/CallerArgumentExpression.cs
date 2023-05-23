#if NET7_0
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking;

/// <summary>
/// Compares methods of creating an <see cref="ArgumentException"/> referring to an expression.
/// </summary>
[MemoryDiagnoser]
public class CallerArgumentExpression
{
    private const int Loops = 100;

    [Benchmark(Baseline = true)]
    public void String() => String(5);

    private void String(int x)
    {
        for (int i = 0; i < Loops; i++)
        {
            try
            {
                CheckInt(x + 1);
            }
            catch (ArgumentException)
            {
            }
        }
    }

    private static void CheckInt(int x)
    {
        if (x < 12)
        {
            throw new ArgumentException("x < 12 is false", nameof(x));
        }
    }

    [Benchmark]
    public void Expression() => Expression(5);

    private void Expression(int x)
    {
        for (int i = 0; i < Loops; i++)
        {
            try
            {
                CheckIntCallerArgumentExpression(x + 1);
            }
            catch (ArgumentException)
            {
            }
        }
    }

    private static void CheckIntCallerArgumentExpression(int x,
        [CallerArgumentExpression(nameof(x))] string? xExpression = null)
    {
        if (x < 12)
        {
            throw new ArgumentException($"'{xExpression}' is < 12", nameof(x));
        }
    }
}
#endif
