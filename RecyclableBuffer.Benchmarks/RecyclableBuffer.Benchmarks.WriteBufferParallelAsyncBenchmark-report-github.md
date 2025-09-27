```

BenchmarkDotNet v0.15.3, Linux CentOS Linux 7 (Core)
Intel Xeon CPU E5-2650 v2 2.60GHz (Max: 1.20GHz), 2 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2
  DefaultJob : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2


```
| Method                                         | BufferSize | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1    | Allocated | Alloc Ratio |
|----------------------------------------------- |----------- |----------:|----------:|----------:|------:|--------:|----------:|--------:|----------:|------------:|
| **SingleSegmentBufferWriter**                      | **1024**       |  **27.27 ms** |  **0.200 ms** |  **0.188 ms** |  **1.01** |    **0.02** |  **875.0000** | **31.2500** |   **8.77 MB** |        **0.86** |
| MultipleSegmentBufferWriter_Shared             | 1024       |  27.06 ms |  0.441 ms |  0.412 ms |  1.00 |    0.02 | 1031.2500 | 31.2500 |  10.22 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 1024       |  27.26 ms |  0.308 ms |  0.288 ms |  1.01 |    0.02 | 1031.2500 | 31.2500 |  10.22 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 1024       |  27.25 ms |  0.417 ms |  0.370 ms |  1.01 |    0.02 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 1024       |  26.11 ms |  0.372 ms |  0.348 ms |  0.97 |    0.02 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 1024       |  27.13 ms |  0.208 ms |  0.195 ms |  1.00 |    0.02 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 1024       |  26.02 ms |  0.229 ms |  0.203 ms |  0.96 |    0.02 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| Microsoft_RecyclableMemoryStream               | 1024       |  27.97 ms |  0.529 ms |  0.495 ms |  1.03 |    0.02 | 1062.5000 | 31.2500 |  10.68 MB |        1.04 |
| DotNext_PoolingArrayBufferWriter               | 1024       |  27.26 ms |  0.340 ms |  0.318 ms |  1.01 |    0.02 |  968.7500 | 31.2500 |   9.77 MB |        0.96 |
| DotNext_SparseBufferWriter                     | 1024       |  34.41 ms |  1.515 ms |  4.466 ms |  1.27 |    0.17 | 1000.0000 |       - |  10.15 MB |        0.99 |
|                                                |            |           |           |           |       |         |           |         |           |             |
| **SingleSegmentBufferWriter**                      | **8192**       |  **27.84 ms** |  **0.480 ms** |  **0.608 ms** |  **0.99** |    **0.03** |  **875.0000** | **31.2500** |   **8.77 MB** |        **0.86** |
| MultipleSegmentBufferWriter_Shared             | 8192       |  28.22 ms |  0.553 ms |  0.568 ms |  1.00 |    0.03 | 1031.2500 | 31.2500 |  10.22 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 8192       |  29.81 ms |  0.584 ms |  0.717 ms |  1.06 |    0.03 | 1031.2500 | 31.2500 |  10.22 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 8192       |  28.03 ms |  0.508 ms |  0.499 ms |  0.99 |    0.03 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 8192       |  27.76 ms |  0.459 ms |  0.510 ms |  0.98 |    0.03 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 8192       |  28.97 ms |  0.562 ms |  0.552 ms |  1.03 |    0.03 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 8192       |  28.08 ms |  0.306 ms |  0.287 ms |  1.00 |    0.02 | 1031.2500 | 31.2500 |  10.45 MB |        1.02 |
| Microsoft_RecyclableMemoryStream               | 8192       |  29.42 ms |  0.288 ms |  0.269 ms |  1.04 |    0.02 | 1062.5000 | 31.2500 |  10.68 MB |        1.04 |
| DotNext_PoolingArrayBufferWriter               | 8192       |  28.93 ms |  0.517 ms |  0.789 ms |  1.03 |    0.03 |  968.7500 | 31.2500 |   9.77 MB |        0.96 |
| DotNext_SparseBufferWriter                     | 8192       |  30.13 ms |  0.343 ms |  0.321 ms |  1.07 |    0.02 | 1000.0000 | 31.2500 |  10.15 MB |        0.99 |
|                                                |            |           |           |           |       |         |           |         |           |             |
| **SingleSegmentBufferWriter**                      | **524288**     | **684.74 ms** | **13.068 ms** | **13.420 ms** |  **0.97** |    **0.05** | **5000.0000** |       **-** |   **52.8 MB** |        **0.91** |
| MultipleSegmentBufferWriter_Shared             | 524288     | 705.83 ms | 14.077 ms | 33.998 ms |  1.00 |    0.07 | 5000.0000 |       - |  58.06 MB |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 524288     | 705.71 ms | 13.508 ms | 25.700 ms |  1.00 |    0.06 | 5000.0000 |       - |  58.06 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 524288     | 719.92 ms | 14.320 ms | 23.926 ms |  1.02 |    0.06 | 5000.0000 |       - |  58.29 MB |        1.00 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 524288     | 697.03 ms | 13.081 ms | 13.996 ms |  0.99 |    0.05 | 5000.0000 |       - |   59.2 MB |        1.02 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 524288     | 688.19 ms | 10.035 ms |  8.896 ms |  0.98 |    0.05 | 5000.0000 |       - |  60.29 MB |        1.04 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 524288     | 698.53 ms |  9.903 ms |  8.269 ms |  0.99 |    0.05 | 5000.0000 |       - |   61.2 MB |        1.05 |
| Microsoft_RecyclableMemoryStream               | 524288     | 708.05 ms |  3.952 ms |  3.085 ms |  1.01 |    0.05 | 6000.0000 |       - |  59.36 MB |        1.02 |
| DotNext_PoolingArrayBufferWriter               | 524288     | 684.77 ms | 13.409 ms | 13.169 ms |  0.97 |    0.05 | 5000.0000 |       - |  53.03 MB |        0.91 |
| DotNext_SparseBufferWriter                     | 524288     | 683.55 ms | 10.611 ms |  9.926 ms |  0.97 |    0.05 | 5000.0000 |       - |  58.67 MB |        1.01 |
