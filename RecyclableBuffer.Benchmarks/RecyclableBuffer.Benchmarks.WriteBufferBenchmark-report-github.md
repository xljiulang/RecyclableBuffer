```

BenchmarkDotNet v0.15.3, Linux CentOS Linux 7 (Core)
Intel Xeon CPU E5-2650 v2 2.60GHz (Max: 1.20GHz), 2 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2
  DefaultJob : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2


```
| Method                                         | BufferSize | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------------------------------- |----------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**                      | **1024**       |    **225.6 ns** |   **1.97 ns** |   **1.85 ns** |    **225.3 ns** |  **0.78** |    **0.01** | **0.0076** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared             | 1024       |    289.9 ns |   5.57 ns |   5.21 ns |    288.3 ns |  1.00 |    0.02 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 1024       |    275.8 ns |   5.22 ns |   5.58 ns |    275.3 ns |  0.95 |    0.02 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 1024       |    263.5 ns |   5.24 ns |   8.76 ns |    261.0 ns |  0.91 |    0.03 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 1024       |    266.7 ns |   5.30 ns |   6.51 ns |    264.7 ns |  0.92 |    0.03 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 1024       |    257.8 ns |   3.58 ns |   2.99 ns |    258.0 ns |  0.89 |    0.02 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 1024       |    257.0 ns |   3.11 ns |   2.75 ns |    256.1 ns |  0.89 |    0.02 | 0.0196 |     208 B |        1.13 |
| Microsoft_RecyclableMemoryStream               | 1024       |  1,165.1 ns |  20.81 ns |  23.13 ns |  1,159.4 ns |  4.02 |    0.10 | 0.0267 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter               | 1024       |    538.5 ns |  10.63 ns |  19.70 ns |    536.8 ns |  1.86 |    0.07 | 0.0172 |     184 B |        1.00 |
| DotNext_SparseBufferWriter                     | 1024       |    357.3 ns |   7.08 ns |  11.43 ns |    354.9 ns |  1.23 |    0.04 | 0.0129 |     136 B |        0.74 |
|                                                |            |             |           |           |             |       |         |        |           |             |
| **SingleSegmentBufferWriter**                      | **8192**       |    **423.8 ns** |   **5.01 ns** |   **4.44 ns** |    **422.7 ns** |  **0.86** |    **0.02** | **0.0076** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared             | 8192       |    494.9 ns |   9.72 ns |  10.39 ns |    492.8 ns |  1.00 |    0.03 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 8192       |    486.2 ns |   9.72 ns |  21.13 ns |    480.6 ns |  0.98 |    0.05 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 8192       |    459.7 ns |   7.31 ns |   6.10 ns |    459.6 ns |  0.93 |    0.02 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 8192       |    472.8 ns |   9.45 ns |  17.04 ns |    468.6 ns |  0.96 |    0.04 | 0.0191 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 8192       |    465.5 ns |   6.60 ns |   8.10 ns |    464.4 ns |  0.94 |    0.03 | 0.0191 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 8192       |    472.3 ns |   8.53 ns |  20.44 ns |    464.1 ns |  0.95 |    0.05 | 0.0191 |     208 B |        1.13 |
| Microsoft_RecyclableMemoryStream               | 8192       |  1,531.2 ns |  24.55 ns |  19.17 ns |  1,530.7 ns |  3.10 |    0.07 | 0.0267 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter               | 8192       |  1,186.8 ns |  23.51 ns |  35.20 ns |  1,169.3 ns |  2.40 |    0.09 | 0.0172 |     184 B |        1.00 |
| DotNext_SparseBufferWriter                     | 8192       |    579.2 ns |  10.99 ns |  11.28 ns |    576.6 ns |  1.17 |    0.03 | 0.0124 |     136 B |        0.74 |
|                                                |            |             |           |           |             |       |         |        |           |             |
| **SingleSegmentBufferWriter**                      | **524288**     | **50,762.4 ns** | **460.84 ns** | **431.07 ns** | **50,795.8 ns** |  **2.16** |    **0.03** |      **-** |     **160 B** |        **0.53** |
| MultipleSegmentBufferWriter_Shared             | 524288     | 23,543.4 ns | 257.52 ns | 201.05 ns | 23,565.1 ns |  1.00 |    0.01 |      - |     304 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 524288     | 22,293.0 ns | 365.46 ns | 305.18 ns | 22,346.6 ns |  0.95 |    0.01 |      - |     304 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 524288     | 23,190.7 ns | 345.01 ns | 288.10 ns | 23,183.1 ns |  0.99 |    0.01 | 0.0305 |     328 B |        1.08 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 524288     | 23,411.8 ns | 291.33 ns | 258.26 ns | 23,432.3 ns |  0.99 |    0.01 | 0.0305 |     328 B |        1.08 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 524288     | 22,720.2 ns | 253.44 ns | 211.63 ns | 22,810.3 ns |  0.97 |    0.01 | 0.0305 |     424 B |        1.39 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 524288     | 23,203.3 ns | 438.22 ns | 409.91 ns | 23,172.2 ns |  0.99 |    0.02 | 0.0305 |     424 B |        1.39 |
| Microsoft_RecyclableMemoryStream               | 524288     | 23,428.8 ns | 458.29 ns | 595.90 ns | 23,226.5 ns |  1.00 |    0.03 | 0.0305 |     480 B |        1.58 |
| DotNext_PoolingArrayBufferWriter               | 524288     | 61,469.5 ns | 708.27 ns | 627.86 ns | 61,551.5 ns |  2.61 |    0.03 |      - |     184 B |        0.61 |
| DotNext_SparseBufferWriter                     | 524288     | 22,133.0 ns | 364.66 ns | 341.10 ns | 22,052.6 ns |  0.94 |    0.02 | 0.0305 |     328 B |        1.08 |
