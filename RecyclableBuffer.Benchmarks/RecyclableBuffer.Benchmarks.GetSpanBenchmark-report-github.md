```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                           | SizeHint | Mean         | Error      | StdDev     | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------------- |--------- |-------------:|-----------:|-----------:|-------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **0**        |     **48.77 ns** |   **1.004 ns** |   **1.031 ns** |   **0.63** |    **0.01** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 0        |     76.86 ns |   0.694 ns |   0.615 ns |   1.00 |    0.01 |   0.0293 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 0        |    183.03 ns |   1.069 ns |   1.000 ns |   2.38 |    0.02 |   0.0446 |        - |        - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 0        |     85.21 ns |   1.692 ns |   1.662 ns |   1.11 |    0.02 |   0.0293 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 0        |    292.89 ns |   4.801 ns |   4.491 ns |   3.81 |    0.06 |   0.0353 |        - |        - |     224 B |        1.22 |
|                                  |          |              |            |            |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **8192**     |     **46.89 ns** |   **0.420 ns** |   **0.350 ns** |   **0.62** |    **0.01** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 8192     |     75.92 ns |   0.684 ns |   0.640 ns |   1.00 |    0.01 |   0.0293 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192     |    185.26 ns |   1.008 ns |   0.943 ns |   2.44 |    0.02 |   0.0446 |        - |        - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 8192     |     85.87 ns |   1.722 ns |   1.611 ns |   1.13 |    0.02 |   0.0293 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 8192     |    289.78 ns |   4.231 ns |   3.751 ns |   3.82 |    0.06 |   0.0353 |        - |        - |     224 B |        1.22 |
|                                  |          |              |            |            |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **131073**   |     **66.83 ns** |   **1.257 ns** |   **2.266 ns** |   **1.01** |    **0.03** |   **0.0191** |        **-** |        **-** |     **120 B** |        **0.65** |
| MultipleSegmentBufferWriter      | 131073   |     66.24 ns |   0.523 ns |   0.436 ns |   1.00 |    0.01 |   0.0293 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 131073   | 17,699.35 ns | 207.608 ns | 194.197 ns | 267.19 |    3.30 | 333.3130 | 333.3130 | 333.3130 | 1049020 B |    5,701.20 |
| DotNext_PoolingArrayBufferWriter | 131073   |     86.48 ns |   1.394 ns |   1.304 ns |   1.31 |    0.02 |   0.0293 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 131073   |    290.17 ns |   5.726 ns |   5.076 ns |   4.38 |    0.08 |   0.0353 |        - |        - |     224 B |        1.22 |
