```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
Intel Core i7-8565U CPU 1.80GHz (Max: 1.99GHz) (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                           | BufferSize | Mean       | Error     | StdDev     | Ratio | RatioSD | Gen0       | Gen1      | Allocated    | Alloc Ratio |
|--------------------------------- |----------- |-----------:|----------:|-----------:|------:|--------:|-----------:|----------:|-------------:|------------:|
| **SingleSegmentBufferWriter**        | **1024**       |   **2.449 ms** | **0.0488 ms** |  **0.1352 ms** |  **0.86** |    **0.05** |   **191.4063** |         **-** |    **783.26 KB** |        **0.44** |
| MultipleSegmentBufferWriter      | 1024       |   2.856 ms | 0.0442 ms |  0.0413 ms |  1.00 |    0.02 |   437.5000 |         - |   1798.88 KB |        1.00 |
| Microsoft_RecyclableMemoryStream | 1024       |   4.970 ms | 0.1046 ms |  0.2985 ms |  1.74 |    0.11 |   671.8750 |         - |   2736.38 KB |        1.52 |
| DotNext_PoolingArrayBufferWriter | 1024       |   1.838 ms | 0.0354 ms |  0.0421 ms |  0.64 |    0.02 |   445.3125 |         - |      1799 KB |        1.00 |
| DotNext_SparseBufferWriter       | 1024       |   3.872 ms | 0.1036 ms |  0.2973 ms |  1.36 |    0.11 |   535.1563 |         - |   2189.51 KB |        1.22 |
|                                  |            |            |           |            |       |         |            |           |              |             |
| **SingleSegmentBufferWriter**        | **8192**       |   **2.194 ms** | **0.0406 ms** |  **0.0678 ms** |  **0.97** |    **0.03** |   **191.4063** |         **-** |    **783.26 KB** |        **0.44** |
| MultipleSegmentBufferWriter      | 8192       |   2.268 ms | 0.0164 ms |  0.0153 ms |  1.00 |    0.01 |   441.4063 |         - |   1798.88 KB |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192       |   6.128 ms | 0.1218 ms |  0.3077 ms |  2.70 |    0.14 |   671.8750 |         - |   2736.39 KB |        1.52 |
| DotNext_PoolingArrayBufferWriter | 8192       |   3.452 ms | 0.0751 ms |  0.2190 ms |  1.52 |    0.10 |   445.3125 |         - |   1798.99 KB |        1.00 |
| DotNext_SparseBufferWriter       | 8192       |   6.228 ms | 0.2484 ms |  0.7323 ms |  2.75 |    0.32 |   742.1875 |         - |    3048.9 KB |        1.69 |
|                                  |            |            |           |            |       |         |            |           |              |             |
| **SingleSegmentBufferWriter**        | **524288**     | **111.986 ms** | **1.7881 ms** |  **1.5851 ms** |  **1.45** |    **0.05** |   **200.0000** |         **-** |   **1564.57 KB** |        **0.53** |
| MultipleSegmentBufferWriter      | 524288     |  77.419 ms | 1.5312 ms |  2.4286 ms |  1.00 |    0.04 |   714.2857 |         - |    2970.8 KB |        1.00 |
| Microsoft_RecyclableMemoryStream | 524288     |  84.206 ms | 1.6736 ms |  2.5558 ms |  1.09 |    0.05 |  1000.0000 |         - |   4689.59 KB |        1.58 |
| DotNext_PoolingArrayBufferWriter | 524288     | 151.906 ms | 3.0318 ms |  8.6988 ms |  1.96 |    0.13 |   250.0000 |         - |   1799.06 KB |        0.61 |
| DotNext_SparseBufferWriter       | 524288     | 373.723 ms | 7.4237 ms | 20.5712 ms |  4.83 |    0.30 | 24000.0000 | 1000.0000 | 116548.84 KB |       39.23 |
