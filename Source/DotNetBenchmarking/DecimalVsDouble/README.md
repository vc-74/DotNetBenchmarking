# Regex
This folder contains benchmarks comparing double performance vs decimal for arithmetic operations.

## Environment
<p>
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)<br/>
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores<br/>
.NET SDK=7.0.203<br/>
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256<br/>
</p>

## Benchmarks

### Double
Calculates the average of n consecutive values using doubles.

### Decimal
Calculates the average of n consecutive values using decimals.

## Results
|  Method | ValueCount |         Mean |     Error |    StdDev | Allocated |
|-------- |----------- |-------------:|----------:|----------:|----------:|
|  Double |       1000 |     5.236 us | 0.0010 us | 0.0008 us |         - |
| Decimal |       1000 |    69.578 us | 0.1417 us | 0.1325 us |         - |
|  Double |      10000 |    53.873 us | 0.0113 us | 0.0088 us |         - |
| Decimal |      10000 |   763.375 us | 1.8037 us | 1.5990 us |       1 B |
|  Double |     100000 |   540.400 us | 0.2939 us | 0.2295 us |       1 B |
| Decimal |     100000 | 7,703.068 us | 6.6387 us | 5.5436 us |       5 B |

## Conclusions
- Double ~13* faster than decimal
- Linear progression in both cases
