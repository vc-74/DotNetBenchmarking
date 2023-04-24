#if NET7_0
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking;

/// <summary>
/// Compares methods of creating an <see cref="ArgumentException"/> referring to an expression.
/// </summary>
[MemoryDiagnoser]
public class ExpressionException
{
    [Benchmark(Baseline = true)]
    public void String() => String(5);

    private static void String(int x)
    {
        for (int i = 0; i < 100; i++)
        {
            try
            {
                if (x < 12)
                {
                    throw new ArgumentException("x < 12 is false", nameof(x));
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    [Benchmark]
    public void Expression() => Expression(5);

    public static void CheckInt(int x,
        [CallerArgumentExpression(nameof(x))] string? xExpression = null)
    {
        if (x < 12)
        {
            throw new ArgumentException($"'{xExpression}' is < 12", nameof(x));
        }
    }

    private static void Expression(int x)
    {
        for (int i = 0; i < 100; i++)
        {
            try
            {
                CheckInt(x + 1);

                throw new NotImplementedException();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
#endif
