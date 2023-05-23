# Regex
This folder contains benchmarks comparing different methods of parsing a string and extracting groups. The string has the following format: "Year-Month [Quarter]" where Quarter is optional.
When used, `Regex` instances are created before the benchmark run to measure execution performance only.

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

### Manual
Parses manually character by character.

### IndexOfString
Parses using `string.IndexOfString`.

### IndexOfSpan
Parses using `ReadOnlySpan<char>.IndexOf`.

### RegexNotCompiledString
Parses using `Regex` without `RegexOptions.Compiled` and extracts the groups as strings.

```csharp
new System.Text.RegularExpressions.Regex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$");
```

### RegexCompiledString
Parses using `Regex` with `RegexOptions.Compiled` and extracts the groups as strings.

```csharp
new System.Text.RegularExpressions.Regex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$", RegexOptions.Compiled);
```

### RegexPrecompiledString
Parses using a pre-compiled `Regex` using `GeneractedRegex` and extracts the groups as strings.

```csharp
[GeneratedRegex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$")]
private static partial System.Text.RegularExpressions.Regex GetRegex();
```

### RegexNotCompiledSpan
Parses using `Regex` without `RegexOptions.Compiled` and extracts the groups as `ReadOnlySpan&lt;char&gt;`.

```csharp
new System.Text.RegularExpressions.Regex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$");
```

### RegexCompiledSpan
Parses using `Regex` with `RegexOptions.Compiled` and extracts the groups as `ReadOnlySpan&lt;char&gt;`.

```csharp
new System.Text.RegularExpressions.Regex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$", RegexOptions.Compiled);
```

### RegexPrecompiledSpan
Parses using a pre-compiled `Regex` using `GeneractedRegex` and extracts the groups as `ReadOnlySpan&lt;char&gt;`.

```csharp
[GeneratedRegex(@"^(?<year>\d{4})\-(?<month>\d{1,2})( \[(?<quarter>\d{1,2})\])?$")]
private static partial System.Text.RegularExpressions.Regex GetRegex();
```

## Results
### Features available on .NET Framework 4.7.2 and .NET 7.0
|                 Method |                  Job |              Runtime | Loops |         Mean |       Error |      StdDev | Ratio | RatioSD |     Gen0 | Allocated | Alloc Ratio |
|----------------------- |--------------------- |--------------------- |------ |-------------:|------------:|------------:|------:|--------:|---------:|----------:|------------:|
|                 Manual |             .NET 7.0 |             .NET 7.0 |    10 |     156.6 ns |     1.27 ns |     1.19 ns |  1.00 |    0.00 |        - |         - |          NA |
|          IndexOfString |             .NET 7.0 |             .NET 7.0 |    10 |     790.1 ns |     2.29 ns |     2.14 ns |  5.05 |    0.04 |   0.0629 |     800 B |          NA |
| RegexNotCompiledString |             .NET 7.0 |             .NET 7.0 |    10 |   3,874.7 ns |    25.96 ns |    23.01 ns | 24.73 |    0.26 |   0.6485 |    8160 B |          NA |
|    RegexCompiledString |             .NET 7.0 |             .NET 7.0 |    10 |   2,768.2 ns |    20.81 ns |    17.38 ns | 17.68 |    0.23 |   0.6485 |    8160 B |          NA |
|                        |                      |                      |       |              |             |             |       |         |          |           |             |
|                 Manual | .NET Framework 4.7.2 | .NET Framework 4.7.2 |    10 |     171.5 ns |     0.63 ns |     0.49 ns |  1.00 |    0.00 |        - |         - |          NA |
|          IndexOfString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |    10 |   3,150.0 ns |     4.90 ns |     4.35 ns | 18.37 |    0.06 |   0.1640 |    1043 B |          NA |
| RegexNotCompiledString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |    10 |   6,921.5 ns |    71.63 ns |    67.01 ns | 40.46 |    0.42 |   1.3351 |    8425 B |          NA |
|    RegexCompiledString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |    10 |   5,700.1 ns |    10.40 ns |     9.73 ns | 33.23 |    0.11 |   1.3351 |    8425 B |          NA |
|                        |                      |                      |       |              |             |             |       |         |          |           |             |
|                 Manual |             .NET 7.0 |             .NET 7.0 |   100 |   1,669.5 ns |     6.60 ns |     6.18 ns |  1.00 |    0.00 |        - |         - |          NA |
|          IndexOfString |             .NET 7.0 |             .NET 7.0 |   100 |   7,712.0 ns |    54.63 ns |    48.43 ns |  4.62 |    0.04 |   0.6256 |    8000 B |          NA |
| RegexNotCompiledString |             .NET 7.0 |             .NET 7.0 |   100 |  40,190.4 ns |   121.86 ns |   101.76 ns | 24.07 |    0.12 |   6.4697 |   81600 B |          NA |
|    RegexCompiledString |             .NET 7.0 |             .NET 7.0 |   100 |  27,547.1 ns |   530.05 ns |   725.53 ns | 16.36 |    0.43 |   6.5002 |   81600 B |          NA |
|                        |                      |                      |       |              |             |             |       |         |          |           |             |
|                 Manual | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   100 |   1,615.4 ns |    11.26 ns |    10.53 ns |  1.00 |    0.00 |        - |         - |          NA |
|          IndexOfString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   100 |  31,517.8 ns |    93.72 ns |    83.08 ns | 19.52 |    0.14 |   1.6479 |   10431 B |          NA |
| RegexNotCompiledString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   100 |  68,581.2 ns |   185.67 ns |   155.04 ns | 42.45 |    0.31 |  13.3057 |   84248 B |          NA |
|    RegexCompiledString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   100 |  57,013.2 ns |   121.12 ns |   107.37 ns | 35.30 |    0.24 |  13.3667 |   84248 B |          NA |
|                        |                      |                      |       |              |             |             |       |         |          |           |             |
|                 Manual |             .NET 7.0 |             .NET 7.0 |  1000 |  17,283.3 ns |    19.43 ns |    17.22 ns |  1.00 |    0.00 |        - |         - |          NA |
|          IndexOfString |             .NET 7.0 |             .NET 7.0 |  1000 |  78,599.3 ns |   632.02 ns |   560.27 ns |  4.55 |    0.03 |   6.3477 |   80000 B |          NA |
| RegexNotCompiledString |             .NET 7.0 |             .NET 7.0 |  1000 | 378,812.3 ns | 2,088.11 ns | 1,851.06 ns | 21.92 |    0.11 |  64.9414 |  816000 B |          NA |
|    RegexCompiledString |             .NET 7.0 |             .NET 7.0 |  1000 | 277,962.2 ns | 1,908.95 ns | 1,785.64 ns | 16.09 |    0.11 |  64.9414 |  816000 B |          NA |
|                        |                      |                      |       |              |             |             |       |         |          |           |             |
|                 Manual | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  1000 |  15,920.5 ns |   112.27 ns |   105.02 ns |  1.00 |    0.00 |        - |         - |          NA |
|          IndexOfString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  1000 | 313,549.0 ns |   716.36 ns |   598.19 ns | 19.68 |    0.11 |  16.1133 |  104309 B |          NA |
| RegexNotCompiledString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  1000 | 680,513.7 ns | 1,219.80 ns | 1,081.32 ns | 42.70 |    0.21 | 133.7891 |  842474 B |          NA |
|    RegexCompiledString | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  1000 | 567,279.9 ns |   858.94 ns |   761.42 ns | 35.60 |    0.18 | 133.7891 |  842482 B |          NA |

### .NET 7.0 features
|                 Method | Loops |         Mean |        Error |       StdDev |       Median | Ratio | RatioSD |    Gen0 | Allocated | Alloc Ratio |
|----------------------- |------ |-------------:|-------------:|-------------:|-------------:|------:|--------:|--------:|----------:|------------:|
|                 Manual |    10 |     156.8 ns |      2.65 ns |      2.35 ns |     156.5 ns |  1.00 |    0.00 |       - |         - |          NA |
|            IndexOfSpan |    10 |     340.9 ns |      0.68 ns |      0.57 ns |     340.7 ns |  2.17 |    0.03 |       - |         - |          NA |
| RegexPrecompiledString |    10 |   2,543.8 ns |     13.08 ns |     10.92 ns |   2,542.6 ns | 16.23 |    0.27 |  0.6485 |    8160 B |          NA |
|   RegexNotCompiledSpan |    10 |   3,639.6 ns |     63.66 ns |     59.55 ns |   3,638.1 ns | 23.22 |    0.56 |  0.5836 |    7360 B |          NA |
|      RegexCompiledSpan |    10 |   2,653.5 ns |     52.58 ns |     68.37 ns |   2,663.3 ns | 16.92 |    0.48 |  0.5836 |    7360 B |          NA |
|   RegexPreCompiledSpan |    10 |   2,363.7 ns |     19.13 ns |     17.89 ns |   2,361.3 ns | 15.07 |    0.25 |  0.5836 |    7360 B |          NA |
|                        |       |              |              |              |              |       |         |         |           |             |
|                 Manual |   100 |   1,676.1 ns |     24.85 ns |     22.03 ns |   1,666.7 ns |  1.00 |    0.00 |       - |         - |          NA |
|            IndexOfSpan |   100 |   3,316.6 ns |     23.98 ns |     21.25 ns |   3,319.9 ns |  1.98 |    0.03 |       - |         - |          NA |
| RegexPrecompiledString |   100 |  24,954.0 ns |    175.82 ns |    146.82 ns |  24,959.2 ns | 14.88 |    0.17 |  6.5002 |   81600 B |          NA |
|   RegexNotCompiledSpan |   100 |  35,837.0 ns |    227.91 ns |    202.03 ns |  35,867.6 ns | 21.38 |    0.28 |  5.8594 |   73600 B |          NA |
|      RegexCompiledSpan |   100 |  25,760.3 ns |    177.14 ns |    165.70 ns |  25,771.6 ns | 15.37 |    0.25 |  5.8594 |   73600 B |          NA |
|   RegexPreCompiledSpan |   100 |  24,848.3 ns |    176.03 ns |    164.66 ns |  24,884.4 ns | 14.82 |    0.21 |  5.8594 |   73600 B |          NA |
|                        |       |              |              |              |              |       |         |         |           |             |
|                 Manual |  1000 |  16,708.6 ns |    130.54 ns |    115.72 ns |  16,747.3 ns |  1.00 |    0.00 |       - |         - |          NA |
|            IndexOfSpan |  1000 |  32,165.0 ns |    208.40 ns |    174.02 ns |  32,092.7 ns |  1.93 |    0.01 |       - |         - |          NA |
| RegexPrecompiledString |  1000 | 246,871.8 ns |  1,241.57 ns |  1,100.62 ns | 246,582.3 ns | 14.78 |    0.13 | 64.9414 |  816000 B |          NA |
|   RegexNotCompiledSpan |  1000 | 360,668.8 ns |  3,097.96 ns |  2,746.26 ns | 360,440.6 ns | 21.59 |    0.18 | 58.5938 |  736000 B |          NA |
|      RegexCompiledSpan |  1000 | 359,436.7 ns | 15,460.42 ns | 44,606.84 ns | 377,092.5 ns | 19.93 |    3.30 | 58.5938 |  736000 B |          NA |
|   RegexPreCompiledSpan |  1000 | 252,484.6 ns |  1,507.05 ns |  1,335.96 ns | 252,113.3 ns | 15.11 |    0.12 | 58.5938 |  736000 B |          NA |

## Conclusions
- .NET 7.0 significantly faster than .NET Framework 4.7.2, even for cases not using `Regex`
- Compilation improves performance more on .NET 7.0 (~-40%) than .NET Framework 4.7.2 (~-20%)
- Regex on spans surprisingly slower than on strings in the 1K test but allocating less
