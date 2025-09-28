```

BenchmarkDotNet v0.15.3, Linux CentOS Linux 7 (Core)
Intel Xeon CPU E5-2650 v2 2.60GHz (Max: 1.20GHz), 2 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2
  DefaultJob : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2


```
| Method                                   | BufferSize | Mean      | Error     | StdDev     | Median    | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
|----------------------------------------- |----------- |----------:|----------:|-----------:|----------:|------:|--------:|---------:|----------:|------------:|
| **SingleSegmentBufferWriter**                | **1024**       |  **7.688 ms** | **0.1534 ms** |  **0.2477 ms** |  **7.647 ms** |  **0.75** |    **0.03** | **109.3750** |   **1.14 MB** |        **0.47** |
| MultipleSegmentBufferWriter_Shared       | 1024       | 10.296 ms | 0.1909 ms |  0.1785 ms | 10.300 ms |  1.00 |    0.02 | 234.3750 |   2.44 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 1024       | 13.240 ms | 0.3398 ms |  0.9966 ms | 13.174 ms |  1.29 |    0.10 | 234.3750 |   2.44 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable     | 1024       |  8.303 ms | 0.1600 ms |  0.1843 ms |  8.274 ms |  0.81 |    0.02 | 265.6250 |   2.67 MB |        1.09 |
| MultipleSegmentBufferWriter_FixedSize    | 1024       |  7.723 ms | 0.1529 ms |  0.2335 ms |  7.703 ms |  0.75 |    0.03 | 265.6250 |   2.67 MB |        1.09 |
| Microsoft_RecyclableMemoryStream         | 1024       |  8.611 ms | 0.1705 ms |  0.4184 ms |  8.667 ms |  0.84 |    0.04 | 265.6250 |   2.67 MB |        1.09 |
| DotNext_PoolingArrayBufferWriter         | 1024       |  7.421 ms | 0.3183 ms |  0.9385 ms |  7.789 ms |  0.72 |    0.09 | 234.3750 |   2.37 MB |        0.97 |
| DotNext_SparseBufferWriter               | 1024       | 14.343 ms | 0.1596 ms |  0.1493 ms | 14.300 ms |  1.39 |    0.03 | 187.5000 |   1.91 MB |        0.78 |
|                                          |            |           |           |            |           |       |         |          |           |             |
| **SingleSegmentBufferWriter**                | **8192**       |  **7.954 ms** | **0.1542 ms** |  **0.2308 ms** |  **7.959 ms** |  **0.94** |    **0.04** | **109.3750** |   **1.14 MB** |        **0.47** |
| MultipleSegmentBufferWriter_Shared       | 8192       |  8.458 ms | 0.1661 ms |  0.2101 ms |  8.436 ms |  1.00 |    0.03 | 242.1875 |   2.44 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 8192       | 12.095 ms | 0.2414 ms |  0.4593 ms | 12.233 ms |  1.43 |    0.06 | 218.7500 |   2.44 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable     | 8192       |  8.496 ms | 0.1695 ms |  0.2018 ms |  8.485 ms |  1.01 |    0.03 | 265.6250 |   2.67 MB |        1.09 |
| MultipleSegmentBufferWriter_FixedSize    | 8192       |  8.639 ms | 0.1684 ms |  0.2571 ms |  8.628 ms |  1.02 |    0.04 | 265.6250 |   2.67 MB |        1.09 |
| Microsoft_RecyclableMemoryStream         | 8192       |  7.636 ms | 0.4551 ms |  1.3420 ms |  7.669 ms |  0.90 |    0.16 | 265.6250 |   2.67 MB |        1.09 |
| DotNext_PoolingArrayBufferWriter         | 8192       |  8.382 ms | 0.1666 ms |  0.2918 ms |  8.461 ms |  0.99 |    0.04 | 234.3750 |   2.37 MB |        0.97 |
| DotNext_SparseBufferWriter               | 8192       | 14.236 ms | 0.2762 ms |  0.3872 ms | 14.289 ms |  1.68 |    0.06 | 187.5000 |   1.91 MB |        0.78 |
|                                          |            |           |           |            |           |       |         |          |           |             |
| **SingleSegmentBufferWriter**                | **524288**     | **37.158 ms** | **1.1486 ms** |  **3.3140 ms** | **36.412 ms** |  **1.93** |    **0.29** | **153.8462** |   **1.91 MB** |        **0.53** |
| MultipleSegmentBufferWriter_Shared       | 524288     | 19.532 ms | 0.7760 ms |  2.2879 ms | 19.618 ms |  1.01 |    0.17 | 343.7500 |   3.59 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable | 524288     | 87.150 ms | 1.7399 ms |  4.6139 ms | 87.786 ms |  4.52 |    0.59 | 333.3333 |   3.59 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable     | 524288     | 86.044 ms | 2.1849 ms |  6.2336 ms | 84.939 ms |  4.47 |    0.62 | 333.3333 |   4.73 MB |        1.32 |
| MultipleSegmentBufferWriter_FixedSize    | 524288     | 89.903 ms | 4.8752 ms | 14.3746 ms | 97.295 ms |  4.67 |    0.93 | 375.0000 |   5.44 MB |        1.52 |
| Microsoft_RecyclableMemoryStream         | 524288     | 61.151 ms | 1.5882 ms |  4.6579 ms | 61.706 ms |  3.17 |    0.45 | 416.6667 |    4.2 MB |        1.17 |
| DotNext_PoolingArrayBufferWriter         | 524288     | 42.644 ms | 0.7339 ms |  0.8737 ms | 42.226 ms |  2.21 |    0.27 | 200.0000 |   2.37 MB |        0.66 |
| DotNext_SparseBufferWriter               | 524288     | 28.558 ms | 0.8717 ms |  2.5150 ms | 27.998 ms |  1.48 |    0.22 | 343.7500 |   3.66 MB |        1.02 |
