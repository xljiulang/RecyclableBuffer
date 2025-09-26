```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                              | SizeHint | Mean         | Error      | StdDev     | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------------ |--------- |-------------:|-----------:|-----------:|-------------:|-------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **SingleSegmentBufferWriter**           | **0**        |     **49.29 ns** |   **1.005 ns** |   **1.271 ns** |     **48.85 ns** |   **0.74** |    **0.02** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared  | 0        |     66.45 ns |   1.361 ns |   1.397 ns |     66.32 ns |   1.00 |    0.03 |   0.0293 |        - |        - |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Default | 0        |     58.36 ns |   1.123 ns |   1.050 ns |     58.30 ns |   0.88 |    0.02 |   0.0293 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream    | 0        |    194.93 ns |   3.724 ns |   8.405 ns |    191.90 ns |   2.93 |    0.14 |   0.0446 |        - |        - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter    | 0        |     87.48 ns |   1.725 ns |   2.181 ns |     87.82 ns |   1.32 |    0.04 |   0.0293 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter          | 0        |    293.11 ns |   5.790 ns |   6.668 ns |    293.71 ns |   4.41 |    0.13 |   0.0353 |        - |        - |     224 B |        1.22 |
|                                     |          |              |            |            |              |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**           | **8192**     |     **47.96 ns** |   **0.958 ns** |   **1.463 ns** |     **47.72 ns** |   **0.72** |    **0.03** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared  | 8192     |     66.64 ns |   1.310 ns |   1.457 ns |     66.36 ns |   1.00 |    0.03 |   0.0293 |        - |        - |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Default | 8192     |     57.89 ns |   1.181 ns |   1.450 ns |     57.48 ns |   0.87 |    0.03 |   0.0293 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream    | 8192     |    188.63 ns |   1.575 ns |   1.473 ns |    188.19 ns |   2.83 |    0.06 |   0.0446 |        - |        - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter    | 8192     |     86.93 ns |   1.339 ns |   1.252 ns |     86.98 ns |   1.31 |    0.03 |   0.0293 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter          | 8192     |    297.56 ns |   4.444 ns |   4.157 ns |    297.21 ns |   4.47 |    0.11 |   0.0353 |        - |        - |     224 B |        1.22 |
|                                     |          |              |            |            |              |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**           | **131073**   |     **67.07 ns** |   **1.147 ns** |   **1.073 ns** |     **66.93 ns** |   **1.02** |    **0.02** |   **0.0191** |        **-** |        **-** |     **120 B** |        **0.65** |
| MultipleSegmentBufferWriter_Shared  | 131073   |     65.79 ns |   1.208 ns |   1.130 ns |     65.36 ns |   1.00 |    0.02 |   0.0293 |        - |        - |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Default | 131073   |     66.54 ns |   1.086 ns |   1.016 ns |     66.40 ns |   1.01 |    0.02 |   0.0293 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream    | 131073   | 17,094.57 ns | 164.537 ns | 153.908 ns | 17,025.96 ns | 259.91 |    4.88 | 333.3130 | 333.3130 | 333.3130 | 1049021 B |    5,701.20 |
| DotNext_PoolingArrayBufferWriter    | 131073   |     85.94 ns |   0.809 ns |   0.757 ns |     85.93 ns |   1.31 |    0.02 |   0.0293 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter          | 131073   |    292.12 ns |   5.224 ns |   5.364 ns |    292.42 ns |   4.44 |    0.11 |   0.0353 |        - |        - |     224 B |        1.22 |
