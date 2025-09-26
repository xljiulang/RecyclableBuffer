```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                              | BufferSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0       | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|------------------------------------ |----------- |-----------:|----------:|----------:|------:|--------:|-----------:|----------:|---------:|-------------:|------------:|
| **SingleSegmentBufferWriter**           | **1024**       |   **3.945 ms** | **0.0809 ms** | **0.2385 ms** |  **1.29** |    **0.10** |   **125.0000** |         **-** |        **-** |    **783.75 KB** |        **0.44** |
| MultipleSegmentBufferWriter_Shared  | 1024       |   3.067 ms | 0.0612 ms | 0.1331 ms |  1.00 |    0.06 |   292.9688 |         - |        - |   1799.36 KB |        1.00 |
| MultipleSegmentBufferWriter_Default | 1024       |   2.947 ms | 0.0587 ms | 0.1226 ms |  0.96 |    0.06 |   292.9688 |         - |        - |   1799.36 KB |        1.00 |
| Microsoft_RecyclableMemoryStream    | 1024       |   4.164 ms | 0.0426 ms | 0.0399 ms |  1.36 |    0.06 |   437.5000 |         - |        - |   2736.86 KB |        1.52 |
| DotNext_PoolingArrayBufferWriter    | 1024       |   6.702 ms | 0.3273 ms | 0.9650 ms |  2.19 |    0.33 |   296.8750 |         - |        - |   1799.45 KB |        1.00 |
| DotNext_SparseBufferWriter          | 1024       |   5.851 ms | 0.1147 ms | 0.1751 ms |  1.91 |    0.10 |   351.5625 |         - |        - |   2190.18 KB |        1.22 |
|                                     |            |            |           |           |       |         |            |           |          |              |             |
| **SingleSegmentBufferWriter**           | **8192**       |   **3.098 ms** | **0.0658 ms** | **0.1930 ms** |  **1.02** |    **0.08** |   **125.0000** |         **-** |        **-** |    **787.24 KB** |        **0.44** |
| MultipleSegmentBufferWriter_Shared  | 8192       |   3.050 ms | 0.0600 ms | 0.1239 ms |  1.00 |    0.06 |   292.9688 |         - |        - |   1802.36 KB |        1.00 |
| MultipleSegmentBufferWriter_Default | 8192       |   3.824 ms | 0.0855 ms | 0.2522 ms |  1.26 |    0.10 |   292.9688 |         - |        - |   1799.37 KB |        1.00 |
| Microsoft_RecyclableMemoryStream    | 8192       |   4.864 ms | 0.0711 ms | 0.0630 ms |  1.60 |    0.07 |   437.5000 |         - |        - |   2736.87 KB |        1.52 |
| DotNext_PoolingArrayBufferWriter    | 8192       |   2.883 ms | 0.0234 ms | 0.0195 ms |  0.95 |    0.04 |   296.8750 |    3.9063 |        - |   1799.79 KB |        1.00 |
| DotNext_SparseBufferWriter          | 8192       |   8.395 ms | 0.1483 ms | 0.1315 ms |  2.76 |    0.12 |   484.3750 |         - |        - |   3049.94 KB |        1.69 |
|                                     |            |            |           |           |       |         |            |           |          |              |             |
| **SingleSegmentBufferWriter**           | **524288**     |  **76.060 ms** | **1.3935 ms** | **1.3035 ms** |  **2.26** |    **0.05** |   **285.7143** |  **142.8571** | **142.8571** |   **2534.43 KB** |        **0.84** |
| MultipleSegmentBufferWriter_Shared  | 524288     |  33.715 ms | 0.4966 ms | 0.4402 ms |  1.00 |    0.02 |   437.5000 |         - |        - |   3019.32 KB |        1.00 |
| MultipleSegmentBufferWriter_Default | 524288     |  67.383 ms | 0.8511 ms | 0.7545 ms |  2.00 |    0.03 |   375.0000 |         - |        - |   2971.31 KB |        0.98 |
| Microsoft_RecyclableMemoryStream    | 524288     |  69.722 ms | 0.5591 ms | 0.5230 ms |  2.07 |    0.03 |   750.0000 |         - |        - |   4690.06 KB |        1.55 |
| DotNext_PoolingArrayBufferWriter    | 524288     |  96.287 ms | 1.3668 ms | 1.2785 ms |  2.86 |    0.05 |   333.3333 |  333.3333 | 166.6667 |   3208.03 KB |        1.06 |
| DotNext_SparseBufferWriter          | 524288     | 346.361 ms | 6.0679 ms | 5.0670 ms | 10.27 |    0.19 | 18000.0000 | 2000.0000 |        - | 113704.81 KB |       37.66 |
