# Iterator benchmarks
This folder contains benchmarks comparing different methods of enumerating integers. 

## Environment
<p>
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.3208/21H2/November2021Update)
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.306
  [Host]     : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
</p>

## Benchmarks

### Array
Enumerates over an integer array.

### List
Enumerates over an integer list.

### IteratorMethod
Enumerates over the result of an iterator method.

### CustomEnumerable
Enumerates over a custom enumerable implementation.

## Results
|           Method |  Count |       Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------- |------- |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
|            Array |  10000 |   2.602 us | 0.0143 us | 0.0314 us |  1.00 |    0.00 |         - |          NA |
|             List |  10000 |   5.165 us | 0.0051 us | 0.0045 us |  1.98 |    0.02 |         - |          NA |
| CustomEnumerable |  10000 |  23.368 us | 0.2013 us | 0.1784 us |  8.97 |    0.11 |      24 B |          NA |
|   IteratorMethod |  10000 |  39.044 us | 0.4322 us | 0.3609 us | 14.98 |    0.15 |      40 B |          NA |
|                  |        |            |           |           |       |         |           |             |
|            Array | 100000 |  25.842 us | 0.0495 us | 0.0439 us |  1.00 |    0.00 |         - |          NA |
|             List | 100000 |  51.600 us | 0.0922 us | 0.0770 us |  2.00 |    0.00 |         - |          NA |
| CustomEnumerable | 100000 | 231.900 us | 0.2278 us | 0.1778 us |  8.97 |    0.02 |      24 B |          NA |
|   IteratorMethod | 100000 | 391.893 us | 2.5466 us | 2.2575 us | 15.16 |    0.10 |      40 B |          NA |

## Conclusions
- Enumerating over arrays benefits from JIT bound checks optimization
- Enumerating over lists is ~2* slower
- Custom enumerables are ~10* slower (+ insignificant allocation)
- Iterator methods are ~16* slower (+ insignificant allocation)
