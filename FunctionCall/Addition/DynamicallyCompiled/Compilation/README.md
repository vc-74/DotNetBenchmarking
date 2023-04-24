# Add delegate compilation
## Description
This benchmark compares different methods of building a static/instance delegate adding two integers dynamically:
- Generating IL using a DynamicMethod
- Generating IL using a MethodBuilder in a dynamically created type
- Building an expression tree and compiling it

## Results
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.203
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-VSDQFM           : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-QRLTAO           : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256


|                          Method |              Runtime |         Mean |      StdDev | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|-------------------------------- |--------------------- |-------------:|------------:|------:|-------:|-------:|-------:|----------:|------------:|
|            DynamicTypeNewModule |             .NET 7.0 |   363.084 us | 136.6018 us |     ? | 0.3662 | 0.1221 |      - |   5.52 KB |           ? |
|       DynamicTypeExistingModule |             .NET 7.0 | 2,072.341 us | 866.2891 us |     ? |      - |      - |      - |   4.61 KB |           ? |
|                                 |                      |              |             |       |        |        |        |           |             |
|            DynamicTypeNewModule | .NET Framework 4.7.2 |   379.851 us | 131.9704 us |     ? | 0.9766 | 0.2441 |      - |   6.54 KB |           ? |
|       DynamicTypeExistingModule | .NET Framework 4.7.2 | 1,980.059 us | 840.9766 us |     ? | 0.4883 |      - |      - |   5.13 KB |           ? |
|                                 |                      |              |             |       |        |        |        |           |             |
|   DynamicMethodILStaticDelegate |             .NET 7.0 |     2.562 us |   0.1317 us |  1.01 | 0.1030 | 0.0992 | 0.0076 |   1.17 KB |        0.99 |
| DynamicMethodILInstanceDelegate |             .NET 7.0 |     2.531 us |   0.1345 us |  1.00 | 0.0954 | 0.0916 | 0.0153 |   1.19 KB |        1.00 |
|                  ExpressionTree |             .NET 7.0 |    20.769 us |   0.4936 us |  8.21 | 0.3662 | 0.3357 |      - |   4.67 KB |        3.94 |
|                                 |                      |              |             |       |        |        |        |           |             |
|   DynamicMethodILStaticDelegate | .NET Framework 4.7.2 |     3.665 us |   0.1795 us |  1.15 | 0.1984 | 0.0992 | 0.0229 |   1.24 KB |        0.97 |
| DynamicMethodILInstanceDelegate | .NET Framework 4.7.2 |     3.183 us |   0.1753 us |  1.00 | 0.2060 | 0.1030 | 0.0229 |   1.27 KB |        1.00 |
|                  ExpressionTree | .NET Framework 4.7.2 |    16.836 us |   0.2475 us |  5.26 | 0.8240 | 0.3967 | 0.0305 |   5.19 KB |        4.09 |

## Conclusions:
- DynamicMethod compilation is much faster (~150 to 1000*) than creating a new type and adding a method to it
- Creating a new module before creating a type is faster (~6*) than reusing an existing module
- As expected, generating IL is faster (~10*) than creating an expression tree (which is then compiled to IL)
- Compilation is faster on core except for expression trees
