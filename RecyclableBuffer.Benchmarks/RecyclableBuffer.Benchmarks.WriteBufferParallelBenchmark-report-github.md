```

BenchmarkDotNet v0.15.3, Linux CentOS Linux 7 (Core)
Intel Xeon CPU E5-2650 v2 2.60GHz (Max: 1.20GHz), 2 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2
  DefaultJob : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2


```
| Method                                         | BufferSize | Mean      | Error     | StdDev     | Median    | Ratio | RatioSD | Gen0     | Allocated  | Alloc Ratio |
|----------------------------------------------- |----------- |----------:|----------:|-----------:|----------:|------:|--------:|---------:|-----------:|------------:|
| **SingleSegmentBufferWriter**                      | **1024**       | **12.016 ms** | **0.2332 ms** |  **0.2067 ms** | **12.026 ms** |  **0.93** |    **0.02** |  **62.5000** |   **781.7 KB** |        **0.34** |
| MultipleSegmentBufferWriter_Shared             | 1024       | 12.920 ms | 0.2263 ms |  0.2117 ms | 12.959 ms |  1.00 |    0.02 | 218.7500 | 2266.08 KB |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 1024       | 10.704 ms | 0.7518 ms |  2.2166 ms | 10.898 ms |  0.83 |    0.17 | 218.7500 | 2266.08 KB |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 1024       | 12.652 ms | 0.2298 ms |  0.2149 ms | 12.695 ms |  0.98 |    0.02 | 234.3750 | 2500.45 KB |        1.10 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 1024       | 12.375 ms | 0.2458 ms |  0.3364 ms | 12.389 ms |  0.96 |    0.03 | 234.3750 | 2500.45 KB |        1.10 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 1024       | 12.403 ms | 0.2417 ms |  0.3904 ms | 12.408 ms |  0.96 |    0.03 | 234.3750 | 2500.45 KB |        1.10 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 1024       | 12.781 ms | 0.3585 ms |  1.0571 ms | 12.864 ms |  0.99 |    0.08 | 234.3750 | 2500.45 KB |        1.10 |
| Microsoft_RecyclableMemoryStream               | 1024       |  6.029 ms | 0.4165 ms |  1.2280 ms |  5.751 ms |  0.47 |    0.09 | 265.6250 | 2734.83 KB |        1.21 |
| DotNext_PoolingArrayBufferWriter               | 1024       | 10.320 ms | 0.1989 ms |  0.2586 ms | 10.328 ms |  0.80 |    0.02 | 171.8750 | 1797.37 KB |        0.79 |
| DotNext_SparseBufferWriter                     | 1024       | 21.772 ms | 0.8898 ms |  2.6236 ms | 22.638 ms |  1.69 |    0.20 | 187.5000 | 2187.95 KB |        0.97 |
|                                                |            |           |           |            |           |       |         |          |            |             |
| **SingleSegmentBufferWriter**                      | **8192**       | **11.517 ms** | **0.2295 ms** |  **0.4940 ms** | **11.510 ms** |  **0.98** |    **0.09** |  **62.5000** |   **781.7 KB** |        **0.34** |
| MultipleSegmentBufferWriter_Shared             | 8192       | 11.814 ms | 0.3194 ms |  0.9417 ms | 11.930 ms |  1.01 |    0.12 | 218.7500 | 2266.08 KB |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 8192       | 13.297 ms | 0.2625 ms |  0.5870 ms | 13.387 ms |  1.13 |    0.11 | 218.7500 | 2266.08 KB |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 8192       | 12.592 ms | 0.2422 ms |  0.2487 ms | 12.564 ms |  1.07 |    0.09 | 234.3750 | 2500.45 KB |        1.10 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 8192       | 10.162 ms | 0.2002 ms |  0.4479 ms | 10.180 ms |  0.87 |    0.08 | 234.3750 | 2500.45 KB |        1.10 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 8192       | 12.064 ms | 0.2362 ms |  0.4320 ms | 12.165 ms |  1.03 |    0.10 | 234.3750 | 2500.45 KB |        1.10 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 8192       | 13.030 ms | 0.2551 ms |  0.2505 ms | 13.083 ms |  1.11 |    0.10 | 234.3750 | 2500.45 KB |        1.10 |
| Microsoft_RecyclableMemoryStream               | 8192       |  9.756 ms | 0.2625 ms |  0.7700 ms |  9.747 ms |  0.83 |    0.10 | 265.6250 | 2734.83 KB |        1.21 |
| DotNext_PoolingArrayBufferWriter               | 8192       | 11.575 ms | 0.2969 ms |  0.8755 ms | 11.635 ms |  0.99 |    0.11 | 171.8750 | 1797.37 KB |        0.79 |
| DotNext_SparseBufferWriter                     | 8192       | 21.954 ms | 0.4340 ms |  0.7251 ms | 22.121 ms |  1.87 |    0.17 | 187.5000 | 2187.95 KB |        0.97 |
|                                                |            |           |           |            |           |       |         |          |            |             |
| **SingleSegmentBufferWriter**                      | **524288**     | **42.986 ms** | **0.4519 ms** |  **0.4006 ms** | **43.014 ms** |  **2.03** |    **0.15** |  **90.9091** | **1562.95 KB** |        **0.32** |
| MultipleSegmentBufferWriter_Shared             | 524288     | 21.249 ms | 0.5142 ms |  1.4999 ms | 20.979 ms |  1.01 |    0.10 | 468.7500 |  4844.2 KB |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 524288     | 79.803 ms | 4.3364 ms | 12.7859 ms | 83.571 ms |  3.77 |    0.66 | 333.3333 |  4844.2 KB |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 524288     | 84.549 ms | 3.5600 ms | 10.4967 ms | 85.557 ms |  4.00 |    0.57 | 428.5714 | 5078.58 KB |        1.05 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 524288     | 86.869 ms | 4.7026 ms | 13.7919 ms | 87.757 ms |  4.11 |    0.71 | 500.0000 | 6016.08 KB |        1.24 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 524288     | 83.358 ms | 3.0626 ms |  8.9821 ms | 84.910 ms |  3.94 |    0.51 | 444.4444 | 6529.51 KB |        1.35 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 524288     | 79.888 ms | 4.6782 ms | 13.7938 ms | 80.869 ms |  3.78 |    0.71 | 500.0000 | 7449.59 KB |        1.54 |
| Microsoft_RecyclableMemoryStream               | 524288     | 67.703 ms | 1.7083 ms |  5.0371 ms | 67.781 ms |  3.20 |    0.33 | 500.0000 | 6172.33 KB |        1.27 |
| DotNext_PoolingArrayBufferWriter               | 524288     | 47.412 ms | 0.9469 ms |  1.3880 ms | 48.170 ms |  2.24 |    0.17 | 100.0000 | 1797.37 KB |        0.37 |
| DotNext_SparseBufferWriter                     | 524288     | 27.734 ms | 0.5434 ms |  0.9079 ms | 27.436 ms |  1.31 |    0.10 | 531.2500 |  5473.2 KB |        1.13 |
