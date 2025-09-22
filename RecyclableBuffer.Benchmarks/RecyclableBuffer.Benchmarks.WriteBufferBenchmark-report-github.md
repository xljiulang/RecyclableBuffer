```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
Intel Core i7-8565U CPU 1.80GHz (Max: 1.99GHz) (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                      | BufferSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**   | **1024**       |   **132.5 ns** |   **2.65 ns** |   **3.15 ns** |  **0.89** |    **0.03** | **0.0191** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter | 1024       |   149.7 ns |   3.02 ns |   4.70 ns |  1.00 |    0.04 | 0.0420 |     176 B |        1.00 |
| RecyclableMemoryStream      | 1024       |   405.3 ns |  16.78 ns |  47.89 ns |  2.71 |    0.33 | 0.0668 |     280 B |        1.59 |
|                             |            |            |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**   | **8192**       |   **224.3 ns** |   **4.51 ns** |   **8.90 ns** |  **0.84** |    **0.05** | **0.0191** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter | 8192       |   267.4 ns |   5.38 ns |  11.81 ns |  1.00 |    0.06 | 0.0420 |     176 B |        1.00 |
| RecyclableMemoryStream      | 8192       |   467.6 ns |   9.05 ns |  11.44 ns |  1.75 |    0.09 | 0.0668 |     280 B |        1.59 |
|                             |            |            |           |           |       |         |        |           |             |
| **SingleSegmentBufferWriter**   | **131073**     | **9,217.2 ns** | **270.79 ns** | **768.19 ns** |  **1.97** |    **0.18** | **0.0153** |     **126 B** |        **0.58** |
| MultipleSegmentBufferWriter | 131073     | 4,675.8 ns |  91.93 ns | 161.00 ns |  1.00 |    0.05 | 0.0458 |     216 B |        1.00 |
| RecyclableMemoryStream      | 131073     | 5,555.9 ns | 194.58 ns | 570.67 ns |  1.19 |    0.13 | 0.0687 |     312 B |        1.44 |
