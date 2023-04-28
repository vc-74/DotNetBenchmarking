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
|                        Method |              Runtime |       Mean |      StdDev | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|------------------------------ |--------------------- |-----------:|------------:|------:|-------:|-------:|-------:|----------:|------------:|
|          DynamicTypeNewModule |             .NET 7.0 | 167.712 us |  35.1706 us |     ? | 0.3662 | 0.1221 |      - |   5.49 KB |           ? |
|     DynamicTypeExistingModule |             .NET 7.0 | 959.181 us | 204.1626 us |     ? |      - |      - |      - |   4.61 KB |           ? |
|   DynamicMethodStaticDelegate |             .NET 7.0 |   1.962 us |   0.1074 us |  1.00 | 0.0992 | 0.0973 | 0.0095 |   1.17 KB |        1.00 |
| DynamicMethodInstanceDelegate |             .NET 7.0 |   1.902 us |   0.0968 us |  0.97 | 0.0973 | 0.0954 | 0.0038 |   1.19 KB |        1.01 |
|           ExpressionTreeBuilt |             .NET 7.0 |  14.676 us |   0.2050 us |  7.54 | 0.3662 | 0.3510 |      - |   4.67 KB |        3.99 |
|      ExpressionTreeFromLambda |             .NET 7.0 |  13.132 us |   0.1683 us |  6.75 | 0.3815 | 0.3510 |      - |   4.73 KB |        4.04 |
|       LoopDynamicMethodStatic |             .NET 7.0 |   2.305 us |   0.0975 us |  1.18 | 0.1335 | 0.1297 |      - |   1.66 KB |        1.41 |
|     LoopDynamicMethodInstance |             .NET 7.0 |   2.402 us |   0.1589 us |  1.23 | 0.1335 | 0.1297 | 0.0153 |   1.66 KB |        1.41 |
|     LoopDynamicMethodEmbedded |             .NET 7.0 |   2.351 us |   0.1874 us |  1.20 | 0.1259 | 0.1221 | 0.0076 |   1.55 KB |        1.32 |
|                               |                      |            |             |       |        |        |        |           |             |
|          DynamicTypeNewModule | .NET Framework 4.7.2 | 184.761 us |  31.7654 us |     ? | 0.9766 | 0.2441 |      - |   6.54 KB |           ? |
|     DynamicTypeExistingModule | .NET Framework 4.7.2 | 883.395 us | 198.1094 us |     ? | 0.4883 |      - |      - |   5.06 KB |           ? |
|   DynamicMethodStaticDelegate | .NET Framework 4.7.2 |   2.771 us |   0.1310 us |  1.00 | 0.1984 | 0.0992 | 0.0229 |   1.24 KB |        1.00 |
| DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |   2.934 us |   0.1578 us |  1.07 | 0.2022 | 0.0992 | 0.0229 |   1.25 KB |        1.01 |
|           ExpressionTreeBuilt | .NET Framework 4.7.2 |  12.586 us |   0.1645 us |  4.64 | 0.8392 | 0.4120 | 0.0305 |   5.19 KB |        4.19 |
|      ExpressionTreeFromLambda | .NET Framework 4.7.2 |  11.762 us |   0.1745 us |  4.34 | 0.7935 | 0.3967 | 0.0305 |   4.92 KB |        3.98 |
|       LoopDynamicMethodStatic | .NET Framework 4.7.2 |   3.572 us |   0.1326 us |  1.29 | 0.2937 | 0.1450 | 0.0267 |   1.82 KB |        1.47 |
|     LoopDynamicMethodInstance | .NET Framework 4.7.2 |   3.637 us |   0.1544 us |  1.32 | 0.2937 | 0.1450 | 0.0267 |   1.82 KB |        1.47 |
|     LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |   3.272 us |   0.1463 us |  1.19 | 0.2708 | 0.1335 | 0.0305 |   1.69 KB |        1.36 |

## Conclusions:
- DynamicMethod compilation is much faster (up to ~1000*) than creating a new type and adding a method to it
- Creating a new module before creating a type is faster (~6*) than reusing an existing module
- As expected, generating IL is faster (~9*) than creating an expression tree (which is then compiled to IL)
- Compilation is faster on .NET 7.0 except for built expression trees
