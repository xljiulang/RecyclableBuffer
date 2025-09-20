```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                              | UserLength | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------ |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_10_Json** | **10**         |   **2.474 μs** | **0.0221 μs** | **0.0196 μs** |  **1.00** |    **0.01** | **0.1068** |     **672 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 10         |   2.712 μs | 0.0534 μs | 0.0767 μs |  1.10 |    0.03 | 0.1183 |     744 B |        1.11 |
|                                     |            |            |           |           |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10_Json** | **100**        |  **22.182 μs** | **0.4393 μs** | **0.3668 μs** |  **1.00** |    **0.02** | **0.0916** |     **672 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 100        |  22.006 μs | 0.4292 μs | 0.4215 μs |  0.99 |    0.02 | 0.0916 |     744 B |        1.11 |
|                                     |            |            |           |           |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10_Json** | **1000**       | **209.356 μs** | **3.0354 μs** | **2.8393 μs** |  **1.00** |    **0.02** |      **-** |     **712 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 1000       | 207.115 μs | 3.5542 μs | 3.1507 μs |  0.99 |    0.02 |      - |     776 B |        1.09 |
