# BitCollection
This folder contains benchmarks comparing different methods of accessing (read/write) a collection of bits.

## Environment
<p>
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)<br/>
12th Gen Intel Core i9-12900H, 1 CPU, 20 logical and 14 physical cores<br/>
.NET SDK=7.0.203<br/>
  [Host]               : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET 7.0             : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2<br/>
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256<br/>
</p>

## Initialization
Benchmarks comparing different methods of creating and initializing a collection of bits.

### Benchmarks

#### BooleanArray
Creates and initializes a `bool[]`.

#### BitArrayFor
Creates and initializes a `BitArray` using a for loop.

#### BitArrayFor
Creates and initializes a `BitArray` using a for loop.

#### BitArrayFromArray
Creates and initializes a `BitArray` using a for a `bool[]`.

### Results
|             Method | Count |         Mean |     Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|------------------- |------ |-------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
|       BooleanArray |   100 |     53.61 ns |  0.451 ns |  0.400 ns |  1.00 |    0.00 | 0.0102 |     128 B |        1.00 |
|        BitArrayFor |   100 |    141.01 ns |  1.020 ns |  0.954 ns |  2.63 |    0.03 | 0.0057 |      72 B |        0.56 |
|  BitArrayFromArray |   100 |     61.85 ns |  1.181 ns |  1.263 ns |  1.15 |    0.03 | 0.0159 |     200 B |        1.56 |
|                    |       |              |           |           |       |         |        |           |             |
|       BooleanArray |  1000 |    568.46 ns |  6.476 ns |  5.740 ns |  1.00 |    0.00 | 0.0811 |    1024 B |        1.00 |
|        BitArrayFor |  1000 |  1,221.84 ns |  2.521 ns |  2.235 ns |  2.15 |    0.02 | 0.0134 |     184 B |        0.18 |
|  BitArrayFromArray |  1000 |    616.54 ns |  5.711 ns |  5.342 ns |  1.08 |    0.02 | 0.0954 |    1208 B |        1.18 |
|                    |       |              |           |           |       |         |        |           |             |
|       BooleanArray | 10000 |  5,768.69 ns | 11.269 ns |  9.990 ns |  1.00 |    0.00 | 0.7935 |   10024 B |        1.00 |
|        BitArrayFor | 10000 | 12,206.96 ns | 78.892 ns | 65.878 ns |  2.12 |    0.01 | 0.0916 |    1312 B |        0.13 |
|  BitArrayFromArray | 10000 |  6,039.59 ns | 59.539 ns | 52.780 ns |  1.05 |    0.01 | 0.9003 |   11336 B |        1.13 |

### Conclusions
- Initializing a `BitArray` from a `bit[]` allocates more but is more efficient than setting bits individually.

## CountSetBits
Benchmarks comparing different methods of counting specific bits in a bits collection.

### Benchmarks

#### BooleanArrayFor
Counts the number of true elements in a `bool[]` using a for loop.

#### BooleanArrayForEach
Counts the number of true elements in a `bool[]` using a foreach loop.

#### BooleanArrayLinq
Counts the number of true elements in a `bool[]` using Linq.

#### BitArrayFor
Counts the number of true elements in a `BitArray` using a for loop.

#### BitArrayForEach
Counts the number of true elements in a `BitArray` using a for each loop.

#### BitArrayLinq
Counts the number of true elements in a `BitArray` using Linq.

### Results
|              Method | Count |          Mean |        Error |        StdDev |        Median | Ratio | RatioSD |    Gen0 | Allocated | Alloc Ratio |
|-------------------- |------ |--------------:|-------------:|--------------:|--------------:|------:|--------:|--------:|----------:|------------:|
|     BooleanArrayFor |   100 |      59.85 ns |     0.287 ns |      0.254 ns |      59.75 ns |  1.00 |    0.00 |       - |         - |          NA |
| BooleanArrayForEach |   100 |      58.61 ns |     0.753 ns |      0.668 ns |      58.47 ns |  0.98 |    0.01 |       - |         - |          NA |
|    BooleanArrayLinq |   100 |     583.02 ns |     9.972 ns |     22.303 ns |     573.33 ns | 10.22 |    0.47 |  0.0019 |      32 B |          NA |
|         BitArrayFor |   100 |      75.61 ns |     0.183 ns |      0.143 ns |      75.63 ns |  1.26 |    0.01 |       - |         - |          NA |
|     BitArrayForEach |   100 |     477.08 ns |     9.298 ns |     13.034 ns |     480.15 ns |  7.97 |    0.25 |  0.1941 |    2440 B |          NA |
|        BitArrayLinq |   100 |   1,190.37 ns |    23.355 ns |     33.495 ns |   1,180.69 ns | 20.16 |    0.63 |  0.1984 |    2496 B |          NA |
|                     |       |               |              |               |               |       |         |         |           |             |
|     BooleanArrayFor |  1000 |     445.02 ns |     0.172 ns |      0.144 ns |     445.01 ns |  1.00 |    0.00 |       - |         - |          NA |
| BooleanArrayForEach |  1000 |     357.22 ns |     0.268 ns |      0.237 ns |     357.22 ns |  0.80 |    0.00 |       - |         - |          NA |
|    BooleanArrayLinq |  1000 |   5,278.97 ns |    95.879 ns |    175.320 ns |   5,281.03 ns | 11.92 |    0.62 |       - |      32 B |          NA |
|         BitArrayFor |  1000 |     654.89 ns |     0.403 ns |      0.358 ns |     654.88 ns |  1.47 |    0.00 |       - |         - |          NA |
|     BitArrayForEach |  1000 |   5,053.18 ns |    98.129 ns |    146.874 ns |   4,999.93 ns | 11.35 |    0.35 |  1.9150 |   24040 B |          NA |
|        BitArrayLinq |  1000 |  12,056.70 ns |   232.728 ns |    258.677 ns |  12,011.42 ns | 26.82 |    0.41 |  1.9073 |   24096 B |          NA |
|                     |       |               |              |               |               |       |         |         |           |             |
|     BooleanArrayFor | 10000 |   4,306.32 ns |     4.630 ns |      4.104 ns |   4,305.14 ns |  1.00 |    0.00 |       - |         - |          NA |
| BooleanArrayForEach | 10000 |   3,351.98 ns |     1.504 ns |      1.333 ns |   3,351.81 ns |  0.78 |    0.00 |       - |         - |          NA |
|    BooleanArrayLinq | 10000 |  54,896.52 ns |   521.505 ns |    435.480 ns |  54,829.97 ns | 12.75 |    0.10 |       - |      32 B |          NA |
|         BitArrayFor | 10000 |   6,447.28 ns |     4.980 ns |      4.159 ns |   6,445.72 ns |  1.50 |    0.00 |       - |         - |          NA |
|     BitArrayForEach | 10000 |  46,696.52 ns |   294.709 ns |    230.090 ns |  46,779.71 ns | 10.84 |    0.05 | 19.1040 |  240040 B |          NA |
|        BitArrayLinq | 10000 | 133,869.38 ns | 4,591.160 ns | 13,024.357 ns | 136,333.45 ns | 30.76 |    1.61 | 19.0430 |  240096 B |          NA |

### Conclusions
- When using `BitArray`, for loops are the most efficient way of accessing the collection's elements.
- For each loops and Linq significantly impact performance and allocate (`BitArrayEnumeratorSimple` for the most part).
- It is unfortunate that `BitArray` implements `ICollection` and not `IReadOnlyList<bool>` or at least `IEnumerable<bool>`.

## ReadWriteBits
Benchmarks comparing methods of setting bits to true and false in a bit collection.

### Benchmarks

#### BooleanArray
Creates a `bool[]` and read/writes from/to it.

#### BooleanArrayStackalloc
Creates a stack allocated `bool[]` and read/writes from/to it.

#### List
Creates a `List<bool>` and read/writes from/to it.

#### BitArray
Creates a `BitArray` and read/writes from/to it.

### Results
|                 Method | Count |         Mean |      Error |     StdDev | Ratio | RatioSD |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
|----------------------- |------ |-------------:|-----------:|-----------:|------:|--------:|-------:|-------:|----------:|------------:|
|           BooleanArray |   100 |     89.48 ns |   0.709 ns |   0.592 ns |  1.00 |    0.00 | 0.0101 |      - |     128 B |        1.00 |
| BooleanArrayStackalloc |   100 |     82.78 ns |   0.355 ns |   0.296 ns |  0.93 |    0.01 |      - |      - |         - |        0.00 |
|                   List |   100 |    151.24 ns |   2.140 ns |   1.671 ns |  1.69 |    0.02 | 0.0126 |      - |     160 B |        1.25 |
|               BitArray |   100 |    218.88 ns |   0.746 ns |   0.623 ns |  2.45 |    0.01 | 0.0057 |      - |      72 B |        0.56 |
|                        |       |              |            |            |       |         |        |        |           |             |
|           BooleanArray |  1000 |    908.63 ns |   2.895 ns |   2.566 ns |  1.00 |    0.00 | 0.0811 |      - |    1024 B |        1.00 |
| BooleanArrayStackalloc |  1000 |    829.48 ns |   9.768 ns |   8.157 ns |  0.91 |    0.01 |      - |      - |         - |        0.00 |
|                   List |  1000 |  1,265.95 ns |  15.178 ns |  13.455 ns |  1.39 |    0.01 | 0.0839 |      - |    1056 B |        1.03 |
|               BitArray |  1000 |  1,887.40 ns |   1.203 ns |   1.005 ns |  2.08 |    0.01 | 0.0134 |      - |     184 B |        0.18 |
|                        |       |              |            |            |       |         |        |        |           |             |
|           BooleanArray | 10000 |  9,162.37 ns |  41.004 ns |  38.355 ns |  1.00 |    0.00 | 0.7935 |      - |   10024 B |        1.00 |
| BooleanArrayStackalloc | 10000 |  8,772.56 ns |  29.452 ns |  26.109 ns |  0.96 |    0.00 |      - |      - |         - |        0.00 |
|                   List | 10000 | 12,276.21 ns | 111.014 ns | 169.530 ns |  1.35 |    0.02 | 0.7935 | 0.0153 |   10056 B |        1.00 |
|               BitArray | 10000 | 18,754.94 ns |  23.633 ns |  20.950 ns |  2.05 |    0.01 | 0.0916 |      - |    1312 B |        0.13 |

### Conclusions
- `BitArray` more than twice slower than a `bool[]` but allocates less.
