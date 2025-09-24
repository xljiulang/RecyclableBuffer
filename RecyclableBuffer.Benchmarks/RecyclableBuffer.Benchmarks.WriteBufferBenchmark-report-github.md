```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                           | BufferSize | Mean         | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------------------- |----------- |-------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **1024**       |     **61.55 ns** |  **1.180 ns** |  **2.384 ns** |  **0.65** |    **0.03** | **0.0126** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 1024       |     94.24 ns |  1.880 ns |  3.090 ns |  1.00 |    0.05 | 0.0293 |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 1024       |    197.98 ns |  3.962 ns |  3.512 ns |  2.10 |    0.08 | 0.0446 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 1024       |    225.08 ns |  1.582 ns |  1.480 ns |  2.39 |    0.08 | 0.0293 |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 1024       |    425.36 ns |  4.852 ns |  4.538 ns |  4.52 |    0.15 | 0.0353 |     224 B |        1.22 |
|                                  |            |              |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **8192**       |    **113.21 ns** |  **0.473 ns** |  **0.395 ns** |  **0.94** |    **0.01** | **0.0126** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 8192       |    120.32 ns |  1.771 ns |  1.657 ns |  1.00 |    0.02 | 0.0293 |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192       |    233.89 ns |  1.136 ns |  1.062 ns |  1.94 |    0.03 | 0.0443 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 8192       |    394.17 ns |  3.709 ns |  3.469 ns |  3.28 |    0.05 | 0.0291 |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 8192       |    478.01 ns |  4.050 ns |  3.382 ns |  3.97 |    0.06 | 0.0496 |     312 B |        1.70 |
|                                  |            |              |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **131073**     |  **6,036.96 ns** | **22.092 ns** | **18.448 ns** |  **2.47** |    **0.01** | **0.0153** |     **120 B** |        **0.54** |
| MultipleSegmentBufferWriter      | 131073     |  2,442.87 ns |  4.245 ns |  3.763 ns |  1.00 |    0.00 | 0.0343 |     224 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 131073     |  2,578.73 ns |  3.225 ns |  3.017 ns |  1.06 |    0.00 | 0.0496 |     312 B |        1.39 |
| DotNext_PoolingArrayBufferWriter | 131073     | 10,697.19 ns | 19.727 ns | 18.453 ns |  4.38 |    0.01 | 0.0153 |     184 B |        0.82 |
| DotNext_SparseBufferWriter       | 131073     |  5,535.72 ns | 22.245 ns | 20.808 ns |  2.27 |    0.01 | 0.4807 |    3040 B |       13.57 |
