# Add delegate compilation
## Description
This benchmark compares different methods of building a static/instance delegates adding two integers dynamically, equivalent to:
```csharp
int sum = 1 + 1;
```

|                        Method |                                                                     Description |
|------------------------------ |-------------------------------------------------------------------------------- |
|   DynamicMethodStaticDelegate |            Creates a DynamicMethod, emits IL code and creates a static delegate |
| DynamicMethodInstanceDelegate |        Creates a DynamicMethod, emits IL code and creates a non-static delegate |
|                ExpressionTree |                                Builds an expression tree and creates a delegate |
|          DynamicTypeNewModule |                Builds a new type in a new module and adds a static method to it |
|     DynamicTypeExistingModule |      Builds a new new type in an existing module and adds a static method to it |

These tests only compile delegates, they don't execute them.

The benchmarks building a new type (DynamicTypeNewModule and DynamicTypeExistingModule) cannot be automatically executed since creating a new type gets slower and slower. 
This is less true if a module is created for each type but the differences with DynamicMethod is still very important.

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
|                        Method |              Runtime |         Mean |      StdDev | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|------------------------------ |--------------------- |-------------:|------------:|------:|-------:|-------:|-------:|----------:|------------:|
|          DynamicTypeNewModule |             .NET 7.0 |   196.888 us |  39.8090 us |     ? | 0.3662 | 0.1221 |      - |   5.49 KB |           ? |
|     DynamicTypeExistingModule |             .NET 7.0 | 1,091.877 us | 207.3289 us |     ? |      - |      - |      - |   4.61 KB |           ? |
|   DynamicMethodStaticDelegate |             .NET 7.0 |     2.212 us |   0.1467 us |  1.00 | 0.0992 | 0.0954 | 0.0038 |   1.17 KB |        1.00 |
| DynamicMethodInstanceDelegate |             .NET 7.0 |     2.242 us |   0.1440 us |  1.02 | 0.1068 | 0.1030 | 0.0153 |   1.19 KB |        1.01 |
|           ExpressionTreeBuilt |             .NET 7.0 |    19.611 us |   0.4810 us |  8.76 | 0.3662 | 0.3357 |      - |   4.67 KB |        3.99 |
|      ExpressionTreeFromLambda |             .NET 7.0 |    15.228 us |   0.3990 us |  6.81 | 0.3662 | 0.3357 |      - |   4.73 KB |        4.04 |
|                               |                      |              |             |       |        |        |        |           |             |
|          DynamicTypeNewModule | .NET Framework 4.7.2 |   225.657 us |  38.1239 us |     ? | 0.9766 | 0.2441 |      - |   6.54 KB |           ? |
|     DynamicTypeExistingModule | .NET Framework 4.7.2 | 1,003.947 us | 229.4997 us |     ? | 0.4883 |      - |      - |   5.07 KB |           ? |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |     3.357 us |   0.1628 us |  1.00 | 0.1984 | 0.0992 | 0.0229 |   1.24 KB |        1.00 |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |     2.773 us |   0.1044 us |  0.83 | 0.2060 | 0.1030 | 0.0229 |   1.27 KB |        1.03 |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |    16.199 us |   0.2466 us |  4.81 | 0.8240 | 0.3967 | 0.0305 |   5.19 KB |        4.19 |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |    15.035 us |   0.2790 us |  4.49 | 0.7935 | 0.3967 | 0.0305 |   4.92 KB |        3.98 |


|                        Method |              Runtime |         Mean |      StdDev |       Median | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|------------------------------ |--------------------- |-------------:|------------:|-------------:|------:|-------:|-------:|-------:|----------:|------------:|
|          DynamicTypeNewModule |             .NET 7.0 |   202.446 us |  40.2311 us |   202.933 us |     ? | 0.3662 | 0.1221 |      - |   5.49 KB |           ? |
|     DynamicTypeExistingModule |             .NET 7.0 | 1,073.113 us | 247.0656 us | 1,052.613 us |     ? |      - |      - |      - |   4.61 KB |           ? |
|   DynamicMethodStaticDelegate |             .NET 7.0 |     1.959 us |   0.0807 us |     1.947 us |  1.00 | 0.0992 | 0.0954 | 0.0038 |   1.17 KB |        1.00 |
| DynamicMethodInstanceDelegate |             .NET 7.0 |     2.170 us |   0.1063 us |     2.173 us |  1.11 | 0.0973 | 0.0954 | 0.0019 |   1.19 KB |        1.01 |
|           ExpressionTreeBuilt |             .NET 7.0 |    16.615 us |   0.2817 us |    16.642 us |  8.47 | 0.3662 | 0.3357 |      - |   4.67 KB |        3.99 |
|      ExpressionTreeFromLambda |             .NET 7.0 |    16.193 us |   0.3446 us |    16.186 us |  8.24 | 0.3662 | 0.3357 |      - |   4.73 KB |        4.04 |
|                               |                      |              |             |              |       |        |        |        |           |             |
|          DynamicTypeNewModule | .NET Framework 4.7.2 |   225.096 us |  38.1231 us |   224.368 us |     ? | 0.9766 | 0.2441 |      - |   6.54 KB |           ? |
|     DynamicTypeExistingModule | .NET Framework 4.7.2 | 1,004.969 us | 246.2931 us |   989.722 us |     ? | 0.4883 |      - |      - |   5.07 KB |           ? |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |     3.011 us |   0.0553 us |     3.012 us |  1.00 | 0.1984 | 0.0992 | 0.0229 |   1.24 KB |        1.00 |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |     4.687 us |   1.2404 us |     5.628 us |  1.06 | 0.2022 | 0.0992 | 0.0229 |   1.25 KB |        1.01 |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |    22.503 us |  12.0601 us |    15.169 us | 10.53 | 0.8392 | 0.4120 | 0.0305 |   5.19 KB |        4.19 |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |    13.864 us |   0.0529 us |    13.855 us |  4.60 | 0.7935 | 0.3967 | 0.0305 |   4.92 KB |        3.98 |

## Conclusions:
- DynamicMethod compilation is much faster (up to ~1000*) than creating a new type and adding a method to it
- Creating a new module before creating a type is faster (~6*) than reusing an existing module
- As expected, generating IL is faster (~9*) than creating an expression tree (which is then compiled to IL)
- Compilation is faster on .NET 7.0 except for built expression trees
