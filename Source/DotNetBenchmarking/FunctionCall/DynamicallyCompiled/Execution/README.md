# Dynamically compiled function execution

## Description
This benchmark compares different methods of executing a static/instance delegate adding two integers built dynamically, equivalent to:
```csharp
int a = 2;

for (int i = 0; i < loops; i++)
{
	int sum = a + i;
}
```

|                          Method |                                                                                                        Description |
|-------------------------------- |------------------------------------------------------------------------------------------------------------------- |
|                      NoFunction |                                              Baseline implementation without using a function (same code as above) |
|                  StaticFunction |                                                              Addition implemented as a static method (not dynamic) |
|     DynamicMethodStaticDelegate |                                               Creates a DynamicMethod, emits IL code and creates a static delegate |
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
|                    NoFunction |             .NET 7.0 |   1000 |     266.1 ns |      0.67 ns |     266.1 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |   1000 |     265.5 ns |      0.48 ns |     265.2 ns |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |   1000 |   2,326.3 ns |      4.53 ns |   2,326.1 ns |  8.74 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |   1000 |   1,823.1 ns |      4.52 ns |   1,823.7 ns |  6.85 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |   1000 |   2,069.9 ns |      2.54 ns |   2,070.6 ns |  7.78 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |   1000 |   1,554.4 ns |      1.58 ns |   1,554.0 ns |  5.84 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 |   1000 |   1,297.6 ns |      0.28 ns |   1,297.5 ns |  4.88 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |   1000 |   2,333.8 ns |      4.32 ns |   2,332.7 ns |  8.77 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |   1000 |  14,351.0 ns |    313.83 ns |  14,383.8 ns | 53.61 | 0.0916 | 0.0763 |      - |    1215 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |   1000 |  13,739.2 ns |    154.73 ns |  13,759.2 ns | 51.62 | 0.0916 | 0.0763 |      - |    1215 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |   1000 |     266.3 ns |      0.74 ns |     266.5 ns |  1.00 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |   1000 |     265.4 ns |      0.59 ns |     265.3 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |   1000 |     264.9 ns |      0.26 ns |     264.8 ns |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |   1000 |   1,826.7 ns |     11.09 ns |   1,822.2 ns |  6.88 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |   1000 |   1,294.2 ns |      1.06 ns |   1,293.9 ns |  4.88 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |   1000 |   1,821.7 ns |      2.04 ns |   1,821.4 ns |  6.86 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |   1000 |   1,294.5 ns |      1.03 ns |   1,294.2 ns |  4.88 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |   1000 |   1,038.5 ns |      0.51 ns |   1,038.3 ns |  3.91 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |   1000 |   1,839.4 ns |     24.69 ns |   1,825.5 ns |  6.94 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |   1000 |  14,349.0 ns |     71.18 ns |  14,331.1 ns | 54.06 | 0.1984 | 0.0916 | 0.0153 |    1284 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |   1000 |  14,424.9 ns |    120.27 ns |  14,409.3 ns | 54.34 | 0.1984 | 0.0916 | 0.0153 |    1284 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |   1000 |     267.6 ns |      0.42 ns |     267.5 ns |  1.01 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction |             .NET 7.0 |  10000 |   2,582.3 ns |      1.04 ns |   2,582.0 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |  10000 |   2,583.9 ns |      1.49 ns |   2,584.0 ns |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |  10000 |  20,614.5 ns |     21.45 ns |  20,609.6 ns |  7.98 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |  10000 |  15,452.2 ns |      4.60 ns |  15,451.8 ns |  5.98 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |  10000 |  23,176.7 ns |      7.03 ns |  23,175.8 ns |  8.98 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |  10000 |  18,112.5 ns |     19.29 ns |  18,119.3 ns |  7.01 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 |  10000 |  10,311.5 ns |      6.96 ns |  10,308.7 ns |  3.99 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |  10000 |  20,599.8 ns |      6.67 ns |  20,597.0 ns |  7.98 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |  10000 |  26,483.5 ns |    288.04 ns |  26,525.8 ns | 10.25 | 0.0916 | 0.0610 |      - |    1215 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |  10000 |  26,446.4 ns |    211.97 ns |  26,427.5 ns | 10.24 | 0.0916 | 0.0610 |      - |    1215 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |  10000 |   2,584.8 ns |      2.12 ns |   2,584.7 ns |  1.00 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |  10000 |   2,583.8 ns |      3.86 ns |   2,581.9 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |  10000 |   2,582.5 ns |      1.57 ns |   2,581.8 ns |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |  10000 |  18,040.8 ns |      4.69 ns |  18,040.1 ns |  6.98 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |  10000 |  12,893.8 ns |     22.28 ns |  12,885.4 ns |  4.99 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |  10000 |  18,050.8 ns |     19.74 ns |  18,044.5 ns |  6.99 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |  10000 |  12,876.4 ns |      6.11 ns |  12,876.6 ns |  4.98 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  10000 |  10,318.1 ns |      5.20 ns |  10,318.0 ns |  3.99 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  10000 |  18,051.9 ns |     23.93 ns |  18,055.2 ns |  6.99 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |  10000 |  52,338.6 ns |    140.91 ns |  52,340.3 ns | 20.26 | 0.1831 | 0.0610 |      - |    1283 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |  10000 |  52,176.2 ns |    141.78 ns |  52,191.6 ns | 20.19 | 0.1831 | 0.0610 |      - |    1283 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |  10000 |   2,585.0 ns |      2.09 ns |   2,584.4 ns |  1.00 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction |             .NET 7.0 | 100000 |  25,758.8 ns |     13.98 ns |  25,754.8 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 | 100000 |  25,770.2 ns |     20.48 ns |  25,765.2 ns |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 | 100000 | 205,860.0 ns |     62.18 ns | 205,868.3 ns |  7.99 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 | 100000 | 154,489.9 ns |     87.18 ns | 154,452.5 ns |  6.00 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 | 100000 | 231,807.2 ns |    197.76 ns | 231,727.1 ns |  9.00 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 | 100000 | 181,160.9 ns |    154.24 ns | 181,142.5 ns |  7.03 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 | 100000 | 103,064.1 ns |     93.28 ns | 103,055.6 ns |  4.00 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 | 100000 | 203,412.4 ns | 10,388.14 ns | 206,029.6 ns |  7.38 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 | 100000 | 163,953.8 ns |    104.12 ns | 163,989.5 ns |  6.36 |      - |      - |      - |    1192 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 | 100000 | 164,227.8 ns |  1,195.95 ns | 163,677.5 ns |  6.38 |      - |      - |      - |    1192 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 | 100000 |  25,768.4 ns |     28.11 ns |  25,766.2 ns |  1.00 |      - |      - |      - |         - |          NA |
|                               |                      |        |              |              |              |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 | 100000 |  25,742.3 ns |      9.60 ns |  25,739.0 ns |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 | 100000 |  33,083.3 ns |    101.57 ns |  33,075.9 ns |  1.29 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 | 100000 | 238,756.8 ns |    564.73 ns | 238,600.1 ns |  9.27 |      - |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 | 100000 | 157,717.6 ns |  6,608.63 ns | 159,235.8 ns |  5.90 |      - |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 | 100000 | 200,210.2 ns | 27,720.39 ns | 180,322.9 ns |  8.97 |      - |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 | 100000 | 128,641.7 ns |     32.28 ns | 128,630.0 ns |  5.00 |      - |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 | 100000 | 103,090.1 ns |    111.48 ns | 103,091.6 ns |  4.01 |      - |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 | 100000 | 180,731.9 ns |    322.02 ns | 180,720.1 ns |  7.02 |      - |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 | 100000 | 431,389.8 ns |  1,908.55 ns | 431,299.3 ns | 16.75 |      - |      - |      - |    1260 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 | 100000 | 432,566.5 ns |  2,594.68 ns | 432,102.1 ns | 16.82 |      - |      - |      - |    1260 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 | 100000 |  25,807.7 ns |     60.34 ns |  25,786.7 ns |  1.00 |      - |      - |      - |         - |          NA |

## Conclusions
- StaticFunction (inlined) and LoopDynamicMethodEmbedded shows similar performance, sometimes even better when loops is small
- DynamicMethodStaticDelegate, DynamicTypeStaticDelegate and ExpressionTreeFromLambda show similar performance since they are based on static delegates
- Framework is faster than core to invoke delegates from dynamically built functions
