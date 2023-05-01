# Dynamically compiled function execution

## Description
This benchmark compares different methods of executing a static/instance delegate adding two integers built dynamically, equivalent to:

## Benchmarks
The benchmarks are the same as the [compilation and execution benchmarks](../CompilationAndExecution) except the compilation part is not included.

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
|                        Method |              Runtime |  Loops |         Mean |      StdDev | Ratio |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------------------------ |--------------------- |------- |-------------:|------------:|------:|-------:|-------:|----------:|------------:|
|                    NoFunction |             .NET 7.0 |   1000 |     266.2 ns |     0.35 ns |  1.00 |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |   1000 |     266.1 ns |     0.24 ns |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |   1000 |   2,332.0 ns |     1.70 ns |  8.76 |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |   1000 |   1,824.1 ns |     2.46 ns |  6.85 |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |   1000 |   2,075.0 ns |     2.03 ns |  7.79 |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |   1000 |   1,559.1 ns |     1.26 ns |  5.86 |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 |   1000 |   1,301.4 ns |     0.76 ns |  4.89 |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |   1000 |   2,338.4 ns |     2.94 ns |  8.78 |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |   1000 |  12,960.6 ns |   127.41 ns | 48.69 | 0.0916 | 0.0763 |    1158 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |   1000 |  13,143.4 ns |   164.25 ns | 49.40 | 0.0916 | 0.0763 |    1165 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |   1000 |     268.0 ns |     0.57 ns |  1.01 |      - |      - |         - |          NA |
|      LoopExpressionTreeStatic |             .NET 7.0 |   1000 |  18,085.4 ns |   650.95 ns | 68.36 | 0.0916 | 0.0763 |    1166 B |          NA |
|    LoopExpressionTreeInstance |             .NET 7.0 |   1000 |  13,373.7 ns |    91.19 ns | 50.20 | 0.0916 | 0.0763 |    1167 B |          NA |
|    LoopExpressionTreeEmbedded |             .NET 7.0 |   1000 |     266.3 ns |     0.26 ns |  1.00 |      - |      - |         - |          NA |
|                               |                      |        |              |             |       |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |   1000 |     266.2 ns |     0.24 ns |  1.00 |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |   1000 |     266.0 ns |     0.57 ns |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |   1000 |   2,006.3 ns |    48.17 ns |  7.53 |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |   1000 |   1,321.6 ns |     8.88 ns |  4.97 |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |   1000 |   1,944.0 ns |    29.49 ns |  7.30 |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |   1000 |   1,324.2 ns |    11.82 ns |  4.97 |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |   1000 |   1,291.7 ns |    11.39 ns |  4.85 |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |   1000 |   2,309.2 ns |    29.20 ns |  8.68 |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |   1000 |  17,626.7 ns |    59.26 ns | 66.22 | 0.1831 | 0.0916 |    1228 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |   1000 |  17,727.0 ns |    51.12 ns | 66.60 | 0.1831 | 0.0916 |    1235 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |   1000 |     267.8 ns |     0.40 ns |  1.01 |      - |      - |         - |          NA |
|      LoopExpressionTreeStatic | .NET Framework 4.7.2 |   1000 |  17,656.4 ns |    48.35 ns | 66.33 | 0.1831 | 0.0916 |    1235 B |          NA |
|    LoopExpressionTreeInstance | .NET Framework 4.7.2 |   1000 |  17,812.4 ns |    89.85 ns | 66.92 | 0.1831 | 0.0916 |    1235 B |          NA |
|    LoopExpressionTreeEmbedded | .NET Framework 4.7.2 |   1000 |     267.3 ns |     0.95 ns |  1.00 |      - |      - |         - |          NA |
|                               |                      |        |              |             |       |        |        |           |             |
|                    NoFunction |             .NET 7.0 |  10000 |   2,590.4 ns |     2.46 ns |  1.00 |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |  10000 |   2,597.5 ns |    13.62 ns |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |  10000 |  20,662.0 ns |    32.59 ns |  7.98 |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |  10000 |  15,497.2 ns |    10.59 ns |  5.98 |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |  10000 |  23,223.5 ns |    42.99 ns |  8.96 |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |  10000 |  18,208.8 ns |   105.77 ns |  7.03 |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 |  10000 |  10,350.1 ns |    14.70 ns |  4.00 |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |  10000 |  20,675.2 ns |    29.90 ns |  7.98 |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |  10000 |  30,145.4 ns |   300.54 ns | 11.64 | 0.0916 | 0.0610 |    1155 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |  10000 |  25,239.8 ns |    75.61 ns |  9.74 | 0.0916 | 0.0610 |    1167 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |  10000 |   2,593.4 ns |     3.23 ns |  1.00 |      - |      - |         - |          NA |
|      LoopExpressionTreeStatic |             .NET 7.0 |  10000 |  25,453.0 ns |   288.18 ns |  9.83 | 0.0916 | 0.0610 |    1168 B |          NA |
|    LoopExpressionTreeInstance |             .NET 7.0 |  10000 |  27,963.0 ns |   180.42 ns | 10.79 | 0.0916 | 0.0610 |    1168 B |          NA |
|    LoopExpressionTreeEmbedded |             .NET 7.0 |  10000 |   2,584.0 ns |     2.10 ns |  1.00 |      - |      - |         - |          NA |
|                               |                      |        |              |             |       |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |  10000 |   2,585.2 ns |     2.61 ns |  1.00 |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |  10000 |   2,585.3 ns |     3.97 ns |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |  10000 |  20,514.9 ns |   107.28 ns |  7.94 |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |  10000 |  13,604.0 ns |   194.71 ns |  5.26 |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |  10000 |  19,470.7 ns |   125.72 ns |  7.53 |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |  10000 |  13,400.7 ns |    89.29 ns |  5.18 |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  10000 |  12,772.6 ns |   106.93 ns |  4.94 |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  10000 |  23,052.7 ns |   176.46 ns |  8.92 |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |  10000 |  67,769.6 ns |   195.99 ns | 26.21 | 0.1221 | 0.1221 |    1220 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |  10000 |  63,487.1 ns |   103.60 ns | 24.56 | 0.1221 | 0.1221 |    1227 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |  10000 |   2,593.6 ns |     3.08 ns |  1.00 |      - |      - |         - |          NA |
|      LoopExpressionTreeStatic | .NET Framework 4.7.2 |  10000 |  63,374.9 ns |    99.19 ns | 24.51 | 0.1221 | 0.1221 |    1227 B |          NA |
|    LoopExpressionTreeInstance | .NET Framework 4.7.2 |  10000 |  63,501.0 ns |    79.09 ns | 24.56 | 0.1221 | 0.1221 |    1227 B |          NA |
|    LoopExpressionTreeEmbedded | .NET Framework 4.7.2 |  10000 |   2,590.4 ns |     1.52 ns |  1.00 |      - |      - |         - |          NA |
|                               |                      |        |              |             |       |        |        |           |             |
|                    NoFunction |             .NET 7.0 | 100000 |  26,018.5 ns |   357.51 ns |  1.00 |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 | 100000 |  25,842.4 ns |    28.51 ns |  0.99 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 | 100000 | 206,437.0 ns |    87.29 ns |  7.93 |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 | 100000 | 154,882.8 ns |    55.20 ns |  5.95 |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 | 100000 | 232,295.0 ns |   106.57 ns |  8.93 |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate |             .NET 7.0 | 100000 | 181,584.4 ns |   116.32 ns |  6.98 |      - |      - |         - |          NA |
|           ExpressionTreeBuilt |             .NET 7.0 | 100000 | 103,339.4 ns |   116.42 ns |  3.97 |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 | 100000 | 206,439.9 ns |   108.06 ns |  7.94 |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 | 100000 | 214,369.5 ns |   286.09 ns |  8.24 |      - |      - |    1136 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 | 100000 | 163,029.0 ns |    90.72 ns |  6.27 |      - |      - |    1144 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 | 100000 |  25,840.3 ns |    20.27 ns |  0.99 |      - |      - |         - |          NA |
|      LoopExpressionTreeStatic |             .NET 7.0 | 100000 | 163,141.3 ns |   392.99 ns |  6.27 |      - |      - |    1144 B |          NA |
|    LoopExpressionTreeInstance |             .NET 7.0 | 100000 | 188,752.6 ns |   159.88 ns |  7.25 |      - |      - |    1144 B |          NA |
|    LoopExpressionTreeEmbedded |             .NET 7.0 | 100000 |  25,843.4 ns |    21.20 ns |  0.99 |      - |      - |         - |          NA |
|                               |                      |        |              |             |       |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 | 100000 |  25,837.2 ns |     4.34 ns |  1.00 |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 | 100000 |  25,833.6 ns |     7.34 ns |  1.00 |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 | 100000 | 206,011.0 ns |   297.53 ns |  7.97 |      - |      - |         - |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 | 100000 | 135,347.1 ns | 1,260.85 ns |  5.24 |      - |      - |         - |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 | 100000 | 204,123.0 ns |   640.12 ns |  7.90 |      - |      - |         - |          NA |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 | 100000 | 134,594.9 ns | 1,792.34 ns |  5.21 |      - |      - |         - |          NA |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 | 100000 | 127,842.5 ns |   325.50 ns |  4.95 |      - |      - |         - |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 | 100000 | 231,837.8 ns |   436.75 ns |  8.97 |      - |      - |         - |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 | 100000 | 573,919.5 ns | 3,012.16 ns | 22.22 |      - |      - |    1208 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 | 100000 | 521,814.4 ns | 2,253.52 ns | 20.20 |      - |      - |    1216 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 | 100000 |  25,842.0 ns |    28.73 ns |  1.00 |      - |      - |         - |          NA |
|      LoopExpressionTreeStatic | .NET Framework 4.7.2 | 100000 | 521,515.5 ns | 1,004.60 ns | 20.18 |      - |      - |    1216 B |          NA |
|    LoopExpressionTreeInstance | .NET Framework 4.7.2 | 100000 | 524,690.1 ns |   579.67 ns | 20.31 |      - |      - |    1216 B |          NA |
|    LoopExpressionTreeEmbedded | .NET Framework 4.7.2 | 100000 |  25,853.1 ns |    52.30 ns |  1.00 |      - |      - |         - |          NA |

## Conclusions
- StaticFunction (inlined) and LoopDynamicMethodEmbedded shows similar performance
- DynamicMethodStaticDelegate, DynamicTypeStaticDelegate and ExpressionTreeFromLambda show similar performance since they are based on static delegates
- Framework is faster than core to invoke delegates from dynamically built functions
