```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                         | BufferLength | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_10** | **1024**         | **209.4 ns** |  **3.98 ns** |  **4.26 ns** |  **1.00** |    **0.03** | **0.0293** |     **184 B** |        **1.00** |
| RecyclableMemoryStream_10      | 1024         | 359.5 ns |  7.14 ns |  9.53 ns |  1.72 |    0.06 | 0.0443 |     280 B |        1.52 |
|                                |              |          |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10** | **2048**         | **398.5 ns** |  **1.75 ns** |  **1.46 ns** |  **1.00** |    **0.00** | **0.0291** |     **184 B** |        **1.00** |
| RecyclableMemoryStream_10      | 2048         | 521.9 ns |  9.50 ns |  8.89 ns |  1.31 |    0.02 | 0.0439 |     280 B |        1.52 |
|                                |              |          |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10** | **4096**         | **704.8 ns** | **13.13 ns** | **11.64 ns** |  **1.00** |    **0.02** | **0.0286** |     **184 B** |        **1.00** |
| RecyclableMemoryStream_10      | 4096         | 837.4 ns |  8.55 ns |  7.58 ns |  1.19 |    0.02 | 0.0439 |     280 B |        1.52 |
