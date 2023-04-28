# Statically compiled Add function execution

## Description
This benchmark compares different methods of executing statically compiled code calculating the sum of two integers, equivalent to:
```csharp
for (int i = 0; i < loops; i++)
{
    int sum = 1 + 1;
}
```

The implementations are executed `loops` times:
- to avoid getting tiny durations considered by Benchmark.net are close to zero
- to minimize the penalty of creating delegates and focus on execution

In this document:
Static delegate=delegate instance on a static method (no target).
Instance delegate=delegate instance on an instance method (with a target).

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
|                       Method |              Runtime | Loops |       Mean |    StdDev |     Median | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |--------------------- |------ |-----------:|----------:|-----------:|------:|-------:|----------:|------------:|
|                   NoFunction |             .NET 7.0 |  1000 |   266.7 ns |   0.62 ns |   266.5 ns |  1.00 |      - |         - |          NA |
|                 StaticMethod |             .NET 7.0 |  1000 |   266.1 ns |   0.24 ns |   266.0 ns |  1.00 |      - |         - |          NA |
|               InstanceMethod |             .NET 7.0 |  1000 |   266.8 ns |   0.68 ns |   266.8 ns |  1.00 |      - |         - |          NA |
|                VirtualMethod |             .NET 7.0 |  1000 | 1,298.4 ns |   4.94 ns | 1,299.5 ns |  4.87 |      - |         - |          NA |
|          StaticLocalFunction |             .NET 7.0 |  1000 |   266.5 ns |   0.54 ns |   266.2 ns |  1.00 |      - |         - |          NA |
|        InstanceLocalFunction |             .NET 7.0 |  1000 |   266.3 ns |   0.39 ns |   266.4 ns |  1.00 |      - |         - |          NA |
| InstanceLocalFunctionCapture |             .NET 7.0 |  1000 |   266.2 ns |   0.12 ns |   266.2 ns |  1.00 |      - |         - |          NA |
|                       Lambda |             .NET 7.0 |  1000 | 1,558.8 ns |   1.71 ns | 1,558.8 ns |  5.85 |      - |         - |          NA |
|                LambdaCapture |             .NET 7.0 |  1000 | 1,563.5 ns |   2.74 ns | 1,562.4 ns |  5.86 | 0.0038 |      64 B |          NA |
|         DelegateStaticMethod |             .NET 7.0 |  1000 | 2,077.8 ns |   1.92 ns | 2,078.0 ns |  7.79 |      - |         - |          NA |
|       DelegateInstanceMethod |             .NET 7.0 |  1000 | 1,561.5 ns |   1.81 ns | 1,561.2 ns |  5.86 | 0.0038 |      64 B |          NA |
|        DelegateVirtualMethod |             .NET 7.0 |  1000 | 1,566.2 ns |   3.08 ns | 1,565.6 ns |  5.87 | 0.0038 |      64 B |          NA |
|                              |                      |       |            |           |            |       |        |           |             |
|                   NoFunction | .NET Framework 4.7.2 |  1000 |   267.0 ns |   0.48 ns |   266.9 ns |  1.00 |      - |         - |          NA |
|                 StaticMethod | .NET Framework 4.7.2 |  1000 |   266.5 ns |   0.38 ns |   266.6 ns |  1.00 |      - |         - |          NA |
|               InstanceMethod | .NET Framework 4.7.2 |  1000 |   266.7 ns |   0.42 ns |   266.6 ns |  1.00 |      - |         - |          NA |
|                VirtualMethod | .NET Framework 4.7.2 |  1000 | 1,299.9 ns |   1.32 ns | 1,300.0 ns |  4.87 |      - |         - |          NA |
|          StaticLocalFunction | .NET Framework 4.7.2 |  1000 |   266.5 ns |   0.29 ns |   266.5 ns |  1.00 |      - |         - |          NA |
|        InstanceLocalFunction | .NET Framework 4.7.2 |  1000 |   266.5 ns |   0.37 ns |   266.5 ns |  1.00 |      - |         - |          NA |
| InstanceLocalFunctionCapture | .NET Framework 4.7.2 |  1000 |   267.4 ns |   0.75 ns |   267.3 ns |  1.00 |      - |         - |          NA |
|                       Lambda | .NET Framework 4.7.2 |  1000 | 1,422.7 ns | 150.95 ns | 1,302.5 ns |  5.60 |      - |         - |          NA |
|                LambdaCapture | .NET Framework 4.7.2 |  1000 | 1,590.5 ns |  54.55 ns | 1,602.5 ns |  5.88 | 0.0095 |      64 B |          NA |
|         DelegateStaticMethod | .NET Framework 4.7.2 |  1000 | 2,891.6 ns | 140.78 ns | 2,917.5 ns | 10.43 |      - |         - |          NA |
|       DelegateInstanceMethod | .NET Framework 4.7.2 |  1000 | 1,590.9 ns |  54.97 ns | 1,602.8 ns |  5.88 | 0.0095 |      64 B |          NA |
|        DelegateVirtualMethod | .NET Framework 4.7.2 |  1000 | 1,615.6 ns |   6.69 ns | 1,613.0 ns |  6.05 | 0.0095 |      64 B |          NA |

## Conclusions:
### .NET 7.0
- No difference between NoFunction, StaticMethod, InstanceMethod, StaticLocalFunction or InstanceLocalFunction and InstanceLocalFunctionCapture: 
  all these implementations are inlined by the JIT
- VirtualMethod is slower (~*5) because it is not inlined
- Lambda, LambdaCapture, DelegateStaticMethod, DelegateInstanceMethod and DelegateVirtualMethod are slower because they are based on instance delegates which invocations cannot be inlined
- Lambdas have similar performance as the instance delegates but do not allocate, this is due to the fact the compiler creates a class with a Func field and instantiates it during the first invocation (like for static delegates but with a target)
- DelegateStaticMethod is based on static delegates and is slower (~*1.4) than instance delegates
- Lambda and DelegateStaticMethod are based on static static and allocate a delegate only during the first invocation ((8 * 8B) / 1_000 -> -)
- LambdaCapture, DelegateInstanceMethod and DelegateVirtualMethod are based on instance delegates and allocate a delegate for each invocation ((8 * 8B) -> 64B)

### .NET Framework 4.7.2
- No difference between NoFunction, StaticMethod, InstanceMethod, StaticLocalFunction or InstanceLocalFunction and InstanceLocalFunctionCapture: 
  all these implementations are inlined by the JIT
- VirtualMethod is slower (~*5) because it is not inlined
- Lambda, LambdaCapture, DelegateStaticMethod, DelegateInstanceMethod and DelegateVirtualMethod are slower because they are based on instance delegates which invocations cannot be inlined
- Lambdas have similar performance as the instance delegates but do not allocate, this is due to the fact the compiler creates a class with a Func field and instantiates it during the first invocation (like for static delegates but with a target)
- No difference between Lambda and LambdaCapture, they seem to be both based on instance delegates
- DelegateStaticMethod is based on static delegates and is slower (~*1.9) than instance delegates
- Lambda and DelegateStaticMethod are based on static delegates and allocate a delegate only during the first invocation ((8 * 8B) / 1_000 -> -)
- LambdaCapture, DelegateInstanceMethod and DelegateVirtualMethod are based on instance delegates and allocate a delegate for each invocation ((8 * 8B) -> 64B)

### .NET 7.0 vs .NET Framework 4.7.2
- No differences for inlined functions and VirtualMethod
- .NET Framework 4.7.2 seems to always create instance delegates for lambdas although Lambda does not allocate whereas .NET 7.0 seems to create static delegates for this case
- DelegateInstanceMethod and DelegateVirtualMethod are identical on .NET 7.0 but DelegateInstanceMethod < DelegateVirtualMethod on .NET Framework 4.7.2
- delegates invocations are faster on .NET Framework 4.7.2 (~*1.1)

### Delegates performance
Static delegates execution require some arguments reshuffling ([details](https://stackoverflow.com/a/42187448/446279)) and are slower, however, their instances can be cached during the first invocation and reused during the next invocation.
Instance delegates do not require arguments reshuffling and are faster than static delegates, however, instances cannot be cached and have to be recreated.
Lambdas are the best of both worlds, the compiler generates a class with a delegate field, instantiates an instance during the first invocation and caches it. This instance is used as the delegate's target. Lambdas are therefore fast and don't allocate.

| Delegate type | Instantiation                    | Target                                       |
|-------------- |--------------------------------- |--------------------------------------------- |
| Instance      | For each invocation              | Not null (instance)                          |
| Static        | Only during the first invocation | Null                                         |
| Lambda        | Only during the first invocation | Not null (compiler generated class instance) |
