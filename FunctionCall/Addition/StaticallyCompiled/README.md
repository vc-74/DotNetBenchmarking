# Statically compiled Add function execution

## Description
This benchmark compares different methods of executing statically compiled code calculating the sum of two integers, equivalent to:
```csharp
int loops = Loops;
for (int a = 0; a < loops; a++)
{
    const int b = 2;
    int c = a + b;
}
```
|                       Method |                                                                    Description |
|----------------------------- |------------------------------------------------------------------------------- |
|                   NoFunction |             Addition implemented without using a function (same code as above) |
|                 StaticMethod |                                        Addition implemented as a static method |
|               InstanceMethod |                                     Addition implemented as an instance method |
|          StaticLocalFunction |                                  Addition implemented as a local static method |
|        InstanceLocalFunction |                              Addition implemented as a local non-static method |
| InstanceLocalFunctionCapture |              Addition implemented as a local method capturing a local variable |
|                       Lambda |                              Addition implemented as a local lambda expression |
|                LambdaCapture |   Addition implemented as a local lambda expression capturing a local variable |
|         DelegateStaticMethod |              Addition implemented as a delegate to a static method (no target) |
|       DelegateInstanceMethod |          Addition implemented as a delegate to a static method (with a target) |

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
|                       Method |              Runtime | Loops |      Mean |    StdDev | Ratio | Allocated | Alloc Ratio |
|----------------------------- |--------------------- |------ |----------:|----------:|------:|----------:|------------:|
|                   NoFunction |             .NET 7.0 | 10000 |  2.194 us | 0.0029 us |  1.00 |         - |          NA |
|                 StaticMethod |             .NET 7.0 | 10000 |  2.201 us | 0.0147 us |  1.00 |         - |          NA |
|               InstanceMethod |             .NET 7.0 | 10000 |  2.192 us | 0.0030 us |  1.00 |         - |          NA |
|          StaticLocalFunction |             .NET 7.0 | 10000 |  2.191 us | 0.0035 us |  1.00 |         - |          NA |
|        InstanceLocalFunction |             .NET 7.0 | 10000 |  2.193 us | 0.0041 us |  1.00 |         - |          NA |
| InstanceLocalFunctionCapture |             .NET 7.0 | 10000 |  2.191 us | 0.0027 us |  1.00 |         - |          NA |
|                       Lambda |             .NET 7.0 | 10000 | 15.598 us | 0.1847 us |  7.11 |         - |          NA |
|                LambdaCapture |             .NET 7.0 | 10000 | 13.518 us | 0.1165 us |  6.16 |      64 B |          NA |
|         DelegateStaticMethod |             .NET 7.0 | 10000 | 18.016 us | 0.1761 us |  8.22 |         - |          NA |
|       DelegateInstanceMethod |             .NET 7.0 | 10000 | 13.369 us | 0.0783 us |  6.10 |      64 B |          NA |
|                   NoFunction | .NET Framework 4.7.2 | 10000 |  2.193 us | 0.0053 us |  1.00 |         - |          NA |
|                 StaticMethod | .NET Framework 4.7.2 | 10000 |  2.193 us | 0.0051 us |  1.00 |         - |          NA |
|               InstanceMethod | .NET Framework 4.7.2 | 10000 |  2.192 us | 0.0032 us |  1.00 |         - |          NA |
|          StaticLocalFunction | .NET Framework 4.7.2 | 10000 |  2.216 us | 0.0402 us |  1.01 |         - |          NA |
|        InstanceLocalFunction | .NET Framework 4.7.2 | 10000 |  2.196 us | 0.0058 us |  1.00 |         - |          NA |
| InstanceLocalFunctionCapture | .NET Framework 4.7.2 | 10000 |  2.194 us | 0.0026 us |  1.00 |         - |          NA |
|                       Lambda | .NET Framework 4.7.2 | 10000 | 12.526 us | 0.2414 us |  5.74 |         - |          NA |
|                LambdaCapture | .NET Framework 4.7.2 | 10000 | 11.343 us | 0.1038 us |  5.18 |      64 B |          NA |
|         DelegateStaticMethod | .NET Framework 4.7.2 | 10000 | 19.428 us | 0.2500 us |  8.85 |         - |          NA |
|       DelegateInstanceMethod | .NET Framework 4.7.2 | 10000 | 11.371 us | 0.1170 us |  5.19 |      64 B |          NA |

## Conclusions:
- No difference between no function, static method, instance method, static local function or instance local function with or without capture
- No difference on these cases between core and framework
- Lambdas are slower (*7 on core, *6 on framework) since they have to be converted to an expression tree before being compiled to IL
- Instance delegates (including lambdas) are faster than static delegates ([as expected](https://stackoverflow.com/a/42187448/446279))
- Instance delegates (including lambdas) are slower on core, static delegates are slower on framework
- Instance delegates (including lambdas) allocate
