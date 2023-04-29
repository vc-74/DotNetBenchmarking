# Dynamically compiled function execution

## Description
This benchmark compares different methods of compiling and executing a static/instance delegate adding two integers built dynamically, equivalent to:
```csharp
int a = 2;

for (int i = 0; i < loops; i++)
{
	int sum = a + i;
}
```

The idea here is to determine the number of loops before which the compilation is the main driver for the compilation/duration duration.

|                        Method |              Runtime |  Loops |       Mean |     StdDev | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|------------------------------ |--------------------- |------- |-----------:|-----------:|------:|-------:|-------:|-------:|----------:|------------:|
|                    NoFunction |             .NET 7.0 |  10000 |   2.019 us |  0.0044 us |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |  10000 |   2.022 us |  0.0066 us |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |  10000 |  27.528 us |  0.1412 us | 13.63 | 0.0916 | 0.0610 |      - |    1199 B |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |  10000 |  23.859 us |  0.3422 us | 11.82 | 0.0916 | 0.0610 |      - |    1215 B |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |  10000 | 184.379 us | 31.9035 us |     ? | 0.3662 | 0.1221 |      - |    5296 B |           ? |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |  10000 | 183.393 us | 32.2308 us |     ? | 0.3662 | 0.1221 |      - |    5624 B |           ? |
|           ExpressionTreeBuilt |             .NET 7.0 |  10000 |  22.008 us |  0.2025 us | 10.90 | 0.3662 | 0.3357 |      - |    4783 B |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |  10000 |  31.947 us |  0.1951 us | 15.82 | 0.3662 | 0.3052 |      - |    4847 B |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |  10000 |  48.854 us |  0.6664 us | 24.19 | 0.1831 | 0.1221 |      - |    2831 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |  10000 |  48.669 us |  0.6067 us | 24.10 | 0.1831 | 0.1221 |      - |    2831 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |  10000 |  19.296 us |  0.4720 us |  9.52 | 0.0916 | 0.0610 |      - |    1506 B |          NA |
|                               |                      |        |            |            |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |  10000 |   2.025 us |  0.0074 us |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |  10000 |   2.020 us |  0.0040 us |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |  10000 |  25.026 us |  0.0660 us | 12.36 | 0.1831 | 0.0916 |      - |    1266 B |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |  10000 |  18.990 us |  0.0970 us |  9.37 | 0.1831 | 0.0916 |      - |    1282 B |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |  10000 | 206.167 us | 31.7855 us |     ? | 0.9766 | 0.2441 |      - |    6460 B |           ? |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |  10000 | 205.644 us | 31.4980 us |     ? | 0.9766 | 0.2441 |      - |    6700 B |           ? |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  10000 |  21.914 us |  0.4297 us | 10.85 | 0.8240 | 0.3967 | 0.0305 |    5316 B |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  10000 |  26.942 us |  0.1031 us | 13.30 | 0.7935 | 0.3967 | 0.0305 |    5041 B |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |  10000 |  95.329 us |  1.3010 us | 47.04 | 0.3662 | 0.2441 |      - |    3048 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |  10000 |  94.306 us |  0.9592 us | 46.51 | 0.3662 | 0.2441 |      - |    3048 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |  10000 |  20.471 us |  0.1133 us | 10.11 | 0.2441 | 0.1221 |      - |    1645 B |          NA |
|                               |                      |        |            |            |       |        |        |        |           |             |
|                    NoFunction |             .NET 7.0 |  50000 |  10.097 us |  0.0646 us |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 |  50000 |  10.102 us |  0.0412 us |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 |  50000 | 100.737 us |  0.1374 us |  9.97 |      - |      - |      - |    1176 B |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 |  50000 |  70.217 us |  0.9016 us |  6.96 |      - |      - |      - |    1192 B |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 |  50000 | 191.741 us | 13.5252 us |     ? | 0.2441 |      - |      - |    5312 B |           ? |
|   DynamicTypeInstanceDelegate |             .NET 7.0 |  50000 | 173.022 us | 13.7605 us |     ? | 0.2441 |      - |      - |    5640 B |           ? |
|           ExpressionTreeBuilt |             .NET 7.0 |  50000 |  54.510 us |  0.4416 us |  5.40 | 0.3662 | 0.3052 |      - |    4783 B |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 |  50000 |  94.837 us |  0.2135 us |  9.39 | 0.3662 | 0.2441 |      - |    4847 B |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 |  50000 |  98.346 us |  2.0101 us |  9.79 | 0.1221 |      - |      - |    2818 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 |  50000 |  97.319 us |  0.6586 us |  9.64 | 0.1221 |      - |      - |    2818 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |  50000 |  26.572 us |  0.2635 us |  2.63 | 0.0916 | 0.0610 |      - |    1506 B |          NA |
|                               |                      |        |            |            |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 |  50000 |  10.077 us |  0.0135 us |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 |  50000 |  10.073 us |  0.0111 us |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |  50000 |  91.915 us |  0.3155 us |  9.12 | 0.1221 | 0.1221 |      - |    1259 B |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |  50000 |  60.905 us |  0.1451 us |  6.05 | 0.1221 | 0.1221 |      - |    1276 B |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 |  50000 | 206.942 us | 14.3653 us |     ? | 0.9766 | 0.2441 |      - |    6460 B |           ? |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 |  50000 | 187.147 us |  9.0823 us |     ? | 0.9766 | 0.2441 |      - |    6700 B |           ? |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  50000 |  55.559 us |  0.1278 us |  5.52 | 0.7935 | 0.3662 |      - |    5316 B |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  50000 |  86.209 us |  0.4082 us |  8.56 | 0.7324 | 0.3662 |      - |    5039 B |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |  50000 | 246.268 us |  4.0862 us | 24.47 |      - |      - |      - |    3012 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |  50000 | 246.080 us |  3.5799 us | 24.40 | 0.2441 | 0.2441 |      - |    3036 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |  50000 |  28.067 us |  0.1510 us |  2.79 | 0.2441 | 0.1221 |      - |    1645 B |          NA |
|                               |                      |        |            |            |       |        |        |        |           |             |
|                    NoFunction |             .NET 7.0 | 100000 |  20.145 us |  0.0378 us |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction |             .NET 7.0 | 100000 |  20.321 us |  0.3031 us |  1.01 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate |             .NET 7.0 | 100000 | 174.793 us |  0.4228 us |  8.68 |      - |      - |      - |    1176 B |          NA |
| DynamicMethodInstanceDelegate |             .NET 7.0 | 100000 | 131.869 us |  0.2016 us |  6.55 |      - |      - |      - |    1192 B |          NA |
|     DynamicTypeStaticDelegate |             .NET 7.0 | 100000 | 286.238 us | 13.2977 us |     ? | 0.2441 |      - |      - |    5312 B |           ? |
|   DynamicTypeInstanceDelegate |             .NET 7.0 | 100000 | 251.118 us | 21.6477 us |     ? | 0.2441 |      - |      - |    5640 B |           ? |
|           ExpressionTreeBuilt |             .NET 7.0 | 100000 |  94.482 us |  0.2254 us |  4.69 | 0.3662 | 0.2441 |      - |    4783 B |          NA |
|      ExpressionTreeFromLambda |             .NET 7.0 | 100000 | 177.554 us |  0.3448 us |  8.81 | 0.2441 |      - |      - |    4839 B |          NA |
|       LoopDynamicMethodStatic |             .NET 7.0 | 100000 | 159.683 us |  0.3452 us |  7.93 |      - |      - |      - |    2792 B |          NA |
|     LoopDynamicMethodInstance |             .NET 7.0 | 100000 | 159.789 us |  0.3367 us |  7.93 |      - |      - |      - |    2792 B |          NA |
|     LoopDynamicMethodEmbedded |             .NET 7.0 | 100000 |  34.503 us |  0.1325 us |  1.71 | 0.0610 |      - |      - |    1500 B |          NA |
|                               |                      |        |            |            |       |        |        |        |           |             |
|                    NoFunction | .NET Framework 4.7.2 | 100000 |  20.127 us |  0.0346 us |  1.00 |      - |      - |      - |         - |          NA |
|                StaticFunction | .NET Framework 4.7.2 | 100000 |  20.149 us |  0.0467 us |  1.00 |      - |      - |      - |         - |          NA |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 | 100000 | 176.056 us |  0.8218 us |  8.75 |      - |      - |      - |    1244 B |          NA |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 | 100000 | 113.708 us |  0.2026 us |  5.65 | 0.1221 | 0.1221 |      - |    1276 B |          NA |
|     DynamicTypeStaticDelegate | .NET Framework 4.7.2 | 100000 | 281.382 us | 10.6636 us |     ? | 0.9766 |      - |      - |    6428 B |           ? |
|   DynamicTypeInstanceDelegate | .NET Framework 4.7.2 | 100000 | 238.085 us | 12.9106 us |     ? | 0.9766 | 0.2441 |      - |    6700 B |           ? |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 | 100000 |  98.197 us |  0.3003 us |  4.88 | 0.7324 | 0.3662 |      - |    5314 B |          NA |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 | 100000 | 159.754 us |  0.5082 us |  7.93 | 0.7324 | 0.2441 |      - |    5040 B |          NA |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 | 100000 | 436.453 us |  6.9390 us | 21.75 |      - |      - |      - |    3012 B |          NA |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 | 100000 | 434.824 us |  4.6301 us | 21.62 |      - |      - |      - |    3012 B |          NA |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 | 100000 |  37.588 us |  0.2083 us |  1.87 | 0.2441 | 0.1221 |      - |    1645 B |          NA |

## Conclusions
- At 10K, ratio is 9.52, at 50K: 2.6, at 100K: 1.71 4* faster than the dynamic method instance delegate case
- Performance .NET 7.0 gets better vs .NET Framework 4.7.2 as loops increases
