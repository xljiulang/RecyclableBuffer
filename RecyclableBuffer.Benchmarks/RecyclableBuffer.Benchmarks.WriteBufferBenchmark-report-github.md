```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                           | BufferSize | Mean         | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------------------- |----------- |-------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **1024**       |     **61.75 ns** |  **1.267 ns** |  **1.556 ns** |  **0.71** |    **0.02** | **0.0126** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 1024       |     87.36 ns |  1.127 ns |  1.054 ns |  1.00 |    0.02 | 0.0280 |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 1024       |    192.96 ns |  2.618 ns |  2.321 ns |  2.21 |    0.04 | 0.0446 |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 1024       |    211.21 ns |  4.177 ns |  3.907 ns |  2.42 |    0.05 | 0.0293 |     184 B |        1.05 |
| DotNext_SparseBufferWriter       | 1024       |    308.97 ns |  6.199 ns |  9.466 ns |  3.54 |    0.11 | 0.0353 |     224 B |        1.27 |
|                                  |            |              |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **8192**       |    **107.34 ns** |  **1.451 ns** |  **1.357 ns** |  **0.90** |    **0.02** | **0.0126** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 8192       |    118.72 ns |  2.410 ns |  2.475 ns |  1.00 |    0.03 | 0.0280 |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192       |    235.23 ns |  2.758 ns |  2.445 ns |  1.98 |    0.04 | 0.0443 |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 8192       |    453.33 ns |  7.222 ns |  6.756 ns |  3.82 |    0.09 | 0.0291 |     184 B |        1.05 |
| DotNext_SparseBufferWriter       | 8192       |    462.50 ns |  7.491 ns |  6.640 ns |  3.90 |    0.10 | 0.0496 |     312 B |        1.77 |
|                                  |            |              |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **131073**     | **10,879.44 ns** | **23.968 ns** | **20.014 ns** |  **4.53** |    **0.01** | **0.0153** |     **120 B** |        **0.56** |
| MultipleSegmentBufferWriter      | 131073     |  2,401.78 ns |  6.358 ns |  5.947 ns |  1.00 |    0.00 | 0.0343 |     216 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 131073     |  2,518.96 ns | 16.652 ns | 13.905 ns |  1.05 |    0.01 | 0.0496 |     312 B |        1.44 |
| DotNext_PoolingArrayBufferWriter | 131073     |  7,236.84 ns | 38.716 ns | 34.321 ns |  3.01 |    0.02 | 0.0229 |     184 B |        0.85 |
| DotNext_SparseBufferWriter       | 131073     |  5,552.85 ns | 69.315 ns | 64.838 ns |  2.31 |    0.03 | 0.4807 |    3040 B |       14.07 |
