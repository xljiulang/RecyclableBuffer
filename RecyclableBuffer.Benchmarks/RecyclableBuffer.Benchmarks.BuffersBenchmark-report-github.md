```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                         | BufferLength | Mean       | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |------------- |-----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_10** | **1024**         |   **227.7 ns** |  **1.44 ns** |  **1.20 ns** |  **1.00** |    **0.01** | **0.0279** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_10      | 1024         |   334.9 ns |  3.11 ns |  2.76 ns |  1.47 |    0.01 | 0.0443 |     280 B |        1.59 |
|                                |              |            |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10** | **8192**         | **1,299.7 ns** |  **4.09 ns** |  **3.63 ns** |  **1.00** |    **0.00** | **0.0267** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_10      | 8192         | 1,413.1 ns | 23.44 ns | 20.78 ns |  1.09 |    0.02 | 0.0439 |     280 B |        1.59 |
|                                |              |            |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10** | **16384**        | **2,368.4 ns** | **16.64 ns** | **14.75 ns** |  **1.00** |    **0.01** | **0.0343** |     **216 B** |        **1.00** |
| RecyclableMemoryStream_10      | 16384        | 2,552.3 ns | 10.81 ns | 10.11 ns |  1.08 |    0.01 | 0.0496 |     312 B |        1.44 |
