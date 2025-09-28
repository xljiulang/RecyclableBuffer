```

BenchmarkDotNet v0.15.3, Linux CentOS Linux 7 (Core)
Intel Xeon CPU E5-2650 v2 2.60GHz (Max: 1.20GHz), 2 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2
  DefaultJob : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2


```
| Method                                   | BufferSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1    | Allocated | Alloc Ratio |
|----------------------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|---------:|--------:|----------:|------------:|
| **SingleSegmentBufferWriter**                | **1024**       |   **7.360 ms** | **0.1451 ms** | **0.3029 ms** |  **1.00** |    **0.04** | **312.5000** |       **-** |   **3.13 MB** |        **0.69** |
| MultipleSegmentBufferWriter_Shared       | 1024       |   7.357 ms | 0.0662 ms | 0.0587 ms |  1.00 |    0.01 | 460.9375 | 15.6250 |   4.56 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 1024       |  13.025 ms | 0.1332 ms | 0.1246 ms |  1.77 |    0.02 | 437.5000 |       - |   4.42 MB |        0.97 |
| MultipleSegmentBufferWriter_Scalable     | 1024       |   9.905 ms | 0.1979 ms | 0.3360 ms |  1.35 |    0.05 | 468.7500 | 15.6250 |   4.63 MB |        1.02 |
| MultipleSegmentBufferWriter_FixedSize    | 1024       |   8.344 ms | 0.1621 ms | 0.2219 ms |  1.13 |    0.03 | 468.7500 | 15.6250 |   4.76 MB |        1.04 |
| Microsoft_RecyclableMemoryStream         | 1024       |   8.409 ms | 0.0406 ms | 0.0339 ms |  1.14 |    0.01 | 468.7500 | 15.6250 |   4.65 MB |        1.02 |
| DotNext_PoolingArrayBufferWriter         | 1024       |   7.805 ms | 0.1550 ms | 0.3714 ms |  1.06 |    0.05 | 437.5000 |       - |   4.41 MB |        0.97 |
| DotNext_SparseBufferWriter               | 1024       |  19.982 ms | 0.2876 ms | 0.2550 ms |  2.72 |    0.04 | 375.0000 |       - |   3.93 MB |        0.86 |
|                                          |            |            |           |           |       |         |          |         |           |             |
| **SingleSegmentBufferWriter**                | **8192**       |   **8.580 ms** | **0.1636 ms** | **0.3073 ms** |  **0.93** |    **0.04** | **328.1250** |       **-** |   **3.26 MB** |        **0.71** |
| MultipleSegmentBufferWriter_Shared       | 8192       |   9.246 ms | 0.1828 ms | 0.1710 ms |  1.00 |    0.03 | 453.1250 |       - |   4.56 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 8192       |  13.194 ms | 0.1775 ms | 0.1482 ms |  1.43 |    0.03 | 437.5000 |       - |   4.43 MB |        0.97 |
| MultipleSegmentBufferWriter_Scalable     | 8192       |  10.994 ms | 0.2025 ms | 0.2772 ms |  1.19 |    0.04 | 468.7500 | 15.6250 |   4.64 MB |        1.02 |
| MultipleSegmentBufferWriter_FixedSize    | 8192       |   8.220 ms | 0.0952 ms | 0.0891 ms |  0.89 |    0.02 | 468.7500 | 15.6250 |   4.68 MB |        1.03 |
| Microsoft_RecyclableMemoryStream         | 8192       |  10.419 ms | 0.0350 ms | 0.0310 ms |  1.13 |    0.02 | 468.7500 | 15.6250 |   4.64 MB |        1.02 |
| DotNext_PoolingArrayBufferWriter         | 8192       |   8.554 ms | 0.1685 ms | 0.2907 ms |  0.93 |    0.04 | 453.1250 |       - |   4.43 MB |        0.97 |
| DotNext_SparseBufferWriter               | 8192       |  20.668 ms | 0.3960 ms | 0.4863 ms |  2.24 |    0.07 | 375.0000 |       - |   3.94 MB |        0.87 |
|                                          |            |            |           |           |       |         |          |         |           |             |
| **SingleSegmentBufferWriter**                | **524288**     | **102.945 ms** | **2.0525 ms** | **5.3709 ms** |  **1.20** |    **0.08** | **333.3333** |       **-** |   **3.99 MB** |        **0.70** |
| MultipleSegmentBufferWriter_Shared       | 524288     |  85.754 ms | 1.6903 ms | 3.1748 ms |  1.00 |    0.05 | 500.0000 |       - |   5.67 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 524288     | 125.701 ms | 3.1974 ms | 9.4277 ms |  1.47 |    0.12 | 333.3333 |       - |   5.69 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable     | 524288     | 121.760 ms | 2.3682 ms | 3.6869 ms |  1.42 |    0.07 | 666.6667 |       - |   6.84 MB |        1.21 |
| MultipleSegmentBufferWriter_FixedSize    | 524288     | 123.204 ms | 2.4321 ms | 5.7801 ms |  1.44 |    0.09 | 333.3333 |       - |   8.45 MB |        1.49 |
| Microsoft_RecyclableMemoryStream         | 524288     | 107.998 ms | 2.6547 ms | 7.8274 ms |  1.26 |    0.10 | 500.0000 |       - |   6.29 MB |        1.11 |
| DotNext_PoolingArrayBufferWriter         | 524288     | 116.044 ms | 2.1074 ms | 5.6251 ms |  1.36 |    0.08 | 333.3333 |       - |   4.46 MB |        0.79 |
| DotNext_SparseBufferWriter               | 524288     |  91.291 ms | 2.0708 ms | 6.1059 ms |  1.07 |    0.08 | 500.0000 |       - |   5.74 MB |        1.01 |
