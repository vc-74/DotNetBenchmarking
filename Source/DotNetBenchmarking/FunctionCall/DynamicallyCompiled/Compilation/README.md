# Add delegate compilation
## Description
This benchmark compares different methods of building a static/instance delegates adding two integers dynamically, equivalent to:
```csharp
int a = 2;

for (int i = 0; i < loops; i++)
{
	int sum = a + i;
}
```

|                        Method |                                                                                                                    Description |
|------------------------------ |------------------------------------------------------------------------------------------------------------------------------- |
|               DynamicMethodStaticDelegate |                                               Creates a DynamicMethod, emits IL code and creates a static delegate |
|             DynamicMethodInstanceDelegate |                                           Creates a DynamicMethod, emits IL code and creates a non-static delegate |
|        DynamicTypeNewModuleStaticDelegate |                        Builds a new type in a new module, adds a static method to it and creates a static delegate |
|      DynamicTypeNewModuleInstanceDelegate |                     Builds a new type in a new module, adds a static method to it and creates an instance delegate |
|   DynamicTypeExistingModuleStaticDelegate |                  Builds a new type in an existing module, adds a static method to it and creates a static delegate |
| DynamicTypeExistingModuleInstanceDelegate |               Builds a new type in an existing module, adds a static method to it and creates an instance delegate |
|                       ExpressionTreeBuilt |                                                                   Builds an expression tree and creates a delegate |
|                  ExpressionTreeFromLambda |                                                       Gets an expression tree from a lambda and creates a delegate |
|                   LoopDynamicMethodStatic |    Creates a DynamicMethod, emits IL to create a loop executing a static delegate and creates an instance delegate |
|                 LoopDynamicMethodInstance | Creates a DynamicMethod, emits IL to create a loop executing an instance delegate and creates an instance delegate |
|                 LoopDynamicMethodEmbedded |      Creates a DynamicMethod, emits IL to create a loop implementing the addition and creates an instance delegate |

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
|                                    Method |              Runtime |         Mean |      StdDev | Ratio |   Gen0 |   Gen1 |   Gen2 | Allocated | Alloc Ratio |
|------------------------------------------ |--------------------- |-------------:|------------:|------:|-------:|-------:|-------:|----------:|------------:|
|               DynamicMethodStaticDelegate |             .NET 7.0 |     2.192 us |   0.1452 us |  1.00 | 0.0954 | 0.0916 |      - |   1.17 KB |        1.00 |
|             DynamicMethodInstanceDelegate |             .NET 7.0 |     2.223 us |   0.1259 us |  1.02 | 0.0992 | 0.0954 | 0.0038 |   1.19 KB |        1.01 |
|        DynamicTypeNewModuleStaticDelegate |             .NET 7.0 |   364.193 us | 107.3461 us |     ? | 0.3662 | 0.1221 |      - |   5.17 KB |           ? |
|      DynamicTypeNewModuleInstanceDelegate |             .NET 7.0 |   202.344 us |  41.4606 us |     ? | 0.3662 | 0.1221 |      - |   5.49 KB |           ? |
|   DynamicTypeExistingModuleStaticDelegate |             .NET 7.0 | 1,081.277 us | 215.7931 us |     ? |      - |      - |      - |   4.29 KB |           ? |
| DynamicTypeExistingModuleInstanceDelegate |             .NET 7.0 | 1,086.329 us | 256.6200 us |     ? |      - |      - |      - |   4.61 KB |           ? |
|                       ExpressionTreeBuilt |             .NET 7.0 |    16.839 us |   0.2495 us |  7.79 | 0.3662 | 0.3357 |      - |   4.67 KB |        3.99 |
|                  ExpressionTreeFromLambda |             .NET 7.0 |    15.494 us |   0.4311 us |  7.14 | 0.3815 | 0.3662 |      - |   4.73 KB |        4.04 |
|                   LoopDynamicMethodStatic |             .NET 7.0 |     2.533 us |   0.1236 us |  1.17 | 0.1297 | 0.1259 | 0.0038 |   1.58 KB |        1.35 |
|                 LoopDynamicMethodInstance |             .NET 7.0 |     2.662 us |   0.1430 us |  1.23 | 0.1297 | 0.1259 | 0.0038 |   1.58 KB |        1.35 |
|                 LoopDynamicMethodEmbedded |             .NET 7.0 |     2.497 us |   0.1446 us |  1.15 | 0.1221 | 0.1183 | 0.0114 |   1.48 KB |        1.26 |
|                                           |                      |              |             |       |        |        |        |           |             |
|               DynamicMethodStaticDelegate | .NET Framework 4.7.2 |     3.168 us |   0.0908 us |  1.00 | 0.1984 | 0.0992 | 0.0229 |   1.24 KB |        1.00 |
|             DynamicMethodInstanceDelegate | .NET Framework 4.7.2 |     3.273 us |   0.0562 us |  1.03 | 0.2022 | 0.0992 | 0.0229 |   1.25 KB |        1.01 |
|        DynamicTypeNewModuleStaticDelegate | .NET Framework 4.7.2 |   205.954 us |  38.4390 us |     ? | 0.9766 | 0.2441 |      - |   6.31 KB |           ? |
|      DynamicTypeNewModuleInstanceDelegate | .NET Framework 4.7.2 |   226.222 us |  42.4134 us |     ? | 0.9766 | 0.2441 |      - |   6.54 KB |           ? |
|   DynamicTypeExistingModuleStaticDelegate | .NET Framework 4.7.2 | 1,049.783 us | 257.2515 us |     ? | 0.4883 |      - |      - |   4.83 KB |           ? |
| DynamicTypeExistingModuleInstanceDelegate | .NET Framework 4.7.2 | 1,020.321 us | 237.4216 us |     ? | 0.4883 |      - |      - |   5.07 KB |           ? |
|                       ExpressionTreeBuilt | .NET Framework 4.7.2 |    15.208 us |   0.0825 us |  4.81 | 0.8240 | 0.3967 | 0.0305 |   5.19 KB |        4.19 |
|                  ExpressionTreeFromLambda | .NET Framework 4.7.2 |    14.062 us |   0.0985 us |  4.44 | 0.7935 | 0.3967 | 0.0305 |   4.92 KB |        3.98 |
|                   LoopDynamicMethodStatic | .NET Framework 4.7.2 |     3.690 us |   0.0986 us |  1.17 | 0.2785 | 0.1411 | 0.0305 |   1.73 KB |        1.40 |
|                 LoopDynamicMethodInstance | .NET Framework 4.7.2 |     3.855 us |   0.0642 us |  1.22 | 0.2785 | 0.1411 | 0.0305 |   1.73 KB |        1.40 |
|                 LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |     3.367 us |   0.0848 us |  1.06 | 0.2594 | 0.1297 | 0.0305 |   1.61 KB |        1.30 |

## Conclusions:
- DynamicMethod compilation is much faster (up to ~500*) than creating a new type and adding a method to it
- Creating a new module before creating a type is faster (~5*) than reusing an existing module
- As expected, generating IL is faster (~7*) than creating an expression tree (which is then compiled to IL)
- Compilation is faster on .NET 7.0

