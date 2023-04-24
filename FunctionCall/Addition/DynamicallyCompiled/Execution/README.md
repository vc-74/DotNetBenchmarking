# Add delegate execution
## Description
This benchmark compares different methods of executing a static/instance delegate adding two integers built dynamically:

## Results
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.203
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256


|                    Method |              Runtime |  Loops |       Mean |    StdDev | Ratio | Allocated | Alloc Ratio |
|-------------------------- |--------------------- |------- |-----------:|----------:|------:|----------:|------------:|
|     DynamicMethodInstance |             .NET 7.0 |   1000 |   1.559 us | 0.0078 us |  1.00 |         - |          NA |
|       DynamicMethodStatic |             .NET 7.0 |   1000 |   2.329 us | 0.0054 us |  1.49 |         - |          NA |
| DynamicTypeMethodInstance |             .NET 7.0 |   1000 |   1.792 us | 0.0135 us |  1.15 |         - |          NA |
|   DynamicTypeMethodStatic |             .NET 7.0 |   1000 |   2.327 us | 0.0048 us |  1.49 |         - |          NA |
|            ExpressionTree |             .NET 7.0 |   1000 |   1.295 us | 0.0021 us |  0.83 |         - |          NA |
|                           |                      |        |            |           |       |           |             |
|     DynamicMethodInstance | .NET Framework 4.7.2 |   1000 |   1.301 us | 0.0089 us |  1.00 |         - |          NA |
|       DynamicMethodStatic | .NET Framework 4.7.2 |   1000 |   1.823 us | 0.0043 us |  1.40 |         - |          NA |
| DynamicTypeMethodInstance | .NET Framework 4.7.2 |   1000 |   1.296 us | 0.0019 us |  1.00 |         - |          NA |
|   DynamicTypeMethodStatic | .NET Framework 4.7.2 |   1000 |   1.822 us | 0.0020 us |  1.40 |         - |          NA |
|            ExpressionTree | .NET Framework 4.7.2 |   1000 |   1.039 us | 0.0010 us |  0.80 |         - |          NA |
|                           |                      |        |            |           |       |           |             |
|     DynamicMethodInstance |             .NET 7.0 |  10000 |  17.830 us | 0.2255 us |  1.00 |         - |          NA |
|       DynamicMethodStatic |             .NET 7.0 |  10000 |  23.194 us | 0.0422 us |  1.30 |         - |          NA |
| DynamicTypeMethodInstance |             .NET 7.0 |  10000 |  17.839 us | 0.2430 us |  1.00 |         - |          NA |
|   DynamicTypeMethodStatic |             .NET 7.0 |  10000 |  23.189 us | 0.0168 us |  1.30 |         - |          NA |
|            ExpressionTree |             .NET 7.0 |  10000 |  12.856 us | 0.1453 us |  0.72 |         - |          NA |
|                           |                      |        |            |           |       |           |             |
|     DynamicMethodInstance | .NET Framework 4.7.2 |  10000 |  12.895 us | 0.0113 us |  1.00 |         - |          NA |
|       DynamicMethodStatic | .NET Framework 4.7.2 |  10000 |  18.072 us | 0.0494 us |  1.40 |         - |          NA |
| DynamicTypeMethodInstance | .NET Framework 4.7.2 |  10000 |  12.881 us | 0.0122 us |  1.00 |         - |          NA |
|   DynamicTypeMethodStatic | .NET Framework 4.7.2 |  10000 |  18.053 us | 0.0220 us |  1.40 |         - |          NA |
|            ExpressionTree | .NET Framework 4.7.2 |  10000 |  10.317 us | 0.0136 us |  0.80 |         - |          NA |
|                           |                      |        |            |           |       |           |             |
|     DynamicMethodInstance |             .NET 7.0 | 100000 | 178.207 us | 0.9945 us |  1.00 |         - |          NA |
|       DynamicMethodStatic |             .NET 7.0 | 100000 | 231.589 us | 0.4088 us |  1.30 |         - |          NA |
| DynamicTypeMethodInstance |             .NET 7.0 | 100000 | 177.880 us | 0.6732 us |  1.00 |         - |          NA |
|   DynamicTypeMethodStatic |             .NET 7.0 | 100000 | 231.680 us | 0.4425 us |  1.30 |         - |          NA |
|            ExpressionTree |             .NET 7.0 | 100000 | 128.240 us | 1.3682 us |  0.72 |         - |          NA |
|                           |                      |        |            |           |       |           |             |
|     DynamicMethodInstance | .NET Framework 4.7.2 | 100000 | 128.727 us | 0.1145 us |  1.00 |         - |          NA |
|       DynamicMethodStatic | .NET Framework 4.7.2 | 100000 | 180.483 us | 0.4220 us |  1.40 |         - |          NA |
| DynamicTypeMethodInstance | .NET Framework 4.7.2 | 100000 | 128.892 us | 0.3405 us |  1.00 |         - |          NA |
|   DynamicTypeMethodStatic | .NET Framework 4.7.2 | 100000 | 180.054 us | 0.0275 us |  1.40 |         - |          NA |
|            ExpressionTree | .NET Framework 4.7.2 | 100000 | 102.940 us | 0.0859 us |  0.80 |         - |          NA |

## Conclusions
- Instance delegates are faster than static delegates because of [this](https://stackoverflow.com/a/42187448/446279)
- Framework is faster than core
