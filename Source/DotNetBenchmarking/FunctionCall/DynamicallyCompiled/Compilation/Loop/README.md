# Dynamically built loop function compilation

## Description
This benchmark compares different methods of building a static/instance delegates implementing a loop adding two integers dynamically, equivalent to:
```csharp
int a = 2;

for (int i = 0; i < loops; i++)
{
	int sum = a + i;
}
```

|                     Method |                                                                                        Description |
|--------------------------- |--------------------------------------------------------------------------------------------------- |
| LoopDynamicMethodStatic    | Creates a DynamicMethod, emits IL invoking a static delegate code and creates an instance delegate |
| LoopDynamicMethodInstance  |   Creates a DynamicMethod, emits IL invoking an instance delegate and creates an instance delegate |
| LoopDynamicMethodEmbedded  |          Creates a DynamicMethod, emits IL embedding the addition and creates an instance delegate |
| LoopExpressionTreeStatic   |        Creates an expression tree invoking a static delegate code and creates an instance delegate |
| LoopExpressionTreeInstance |     Creates an expression tree invoking an instance delegate code and creates an instance delegate |
| LoopExpressionTreeEmbedded |             Creates an expression tree embedding the addition and and creates an instance delegate |

These tests only compile delegates, they don't execute them.

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
|                     Method |              Runtime |       Mean |     StdDev |     Median |   Gen0 |   Gen1 |   Gen2 | Allocated |
|--------------------------- |--------------------- |-----------:|-----------:|-----------:|-------:|-------:|-------:|----------:|
|    LoopDynamicMethodStatic |             .NET 7.0 |   3.634 us |  0.1482 us |   3.627 us | 0.1297 | 0.1259 | 0.0038 |   1.58 KB |
|  LoopDynamicMethodInstance |             .NET 7.0 |   3.553 us |  0.0422 us |   3.564 us | 0.1259 | 0.1221 |      - |   1.58 KB |
|  LoopDynamicMethodEmbedded |             .NET 7.0 |   3.269 us |  0.1659 us |   3.268 us | 0.1221 | 0.1183 | 0.0038 |   1.48 KB |
|   LoopExpressionTreeStatic |             .NET 7.0 |  94.161 us | 15.8950 us |  98.661 us | 0.9766 | 0.8545 |      - |  12.88 KB |
| LoopExpressionTreeInstance |             .NET 7.0 |  99.114 us | 20.0566 us | 102.540 us | 0.9766 | 0.8545 |      - |  12.88 KB |
| LoopExpressionTreeEmbedded |             .NET 7.0 |  30.179 us |  0.3061 us |  30.114 us | 0.8545 | 0.7324 |      - |  11.63 KB |
|    LoopDynamicMethodStatic | .NET Framework 4.7.2 |   3.750 us |  0.1653 us |   3.731 us | 0.2747 | 0.1373 | 0.0305 |   1.73 KB |
|  LoopDynamicMethodInstance | .NET Framework 4.7.2 |   3.714 us |  0.1547 us |   3.693 us | 0.2747 | 0.1373 | 0.0229 |   1.73 KB |
|  LoopDynamicMethodEmbedded | .NET Framework 4.7.2 |   3.349 us |  0.0441 us |   3.356 us | 0.2594 | 0.1297 | 0.0305 |   1.61 KB |
|   LoopExpressionTreeStatic | .NET Framework 4.7.2 | 115.409 us |  0.2935 us | 115.364 us | 2.3193 | 1.0986 |      - |  14.94 KB |
| LoopExpressionTreeInstance | .NET Framework 4.7.2 | 116.863 us |  1.1276 us | 116.775 us | 2.3193 | 1.0986 |      - |  14.94 KB |
| LoopExpressionTreeEmbedded | .NET Framework 4.7.2 |  51.492 us |  0.3653 us |  51.394 us | 2.1362 | 1.0376 |      - |  13.15 KB |

## Conclusions:
- Compilation of implementations embedding the addition seems slightly faster
- As expected, generating IL is faster (~30*) than creating an expression tree (which is then compiled to IL)
- Compilation is slightly faster on .NET 7.0
