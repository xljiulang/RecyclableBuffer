```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                    | BufferLength | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------- |------------- |---------:|---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **RecyclableBuffer_10**       | **1024**         | **209.7 ns** |  **2.17 ns** |  **1.92 ns** | **209.0 ns** |  **1.00** |    **0.01** | **0.0267** |     **168 B** |        **1.00** |
| RecyclableMemoryStream_10 | 1024         | 331.3 ns |  5.88 ns |  5.50 ns | 329.4 ns |  1.58 |    0.03 | 0.0443 |     280 B |        1.67 |
|                           |              |          |          |          |          |       |         |        |           |             |
| **RecyclableBuffer_10**       | **2048**         | **381.0 ns** |  **5.04 ns** |  **4.72 ns** | **381.3 ns** |  **1.00** |    **0.02** | **0.0267** |     **168 B** |        **1.00** |
| RecyclableMemoryStream_10 | 2048         | 503.5 ns | 10.06 ns | 25.43 ns | 492.7 ns |  1.32 |    0.07 | 0.0439 |     280 B |        1.67 |
|                           |              |          |          |          |          |       |         |        |           |             |
| **RecyclableBuffer_10**       | **4096**         | **643.7 ns** |  **3.86 ns** |  **3.61 ns** | **643.8 ns** |  **1.00** |    **0.01** | **0.0267** |     **169 B** |        **1.00** |
| RecyclableMemoryStream_10 | 4096         | 818.8 ns |  5.90 ns |  5.23 ns | 818.4 ns |  1.27 |    0.01 | 0.0439 |     280 B |        1.66 |
