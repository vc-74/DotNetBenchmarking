Conclusions:
- core faster than framework
- no major differences between the implementation types

// * Summary *

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.203
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256


|           Method |              Runtime |     Mean |    StdDev | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|----------------- |--------------------- |---------:|----------:|------:|-------:|-------:|-------:|----------:|------------:|
|   StaticDelegate |             .NET 7.0 | 2.925 us | 0.1642 us |  0.72 | 0.1335 | 0.1297 | 0.0229 |   1.58 KB |        0.91 |
|   StaticDelegate | .NET Framework 4.7.2 | 4.076 us | 0.1897 us |  1.00 | 0.2785 | 0.1373 | 0.0305 |   1.73 KB |        1.00 |
|                  |                      |          |           |       |        |        |        |           |             |
| InstanceDelegate |             .NET 7.0 | 2.985 us | 0.1668 us |  0.75 | 0.1297 | 0.1259 | 0.0153 |   1.58 KB |        0.91 |
| InstanceDelegate | .NET Framework 4.7.2 | 3.978 us | 0.2616 us |  1.00 | 0.2747 | 0.1373 | 0.0229 |   1.73 KB |        1.00 |
|                  |                      |          |           |       |        |        |        |           |             |
|         Embedded |             .NET 7.0 | 2.771 us | 0.1619 us |  0.71 | 0.1373 | 0.1335 | 0.0153 |   1.55 KB |        0.91 |
|         Embedded | .NET Framework 4.7.2 | 3.931 us | 0.1786 us |  1.00 | 0.2747 | 0.1373 | 0.0267 |    1.7 KB |        1.00 |
