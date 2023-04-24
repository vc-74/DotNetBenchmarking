﻿#if NET7_0
using BenchmarkDotNet.Attributes;

namespace DotNetBenchmarking.HashCode;

/// <summary>
/// Compares methods of calculating a hash code.
/// </summary>
[MemoryDiagnoser]
public class HashCodeTests
{
    /// <summary>
    /// Number of elements in a bit collection.
    /// </summary>
    [Params(10_000, 100_000)]
    public int Count;

    [Benchmark(Baseline = true)]
    public void HashCodeTest()
    {
        int hashCode = System.HashCode.Combine("CollectionName", DayOfWeek.Wednesday, 1, (object?)null, 3);
    }

    [Benchmark]
    public void MyHashCodeTest()
    {
        MyHashCode result = new("CollectionName");

        result += DayOfWeek.Wednesday;
        result += 1;
        result += null;
        result += 3;

        int hashCode = result;
    }
}
#endif
