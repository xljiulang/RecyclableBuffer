```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                    | BufferLength | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------- |------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **RecyclableBuffer_10**       | **1024**         | **189.5 ns** |  **2.99 ns** |  **2.79 ns** |  **1.00** |    **0.02** | **0.0293** |     **184 B** |        **1.00** |
| RecyclableMemoryStream_10 | 1024         | 333.1 ns |  3.93 ns |  3.48 ns |  1.76 |    0.03 | 0.0443 |     280 B |        1.52 |
|                           |              |          |          |          |       |         |        |           |             |
| **RecyclableBuffer_10**       | **2048**         | **372.7 ns** |  **7.45 ns** |  **8.29 ns** |  **1.00** |    **0.03** | **0.0291** |     **184 B** |        **1.00** |
| RecyclableMemoryStream_10 | 2048         | 490.5 ns |  5.10 ns |  4.52 ns |  1.32 |    0.03 | 0.0439 |     280 B |        1.52 |
|                           |              |          |          |          |       |         |        |           |             |
| **RecyclableBuffer_10**       | **4096**         | **663.8 ns** |  **7.31 ns** |  **6.48 ns** |  **1.00** |    **0.01** | **0.0286** |     **184 B** |        **1.00** |
| RecyclableMemoryStream_10 | 4096         | 841.9 ns | 11.83 ns | 10.48 ns |  1.27 |    0.02 | 0.0439 |     280 B |        1.52 |
