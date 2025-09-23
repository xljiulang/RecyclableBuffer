```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                           | BufferSize | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------------------- |----------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **1024**       |    **57.66 ns** |  **0.465 ns** |  **0.388 ns** |  **0.64** |    **0.01** | **0.0127** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 1024       |    90.05 ns |  1.752 ns |  2.018 ns |  1.00 |    0.03 | 0.0280 |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 1024       |   196.16 ns |  2.367 ns |  2.214 ns |  2.18 |    0.05 | 0.0446 |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 1024       |   228.95 ns |  2.211 ns |  1.960 ns |  2.54 |    0.06 | 0.0293 |     184 B |        1.05 |
| DotNext_SparseBufferWriter       | 1024       |   420.19 ns |  8.115 ns |  7.591 ns |  4.67 |    0.13 | 0.0353 |     224 B |        1.27 |
|                                  |            |             |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **8192**       |   **107.89 ns** |  **1.423 ns** |  **1.331 ns** |  **0.83** |    **0.03** | **0.0126** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 8192       |   130.70 ns |  2.641 ns |  4.762 ns |  1.00 |    0.05 | 0.0279 |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192       |   244.26 ns |  3.472 ns |  3.248 ns |  1.87 |    0.07 | 0.0443 |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 8192       |   399.21 ns |  7.549 ns |  9.271 ns |  3.06 |    0.13 | 0.0291 |     184 B |        1.05 |
| DotNext_SparseBufferWriter       | 8192       |   462.48 ns |  9.120 ns | 11.534 ns |  3.54 |    0.16 | 0.0496 |     312 B |        1.77 |
|                                  |            |             |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**        | **131073**     | **4,625.54 ns** | **10.573 ns** |  **9.372 ns** |  **1.88** |    **0.04** | **0.0153** |     **120 B** |        **0.56** |
| MultipleSegmentBufferWriter      | 131073     | 2,456.49 ns | 46.496 ns | 49.750 ns |  1.00 |    0.03 | 0.0343 |     216 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 131073     | 5,909.63 ns | 12.737 ns | 11.914 ns |  2.41 |    0.05 | 0.0458 |     312 B |        1.44 |
| DotNext_PoolingArrayBufferWriter | 131073     | 7,046.80 ns | 16.800 ns | 15.715 ns |  2.87 |    0.06 | 0.0229 |     184 B |        0.85 |
| DotNext_SparseBufferWriter       | 131073     | 5,484.15 ns | 28.729 ns | 26.873 ns |  2.23 |    0.04 | 0.4807 |    3040 B |       14.07 |
