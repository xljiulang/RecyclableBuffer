```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                              | UserLength | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------ |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_10_Json** | **10**         |   **2.484 μs** | **0.0495 μs** | **0.0550 μs** |  **1.00** |    **0.03** | **0.1068** |     **680 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 10         |   2.745 μs | 0.0517 μs | 0.0508 μs |  1.11 |    0.03 | 0.1183 |     744 B |        1.09 |
|                                     |            |            |           |           |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10_Json** | **100**        |  **21.450 μs** | **0.2282 μs** | **0.2135 μs** |  **1.00** |    **0.01** | **0.0916** |     **680 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 100        |  21.837 μs | 0.2160 μs | 0.2021 μs |  1.02 |    0.01 | 0.0916 |     744 B |        1.09 |
|                                     |            |            |           |           |       |         |        |           |             |
| **MultipleSegmentBufferWriter_10_Json** | **1000**       | **217.429 μs** | **4.2635 μs** | **7.4672 μs** |  **1.00** |    **0.05** |      **-** |     **680 B** |        **1.00** |
| RecyclableMemoryStream_Json         | 1000       | 219.928 μs | 3.7906 μs | 3.5458 μs |  1.01 |    0.04 |      - |     776 B |        1.14 |
