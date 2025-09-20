```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                         | BufferLength | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_10** | **1024**         | **182.9 ns** |  **3.62 ns** |  **3.38 ns** |  **1.00** |    **0.03** | **0.0279** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_10      | 1024         | 338.9 ns |  2.51 ns |  2.35 ns |  1.85 |    0.04 | 0.0443 |     280 B |        1.59 |
|                                |              |          |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10** | **2048**         | **316.2 ns** |  **3.44 ns** |  **3.21 ns** |  **1.00** |    **0.01** | **0.0277** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_10      | 2048         | 510.0 ns |  9.67 ns |  9.93 ns |  1.61 |    0.03 | 0.0439 |     280 B |        1.59 |
|                                |              |          |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10** | **4096**         | **682.6 ns** | **12.74 ns** | **12.51 ns** |  **1.00** |    **0.02** | **0.0277** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_10      | 4096         | 827.0 ns |  6.80 ns |  5.68 ns |  1.21 |    0.02 | 0.0439 |     280 B |        1.59 |
