```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                              | UserLength | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------ |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_10_Json** | **10**         |   **2.482 μs** | **0.0393 μs** | **0.0368 μs** |  **1.00** |    **0.02** | **0.1068** |     **672 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 10         |   2.686 μs | 0.0536 μs | 0.0527 μs |  1.08 |    0.03 | 0.1183 |     744 B |        1.11 |
|                                     |            |            |           |           |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10_Json** | **100**        |  **21.691 μs** | **0.2494 μs** | **0.2332 μs** |  **1.00** |    **0.01** | **0.0916** |     **672 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 100        |  22.185 μs | 0.4312 μs | 0.4428 μs |  1.02 |    0.02 | 0.0916 |     744 B |        1.11 |
|                                     |            |            |           |           |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10_Json** | **1000**       | **212.167 μs** | **3.1927 μs** | **2.9865 μs** |  **1.00** |    **0.02** |      **-** |     **712 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 1000       | 216.284 μs | 4.1275 μs | 9.2319 μs |  1.02 |    0.05 |      - |     776 B |        1.09 |
