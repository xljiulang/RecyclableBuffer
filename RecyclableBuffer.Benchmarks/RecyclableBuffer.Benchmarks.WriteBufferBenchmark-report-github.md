```

BenchmarkDotNet v0.15.3, Linux CentOS Linux 7 (Core)
Intel Xeon CPU E5-2650 v2 2.60GHz (Max: 1.20GHz), 2 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2
  DefaultJob : .NET 8.0.0 (8.0.0, 8.0.23.53103), X64 RyuJIT x86-64-v2


```
| Method                                         | BufferSize | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------------------------------- |----------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**                      | **1024**       |    **218.2 ns** |   **2.32 ns** |   **2.17 ns** |  **0.84** |    **0.01** | **0.0076** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared             | 1024       |    260.2 ns |   3.22 ns |   2.69 ns |  1.00 |    0.01 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 1024       |    259.9 ns |   2.78 ns |   2.32 ns |  1.00 |    0.01 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 1024       |    272.0 ns |   4.98 ns |   3.89 ns |  1.05 |    0.02 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 1024       |    245.9 ns |   3.54 ns |   2.76 ns |  0.95 |    0.01 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 1024       |    256.5 ns |   3.27 ns |   2.73 ns |  0.99 |    0.01 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 1024       |    267.2 ns |   4.04 ns |   3.38 ns |  1.03 |    0.02 | 0.0196 |     208 B |        1.13 |
| Microsoft_RecyclableMemoryStream               | 1024       |  1,131.7 ns |  17.07 ns |  15.13 ns |  4.35 |    0.07 | 0.0267 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter               | 1024       |    517.1 ns |  10.06 ns |  12.35 ns |  1.99 |    0.05 | 0.0172 |     184 B |        1.00 |
| DotNext_SparseBufferWriter                     | 1024       |    339.1 ns |   6.81 ns |   8.62 ns |  1.30 |    0.03 | 0.0129 |     136 B |        0.74 |
|                                                |            |             |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**                      | **8192**       |    **429.1 ns** |   **7.24 ns** |   **6.42 ns** |  **0.97** |    **0.02** | **0.0076** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared             | 8192       |    443.6 ns |   5.32 ns |   4.97 ns |  1.00 |    0.02 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 8192       |    491.7 ns |   9.05 ns |   8.03 ns |  1.11 |    0.02 | 0.0172 |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 8192       |    465.4 ns |   6.45 ns |   5.39 ns |  1.05 |    0.02 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 8192       |    450.5 ns |   8.77 ns |  11.41 ns |  1.02 |    0.03 | 0.0196 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 8192       |    429.3 ns |   5.66 ns |   4.73 ns |  0.97 |    0.01 | 0.0191 |     208 B |        1.13 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 8192       |    441.7 ns |   8.82 ns |  11.47 ns |  1.00 |    0.03 | 0.0196 |     208 B |        1.13 |
| Microsoft_RecyclableMemoryStream               | 8192       |  1,534.1 ns |  18.12 ns |  22.25 ns |  3.46 |    0.06 | 0.0267 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter               | 8192       |  1,329.8 ns |  25.72 ns |  25.26 ns |  3.00 |    0.06 | 0.0172 |     184 B |        1.00 |
| DotNext_SparseBufferWriter                     | 8192       |    570.5 ns |  10.96 ns |  13.05 ns |  1.29 |    0.03 | 0.0124 |     136 B |        0.74 |
|                                                |            |             |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**                      | **524288**     | **52,893.5 ns** | **570.27 ns** | **533.43 ns** |  **2.26** |    **0.03** |      **-** |     **160 B** |        **0.53** |
| MultipleSegmentBufferWriter_Shared             | 524288     | 23,380.9 ns | 273.96 ns | 242.86 ns |  1.00 |    0.01 |      - |     304 B |        1.00 |
| MultipleSegmentBufferWriter_Configurable       | 524288     | 23,467.4 ns | 405.55 ns | 359.51 ns |  1.00 |    0.02 |      - |     304 B |        1.00 |
| MultipleSegmentBufferWriter_Scalable_SpinLock  | 524288     | 23,223.0 ns | 351.00 ns | 293.10 ns |  0.99 |    0.02 | 0.0305 |     328 B |        1.08 |
| MultipleSegmentBufferWriter_Scalable_Stack     | 524288     | 23,562.5 ns | 464.66 ns | 849.66 ns |  1.01 |    0.04 |      - |     424 B |        1.39 |
| MultipleSegmentBufferWriter_FixedSize_SpinLock | 524288     | 23,327.0 ns | 438.31 ns | 409.99 ns |  1.00 |    0.02 | 0.0305 |     328 B |        1.08 |
| MultipleSegmentBufferWriter_FixedSize_Stack    | 524288     | 24,011.0 ns | 331.36 ns | 258.70 ns |  1.03 |    0.01 | 0.0305 |     424 B |        1.39 |
| Microsoft_RecyclableMemoryStream               | 524288     | 25,173.0 ns | 317.44 ns | 265.08 ns |  1.08 |    0.02 |      - |     480 B |        1.58 |
| DotNext_PoolingArrayBufferWriter               | 524288     | 62,590.3 ns | 963.78 ns | 752.46 ns |  2.68 |    0.04 |      - |     184 B |        0.61 |
| DotNext_SparseBufferWriter                     | 524288     | 24,654.2 ns | 230.11 ns | 179.66 ns |  1.05 |    0.01 | 0.0305 |     328 B |        1.08 |
