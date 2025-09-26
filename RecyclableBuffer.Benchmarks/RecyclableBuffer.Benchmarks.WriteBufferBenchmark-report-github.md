```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
Intel Core i7-8565U CPU 1.80GHz (Max: 1.99GHz) (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                           | BufferSize | Mean        | Error       | StdDev      | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------------------- |----------- |------------:|------------:|------------:|------------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **1024**       |    **154.7 ns** |     **5.71 ns** |    **16.83 ns** |    **151.7 ns** |  **1.11** |    **0.14** | **0.0191** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 1024       |    140.6 ns |     3.87 ns |    10.92 ns |    137.7 ns |  1.01 |    0.11 | 0.0439 |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 1024       |    317.8 ns |     5.25 ns |     4.65 ns |    317.7 ns |  2.27 |    0.17 | 0.0668 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 1024       |    321.0 ns |     6.40 ns |     8.32 ns |    320.7 ns |  2.30 |    0.17 | 0.0439 |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 1024       |    692.7 ns |    16.99 ns |    49.57 ns |    686.7 ns |  4.95 |    0.50 | 0.0534 |     224 B |        1.22 |
|                                  |            |             |             |             |             |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **8192**       |    **186.9 ns** |     **2.69 ns** |     **2.24 ns** |    **186.7 ns** |  **0.86** |    **0.05** | **0.0191** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 8192       |    218.4 ns |     5.14 ns |    14.57 ns |    212.7 ns |  1.00 |    0.09 | 0.0439 |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192       |    408.2 ns |     8.04 ns |    11.78 ns |    405.9 ns |  1.88 |    0.13 | 0.0668 |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 8192       |    693.2 ns |    13.31 ns |    12.45 ns |    691.9 ns |  3.19 |    0.20 | 0.0439 |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 8192       |    959.1 ns |    14.02 ns |    13.77 ns |    956.8 ns |  4.41 |    0.28 | 0.0744 |     312 B |        1.70 |
|                                  |            |             |             |             |             |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **524288**     | **30,841.5 ns** |   **650.77 ns** | **1,918.81 ns** | **30,288.9 ns** |  **1.79** |    **0.13** | **0.0305** |     **160 B** |        **0.53** |
| MultipleSegmentBufferWriter      | 524288     | 17,217.1 ns |   337.47 ns |   689.36 ns | 17,138.9 ns |  1.00 |    0.06 | 0.0610 |     304 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 524288     | 17,478.3 ns |   349.69 ns |   992.01 ns | 17,310.9 ns |  1.02 |    0.07 | 0.0916 |     480 B |        1.58 |
| DotNext_PoolingArrayBufferWriter | 524288     | 33,636.5 ns |   662.30 ns | 1,106.55 ns | 33,515.2 ns |  1.96 |    0.10 |      - |     184 B |        0.61 |
| DotNext_SparseBufferWriter       | 524288     | 48,342.4 ns | 1,074.89 ns | 3,049.30 ns | 47,667.0 ns |  2.81 |    0.21 | 2.6855 |   11400 B |       37.50 |
