# Add delegate execution
## Description
This benchmark compares different methods of executing a static/instance delegate adding two integers built dynamically, equivalent to:
```csharp
for (int i = 0; i < loops; i++)
{
    int sum = 1 + 1;
}
```

The implementations are executed `loops` times:
- to avoid getting tiny durations considered by Benchmark.net are close to zero
- to minimize the penalty of creating delegates and focus on execution

In this document:
Static delegate=delegate instance on a static method (no target).
Instance delegate=delegate instance on an instance method (with a target).

|                          Method |                                                                                                        Description |
|-------------------------------- |------------------------------------------------------------------------------------------------------------------- |
|                      NoFunction |                                              Baseline implementation without using a function (same code as above) |
|                      NoFunction |                                                              Addition implemented as a static method (not dynamic) |
|   DynamicMethodStaticDelegate   |                                               Creates a DynamicMethod, emits IL code and creates a static delegate |
|   DynamicMethodInstanceDelegate |                                            Creates a DynamicMethod, emits IL code and creates an instance delegate |
|       DynamicTypeStaticDelegate |                        Builds a new type in a new module, adds a static method to it and creates a static delegate |
|     DynamicTypeInstanceDelegate |              Builds a new new type in a new module, adds an instance method to it and creates an instance delegate |
|             ExpressionTreeBuilt |                                                                  Builds an expression tree and creates a delegate  |
|        ExpressionTreeFromLambda |                                                      Gets an expression tree from a lambda and creates a delegate  |
|         LoopDynamicMethodStatic |    Creates a DynamicMethod, emits IL to create a loop executing a static delegate and creates an instance delegate |
|       LoopDynamicMethodInstance | Creates a DynamicMethod, emits IL to create a loop executing an instance delegate and creates an instance delegate |
|       LoopDynamicMethodEmbedded |      Creates a DynamicMethod, emits IL to create a loop implementing the addition and creates an instance delegate |

These tests only execute delegates, they don't compile them.

## Environment
<p>
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)<br/>
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores<br/>
.NET SDK=7.0.203<br/>
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256<br/>
</p>

## Results
|                        Method |              Runtime |  Loops |         Mean |       StdDev |       Median | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|------------------------------ |--------------------- |------- |-------------:|-------------:|-------------:|------:|-------:|-------:|-------:|----------:|------------:|
|                    NoFunction |             .NET 7.0 |   1000 |     361.9 ns |      1.66 ns |     361.5 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |   1000 |     358.1 ns |     21.45 ns |     362.2 ns |  0.92 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |   1000 |   1,609.3 ns |     12.93 ns |   1,607.9 ns |  4.45 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |   1000 |   1,600.1 ns |      4.68 ns |   1,599.4 ns |  4.42 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |   1000 |   1,879.9 ns |    253.96 ns |   1,821.1 ns |  5.45 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |   1000 |   1,696.0 ns |    104.39 ns |   1,647.9 ns |  4.65 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 |   1000 |   1,114.5 ns |      5.84 ns |   1,113.7 ns |  3.08 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |   1000 |   1,617.8 ns |     26.79 ns |   1,607.8 ns |  4.49 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |   1000 |  28,843.9 ns |    336.24 ns |  28,888.9 ns | 79.68 | 0.0916 | 0.0763 |      - |    1215 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |   1000 |  14,293.8 ns |    308.75 ns |  14,184.7 ns | 39.57 | 0.0916 | 0.0763 |      - |    1215 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |   1000 |     268.5 ns |      3.09 ns |     267.4 ns |  0.74 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |   1000 |     266.1 ns |      1.02 ns |     265.7 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |   1000 |     268.1 ns |      0.78 ns |     267.9 ns |  1.01 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |   1000 |   1,823.1 ns |      5.41 ns |   1,822.9 ns |  6.85 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |   1000 |   1,297.1 ns |      3.82 ns |   1,296.4 ns |  4.87 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |   1000 |   1,824.4 ns |      3.38 ns |   1,823.4 ns |  6.85 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |   1000 |   1,294.6 ns |      1.21 ns |   1,294.0 ns |  4.86 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |   1000 |   1,037.5 ns |      0.69 ns |   1,037.4 ns |  3.90 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |   1000 |   2,072.2 ns |      1.75 ns |   2,071.5 ns |  7.79 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |   1000 |  15,009.5 ns |     96.56 ns |  14,993.2 ns | 56.39 | 0.1984 | 0.0916 | 0.0153 |    1284 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |   1000 |  15,133.5 ns |    115.66 ns |  15,091.0 ns | 56.86 | 0.1984 | 0.0916 | 0.0153 |    1284 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |   1000 |     267.2 ns |      0.36 ns |     267.1 ns |  1.00 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction |             .NET 7.0 |  10000 |   2,582.7 ns |      0.96 ns |   2,582.3 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |  10000 |   2,584.4 ns |      3.00 ns |   2,583.1 ns |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |  10000 |  23,156.7 ns |     32.95 ns |  23,172.0 ns |  8.97 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |  10000 |  18,038.6 ns |     18.32 ns |  18,038.3 ns |  6.99 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |  10000 |  19,931.5 ns |  2,366.01 ns |  21,181.2 ns |  7.94 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |  10000 |  16,712.2 ns |  1,087.80 ns |  16,041.0 ns |  6.68 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 |  10000 |  10,998.5 ns |     66.46 ns |  11,010.3 ns |  4.26 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |  10000 |  18,830.7 ns |  3,126.31 ns |  16,834.8 ns |  6.97 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |  10000 |  25,810.9 ns |    211.79 ns |  25,710.9 ns |  9.98 | 0.0916 | 0.0610 |      - |    1215 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |  10000 |  26,123.3 ns |    429.20 ns |  26,046.8 ns | 10.08 | 0.0916 | 0.0610 |      - |    1215 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |  10000 |   2,594.0 ns |      5.75 ns |   2,593.0 ns |  1.00 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |  10000 |   2,586.4 ns |      4.82 ns |   2,585.1 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |  10000 |   2,587.3 ns |      4.34 ns |   2,584.7 ns |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |  10000 |  18,051.5 ns |     12.63 ns |  18,046.4 ns |  6.98 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |  10000 |  12,888.0 ns |      9.90 ns |  12,883.9 ns |  4.98 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |  10000 |  18,036.3 ns |      7.36 ns |  18,034.7 ns |  6.97 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |  10000 |  12,888.0 ns |     10.93 ns |  12,884.9 ns |  4.98 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  10000 |  10,319.6 ns |      5.69 ns |  10,317.5 ns |  3.99 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  10000 |  26,255.0 ns |  1,836.80 ns |  26,713.2 ns |  8.74 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |  10000 |  90,077.8 ns |    389.00 ns |  90,103.6 ns | 34.84 | 0.1831 | 0.0610 |      - |    1283 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |  10000 |  91,131.0 ns |  1,231.22 ns |  91,266.0 ns | 35.27 | 0.1831 | 0.0610 |      - |    1283 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |  10000 |   3,235.8 ns |    335.03 ns |   3,361.0 ns |  1.29 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction |             .NET 7.0 | 100000 |  33,219.8 ns |     97.98 ns |  33,219.7 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 | 100000 |  32,920.5 ns |    174.70 ns |  32,917.4 ns |  0.99 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 | 100000 | 159,984.2 ns |  1,194.61 ns | 159,925.1 ns |  4.82 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 | 100000 | 161,330.7 ns |  4,464.63 ns | 159,106.2 ns |  4.89 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 | 100000 | 192,260.0 ns | 24,809.67 ns | 211,648.4 ns |  5.94 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 | 100000 | 170,556.7 ns | 10,801.55 ns | 168,428.5 ns |  5.14 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 | 100000 | 110,016.3 ns |  1,086.43 ns | 109,649.6 ns |  3.31 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 | 100000 | 173,509.9 ns | 19,870.60 ns | 160,034.4 ns |  5.16 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 | 100000 | 199,693.3 ns |  1,281.02 ns | 199,920.6 ns |  6.01 |      - |      - |      - |    1192 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 | 100000 | 200,331.5 ns |  2,908.39 ns | 200,484.9 ns |  6.02 |      - |      - |      - |    1192 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 | 100000 |  32,481.1 ns |  1,028.90 ns |  32,621.8 ns |  0.97 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 | 100000 |  32,953.9 ns |     87.45 ns |  32,951.7 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 | 100000 |  31,814.5 ns |    868.23 ns |  31,937.1 ns |  0.96 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 | 100000 | 263,348.7 ns | 10,221.82 ns | 264,697.4 ns |  7.88 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 | 100000 | 158,672.8 ns |  4,616.34 ns | 159,083.2 ns |  4.80 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 | 100000 | 266,708.7 ns |  1,289.71 ns | 266,327.1 ns |  8.09 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 | 100000 | 157,286.2 ns |  7,244.39 ns | 159,009.3 ns |  4.59 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 | 100000 | 107,655.6 ns |    570.16 ns | 107,404.6 ns |  3.27 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 | 100000 | 266,147.8 ns |    939.30 ns | 266,283.6 ns |  8.07 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 | 100000 | 664,830.2 ns | 38,112.22 ns | 674,070.1 ns | 18.47 |      - |      - |      - |    1264 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 | 100000 | 666,512.5 ns | 36,747.14 ns | 676,388.6 ns | 18.65 |      - |      - |      - |    1264 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 | 100000 |  33,279.9 ns |    191.09 ns |  33,294.7 ns |  1.01 |      - |      - |      - |         - |          NA |

## Conclusions
### .NET 7.0

- StaticFunction is inlined and LoopDynamicMethodEmbedded shows similar performance, sometimes even better when loops is small
- DynamicMethodStaticDelegate is slower than the DynamicMethod cases bases on instance delegates
- DynamicMethodInstanceDelegate, DynamicTypeStaticDelegate and ExpressionTreeFromLambda show similar performance since they 
- Instance delegates are faster than static delegates because of [this](https://stackoverflow.com/a/42187448/446279)
- Framework is faster than core

### Delegates performance
Static delegates execution require some arguments reshuffling ([details](https://stackoverflow.com/a/42187448/446279)) and are slower, however, their instances can be cached during the first invocation and reused during the next invocation.
Instance delegates do not require arguments reshuffling and are faster than static delegates, however, instances cannot be cached and have to be recreated.
Lambdas are the best of both worlds, the compiler generates a class with a delegate field, instantiates an instance during the first invocation and caches it. This instance is used as the delegate's target. Lambdas are therefore fast and don't allocate.

| Delegate type | Instantiation                    | Target                                       |
|-------------- |--------------------------------- |--------------------------------------------- |
| Instance      | For each invocation              | Not null (instance)                          |
| Static        | Only during the first invocation | Null                                         |
| Lambda        | Only during the first invocation | Not null (compiler generated class instance) |

