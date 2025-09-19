```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                      | UserLength | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **RecyclableBuffer_Json**       | **10**         |   **2.481 μs** | **0.0479 μs** | **0.0551 μs** |  **1.00** |    **0.03** | **0.1030** |     **664 B** |        **1.00** |
| RecyclableMemoryStream_Json | 10         |   2.699 μs | 0.0472 μs | 0.0418 μs |  1.09 |    0.03 | 0.1183 |     744 B |        1.12 |
|                             |            |            |           |           |       |         |        |           |             |
| **RecyclableBuffer_Json**       | **100**        |  **21.879 μs** | **0.4213 μs** | **0.5016 μs** |  **1.00** |    **0.03** | **0.0916** |     **664 B** |        **1.00** |
| RecyclableMemoryStream_Json | 100        |  22.328 μs | 0.4355 μs | 0.5814 μs |  1.02 |    0.03 | 0.0916 |     744 B |        1.12 |
|                             |            |            |           |           |       |         |        |           |             |
| **RecyclableBuffer_Json**       | **1000**       | **212.594 μs** | **4.0946 μs** | **5.0286 μs** |  **1.00** |    **0.03** |      **-** |     **677 B** |        **1.00** |
| RecyclableMemoryStream_Json | 1000       | 209.742 μs | 4.1548 μs | 4.0806 μs |  0.99 |    0.03 |      - |     776 B |        1.15 |
