Conclusions:
- Static delegates slower than instance delegates as expected (more and more true as loops count increases)
- Core faster than framework
- Logarithmic scale for pre-compiled embedded for Core and framework

// * Summary *

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.203
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256


|                      Method |              Runtime |  Loops |       Mean |     StdDev |     Median | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|---------------------------- |--------------------- |------- |-----------:|-----------:|-----------:|------:|-------:|-------:|-------:|----------:|------------:|
|              StaticDelegate |             .NET 7.0 |   1000 |  43.985 us |  1.1609 us |  43.469 us |  0.62 | 0.1831 | 0.1221 |      - |   2.76 KB |        0.92 |
|            InstanceDelegate |             .NET 7.0 |   1000 |  43.373 us |  0.9171 us |  43.334 us |  0.61 | 0.1831 | 0.1221 |      - |   2.76 KB |        0.92 |
|                    Embedded |             .NET 7.0 |   1000 |  27.741 us |  0.6139 us |  27.611 us |  0.39 | 0.2136 | 0.1831 |      - |   2.74 KB |        0.92 |
|   PreCompiledStaticDelegate |             .NET 7.0 |   1000 |  14.918 us |  0.5947 us |  14.831 us |  0.21 | 0.0916 | 0.0763 |      - |   1.19 KB |        0.40 |
| PreCompiledInstanceDelegate |             .NET 7.0 |   1000 |  14.140 us |  0.2467 us |  14.140 us |  0.20 | 0.0916 | 0.0763 |      - |   1.19 KB |        0.40 |
|         PreCompiledEmbedded |             .NET 7.0 |   1000 |   2.941 us |  0.1637 us |   2.949 us |  0.04 | 0.1068 | 0.1030 | 0.0153 |   1.19 KB |        0.40 |
|              StaticDelegate | .NET Framework 4.7.2 |   1000 |  70.954 us |  1.0211 us |  70.865 us |  1.00 | 0.3662 | 0.2441 |      - |   2.99 KB |        1.00 |
|            InstanceDelegate | .NET Framework 4.7.2 |   1000 |  71.319 us |  0.8722 us |  71.187 us |  1.00 | 0.3662 | 0.2441 |      - |   2.99 KB |        1.00 |
|                    Embedded | .NET Framework 4.7.2 |   1000 |  34.819 us |  0.5303 us |  34.541 us |  0.49 | 0.4272 | 0.2441 |      - |   2.97 KB |        0.99 |
|   PreCompiledStaticDelegate | .NET Framework 4.7.2 |   1000 |  15.545 us |  0.3751 us |  15.438 us |  0.22 | 0.1984 | 0.0916 | 0.0153 |   1.27 KB |        0.42 |
| PreCompiledInstanceDelegate | .NET Framework 4.7.2 |   1000 |  15.424 us |  0.2404 us |  15.364 us |  0.22 | 0.1831 | 0.0916 |      - |   1.27 KB |        0.42 |
|         PreCompiledEmbedded | .NET Framework 4.7.2 |   1000 |   3.570 us |  0.1774 us |   3.600 us |  0.05 | 0.2098 | 0.0992 | 0.0191 |   1.27 KB |        0.42 |
|                             |                      |        |            |            |            |       |        |        |        |           |             |
|              StaticDelegate |             .NET 7.0 |  10000 |  57.298 us |  0.7243 us |  56.972 us |  0.53 | 0.1831 | 0.1221 |      - |   2.76 KB |        0.93 |
|            InstanceDelegate |             .NET 7.0 |  10000 |  56.504 us |  0.4583 us |  56.501 us |  0.52 | 0.1831 | 0.1221 |      - |   2.76 KB |        0.93 |
|                    Embedded |             .NET 7.0 |  10000 |  29.669 us |  0.2432 us |  29.704 us |  0.27 | 0.2136 | 0.1831 |      - |   2.74 KB |        0.92 |
|   PreCompiledStaticDelegate |             .NET 7.0 |  10000 |  26.176 us |  0.5797 us |  25.934 us |  0.24 | 0.0916 | 0.0610 |      - |   1.19 KB |        0.40 |
| PreCompiledInstanceDelegate |             .NET 7.0 |  10000 |  26.120 us |  0.2076 us |  26.114 us |  0.24 | 0.0916 | 0.0610 |      - |   1.19 KB |        0.40 |
|         PreCompiledEmbedded |             .NET 7.0 |  10000 |   4.744 us |  0.1380 us |   4.705 us |  0.04 | 0.0916 | 0.0839 |      - |   1.19 KB |        0.40 |
|              StaticDelegate | .NET Framework 4.7.2 |  10000 | 109.093 us |  1.4037 us | 108.706 us |  1.00 | 0.2441 | 0.2441 |      - |   2.98 KB |        1.00 |
|            InstanceDelegate | .NET Framework 4.7.2 |  10000 |  98.031 us |  2.8692 us |  96.601 us |  0.91 | 0.3662 | 0.2441 |      - |   2.99 KB |        1.00 |
|                    Embedded | .NET Framework 4.7.2 |  10000 |  31.932 us |  0.5430 us |  31.852 us |  0.29 | 0.4272 | 0.2441 |      - |   2.97 KB |        1.00 |
|   PreCompiledStaticDelegate | .NET Framework 4.7.2 |  10000 |  46.013 us |  0.6491 us |  46.067 us |  0.42 | 0.1831 | 0.0610 |      - |   1.27 KB |        0.43 |
| PreCompiledInstanceDelegate | .NET Framework 4.7.2 |  10000 |  46.018 us |  0.4841 us |  45.967 us |  0.42 | 0.1831 | 0.0610 |      - |   1.27 KB |        0.43 |
|         PreCompiledEmbedded | .NET Framework 4.7.2 |  10000 |   6.781 us |  0.1383 us |   6.764 us |  0.06 | 0.2060 | 0.0992 | 0.0229 |   1.27 KB |        0.43 |
|                             |                      |        |            |            |            |       |        |        |        |           |             |
|              StaticDelegate |             .NET 7.0 | 100000 | 167.723 us |  1.0909 us | 167.401 us |  0.37 |      - |      - |      - |   2.73 KB |        0.92 |
|            InstanceDelegate |             .NET 7.0 | 100000 | 167.683 us |  1.3912 us | 167.214 us |  0.37 |      - |      - |      - |   2.73 KB |        0.92 |
|                    Embedded |             .NET 7.0 | 100000 |  44.519 us |  0.6869 us |  44.346 us |  0.10 | 0.1831 | 0.1221 |      - |   2.73 KB |        0.92 |
|   PreCompiledStaticDelegate |             .NET 7.0 | 100000 | 138.001 us |  0.8516 us | 138.010 us |  0.30 |      - |      - |      - |   1.16 KB |        0.39 |
| PreCompiledInstanceDelegate |             .NET 7.0 | 100000 | 138.817 us |  0.4253 us | 138.646 us |  0.31 |      - |      - |      - |   1.16 KB |        0.39 |
|         PreCompiledEmbedded |             .NET 7.0 | 100000 |  23.705 us |  0.2401 us |  23.680 us |  0.05 | 0.0916 | 0.0610 |      - |   1.19 KB |        0.40 |
|              StaticDelegate | .NET Framework 4.7.2 | 100000 | 453.164 us | 12.4923 us | 448.039 us |  1.00 |      - |      - |      - |   2.96 KB |        1.00 |
|            InstanceDelegate | .NET Framework 4.7.2 | 100000 | 439.660 us |  3.5668 us | 438.691 us |  0.97 |      - |      - |      - |   2.96 KB |        1.00 |
|                    Embedded | .NET Framework 4.7.2 | 100000 |  52.141 us |  0.6307 us |  52.363 us |  0.12 | 0.4272 | 0.2441 |      - |   2.97 KB |        1.00 |
|   PreCompiledStaticDelegate | .NET Framework 4.7.2 | 100000 | 383.710 us |  2.8132 us | 383.185 us |  0.85 |      - |      - |      - |   1.25 KB |        0.42 |
| PreCompiledInstanceDelegate | .NET Framework 4.7.2 | 100000 | 390.786 us |  3.5416 us | 390.374 us |  0.86 |      - |      - |      - |   1.25 KB |        0.42 |
|         PreCompiledEmbedded | .NET Framework 4.7.2 | 100000 |  43.890 us |  0.8126 us |  44.012 us |  0.10 | 0.1831 | 0.0610 |      - |   1.27 KB |        0.43 |
