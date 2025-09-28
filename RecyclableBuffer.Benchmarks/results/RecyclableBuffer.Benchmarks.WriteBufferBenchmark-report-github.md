```

BenchmarkDotNet v0.15.3, Linux CentOS Linux 7 (Core)
Intel Xeon CPU E5-2650 v2 2.60GHz (Max: 1.20GHz), 2 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2
  DefaultJob : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2


```
| Method                                   | BufferSize | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------------------------- |----------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**                | **1024**       |    **226.8 ns** |   **4.48 ns** |   **6.70 ns** |    **224.7 ns** |  **0.76** |    **0.04** | **0.0076** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared       | 1024       |    298.9 ns |   6.02 ns |  12.82 ns |    299.8 ns |  1.00 |    0.06 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 1024       |    308.9 ns |   5.60 ns |  10.93 ns |    306.8 ns |  1.04 |    0.06 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable     | 1024       |    280.8 ns |   5.44 ns |   6.88 ns |    281.0 ns |  0.94 |    0.05 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize    | 1024       |    263.1 ns |   5.29 ns |  11.62 ns |    262.5 ns |  0.88 |    0.05 | 0.0196 |     208 B |        1.13 |
| Microsoft_RecyclableMemoryStream         | 1024       |  1,209.6 ns |  22.79 ns |  21.32 ns |  1,202.6 ns |  4.05 |    0.19 | 0.0267 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter         | 1024       |    668.3 ns |   8.83 ns |   7.82 ns |    667.8 ns |  2.24 |    0.10 | 0.0172 |     184 B |        1.00 |
| DotNext_SparseBufferWriter               | 1024       |    364.4 ns |   7.28 ns |   8.93 ns |    365.3 ns |  1.22 |    0.06 | 0.0129 |     136 B |        0.74 |
|                                          |            |             |           |           |             |       |         |        |           |             |
| **SingleSegmentBufferWriter**                | **8192**       |    **471.3 ns** |   **6.59 ns** |   **6.16 ns** |    **471.2 ns** |  **0.92** |    **0.03** | **0.0076** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared       | 8192       |    510.1 ns |   8.79 ns |  14.93 ns |    506.8 ns |  1.00 |    0.04 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 8192       |    504.7 ns |  10.00 ns |  12.28 ns |    500.9 ns |  0.99 |    0.04 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable     | 8192       |    485.1 ns |   8.52 ns |   6.65 ns |    485.0 ns |  0.95 |    0.03 | 0.0191 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize    | 8192       |    484.5 ns |   9.17 ns |   8.13 ns |    485.7 ns |  0.95 |    0.03 | 0.0196 |     208 B |        1.13 |
| Microsoft_RecyclableMemoryStream         | 8192       |  1,530.2 ns |  28.97 ns |  29.75 ns |  1,536.7 ns |  3.00 |    0.10 | 0.0267 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter         | 8192       |  1,311.4 ns |  25.46 ns |  31.27 ns |  1,311.0 ns |  2.57 |    0.09 | 0.0172 |     184 B |        1.00 |
| DotNext_SparseBufferWriter               | 8192       |    577.7 ns |  11.53 ns |  13.28 ns |    575.3 ns |  1.13 |    0.04 | 0.0124 |     136 B |        0.74 |
|                                          |            |             |           |           |             |       |         |        |           |             |
| **SingleSegmentBufferWriter**                | **524288**     | **50,955.1 ns** | **591.07 ns** | **461.47 ns** | **50,929.3 ns** |  **2.09** |    **0.03** |      **-** |     **160 B** |        **0.53** |
| MultipleSegmentBufferWriter_Shared       | 524288     | 24,376.1 ns | 276.95 ns | 259.06 ns | 24,394.5 ns |  1.00 |    0.01 |      - |     304 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 524288     | 22,723.8 ns | 452.10 ns | 717.08 ns | 22,380.4 ns |  0.93 |    0.03 |      - |     304 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable     | 524288     | 22,217.5 ns | 248.74 ns | 232.67 ns | 22,226.4 ns |  0.91 |    0.01 |      - |     424 B |        1.39 |
| MultipleSegmentBufferWriter_FixedSize    | 524288     | 22,756.5 ns | 358.09 ns | 299.03 ns | 22,650.8 ns |  0.93 |    0.02 | 0.0305 |     328 B |        1.08 |
| Microsoft_RecyclableMemoryStream         | 524288     | 24,001.3 ns | 462.99 ns | 410.43 ns | 23,918.7 ns |  0.98 |    0.02 | 0.0305 |     480 B |        1.58 |
| DotNext_PoolingArrayBufferWriter         | 524288     | 59,939.2 ns | 986.57 ns | 922.84 ns | 59,522.8 ns |  2.46 |    0.04 |      - |     184 B |        0.61 |
| DotNext_SparseBufferWriter               | 524288     | 23,240.2 ns | 437.47 ns | 468.09 ns | 23,039.2 ns |  0.95 |    0.02 |      - |     328 B |        1.08 |
