# Statically compiled Add function execution

## Description
This benchmark compares different methods of executing statically compiled code calculating the sum of two integers, equivalent to:
```csharp
int a = 2;

for (int i = 0; i < loops; i++)
{
	int sum = a + i;
}
```

|                       Method |                                                                     Description |
|----------------------------- |-------------------------------------------------------------------------------- |
|                   NoFunction |           Baseline implementation without using a function (same code as above) |
|                 StaticMethod |                                         Addition implemented as a static method |
|               InstanceMethod |                                      Addition implemented as an instance method |
|                VirtualMethod |                               Addition implemented as a virtual instance method |
|          StaticLocalFunction |                                   Addition implemented as a local static method |
|        InstanceLocalFunction |                               Addition implemented as a local non-static method |
| InstanceLocalFunctionCapture |               Addition implemented as a local method capturing a local variable |
|                       Lambda |                               Addition implemented as a local lambda expression |
|                LambdaCapture |    Addition implemented as a local lambda expression capturing a local variable |
|         DelegateStaticMethod |                                       Addition implemented as a static delegate |
|       DelegateInstanceMethod |            Addition implemented as an instance delegate on a non-virtual method |
|        DelegateVirtualMethod |                Addition implemented as an instance delegate on a virtual method |

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
|                       Method |              Runtime | Loops |       Mean |   StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |--------------------- |------ |-----------:|---------:|------:|-------:|----------:|------------:|
|                   NoFunction |             .NET 7.0 |  1000 |   265.2 ns |  0.19 ns |  1.00 |      - |         - |          NA |
|                 StaticMethod |             .NET 7.0 |  1000 |   265.2 ns |  0.13 ns |  1.00 |      - |         - |          NA |
|               InstanceMethod |             .NET 7.0 |  1000 |   265.3 ns |  0.11 ns |  1.00 |      - |         - |          NA |
|                VirtualMethod |             .NET 7.0 |  1000 | 1,034.8 ns |  0.48 ns |  3.90 |      - |         - |          NA |
|          StaticLocalFunction |             .NET 7.0 |  1000 |   265.2 ns |  0.07 ns |  1.00 |      - |         - |          NA |
|        InstanceLocalFunction |             .NET 7.0 |  1000 |   265.5 ns |  0.33 ns |  1.00 |      - |         - |          NA |
| InstanceLocalFunctionCapture |             .NET 7.0 |  1000 |   265.1 ns |  0.25 ns |  1.00 |      - |         - |          NA |
|                       Lambda |             .NET 7.0 |  1000 | 1,552.2 ns |  1.15 ns |  5.85 |      - |         - |          NA |
|                LambdaCapture |             .NET 7.0 |  1000 | 1,562.1 ns |  1.37 ns |  5.89 | 0.0057 |      88 B |          NA |
|         DelegateStaticMethod |             .NET 7.0 |  1000 | 2,070.2 ns |  2.03 ns |  7.81 |      - |         - |          NA |
|       DelegateInstanceMethod |             .NET 7.0 |  1000 | 1,559.1 ns |  1.13 ns |  5.88 | 0.0038 |      64 B |          NA |
|        DelegateVirtualMethod |             .NET 7.0 |  1000 | 1,565.1 ns |  2.01 ns |  5.90 | 0.0038 |      64 B |          NA |
|                              |                      |       |            |          |       |        |           |             |
|                   NoFunction | .NET Framework 4.7.2 |  1000 |   265.7 ns |  0.27 ns |  1.00 |      - |         - |          NA |
|                 StaticMethod | .NET Framework 4.7.2 |  1000 |   266.1 ns |  0.34 ns |  1.00 |      - |         - |          NA |
|               InstanceMethod | .NET Framework 4.7.2 |  1000 |   265.8 ns |  0.47 ns |  1.00 |      - |         - |          NA |
|                VirtualMethod | .NET Framework 4.7.2 |  1000 | 1,039.4 ns |  0.82 ns |  3.91 |      - |         - |          NA |
|          StaticLocalFunction | .NET Framework 4.7.2 |  1000 |   265.6 ns |  0.36 ns |  1.00 |      - |         - |          NA |
|        InstanceLocalFunction | .NET Framework 4.7.2 |  1000 |   265.4 ns |  0.10 ns |  1.00 |      - |         - |          NA |
| InstanceLocalFunctionCapture | .NET Framework 4.7.2 |  1000 |   265.8 ns |  0.18 ns |  1.00 |      - |         - |          NA |
|                       Lambda | .NET Framework 4.7.2 |  1000 | 1,469.4 ns | 32.62 ns |  5.50 |      - |         - |          NA |
|                LambdaCapture | .NET Framework 4.7.2 |  1000 | 1,303.4 ns |  2.20 ns |  4.90 | 0.0134 |      88 B |          NA |
|         DelegateStaticMethod | .NET Framework 4.7.2 |  1000 | 2,327.7 ns | 13.73 ns |  8.76 |      - |         - |          NA |
|       DelegateInstanceMethod | .NET Framework 4.7.2 |  1000 | 1,299.4 ns |  0.52 ns |  4.89 | 0.0095 |      64 B |          NA |
|        DelegateVirtualMethod | .NET Framework 4.7.2 |  1000 | 1,303.8 ns |  0.78 ns |  4.91 | 0.0095 |      64 B |          NA |

## Conclusions:
### .NET 7.0 and .NET Framework 4.7.2
- No difference between NoFunction, StaticMethod, InstanceMethod, StaticLocalFunction or InstanceLocalFunction and InstanceLocalFunctionCapture: 
  all these implementations are inlined by the JIT
- VirtualMethod is slower (~*5/~*5) because it is not inlined
- Lambda, LambdaCapture, DelegateStaticMethod, DelegateInstanceMethod and DelegateVirtualMethod are slower because they are based on instance delegates which cannot be inlined
- Lambdas have similar performance as the instance delegates but do not allocate, this is due to the fact the compiler creates a class with a Func field and instantiates it during the first invocation (like for static delegates but with a target)
- DelegateStaticMethod is based on static delegates and is slower (~*1.4/~*1.9) than instance delegates
- Lambda and DelegateStaticMethod are based on static delegates and allocate a delegate only during the first benchmark operation
- LambdaCapture, DelegateInstanceMethod and DelegateVirtualMethod are based on instance delegates and allocate during each benchmark operation

### .NET 7.0 vs .NET Framework 4.7.2
- Instance delegates (including lambdas) execute faster on .NET Framework 4.7.2
