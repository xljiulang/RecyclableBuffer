```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                      | UserLength | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |----------- |-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **RecyclableBuffer_Json**       | **10**         |   **2.476 μs** | **0.0495 μs** | **0.0661 μs** |   **2.485 μs** |  **1.00** |    **0.04** | **0.1068** |     **680 B** |        **1.00** |
| RecyclableMemoryStream_Json | 10         |   2.663 μs | 0.0508 μs | 0.0565 μs |   2.640 μs |  1.08 |    0.04 | 0.1183 |     744 B |        1.09 |
|                             |            |            |           |           |            |       |         |        |           |             |
| **RecyclableBuffer_Json**       | **100**        |  **22.532 μs** | **0.4457 μs** | **1.1345 μs** |  **22.173 μs** |  **1.00** |    **0.07** | **0.0916** |     **680 B** |        **1.00** |
| RecyclableMemoryStream_Json | 100        |  21.922 μs | 0.4299 μs | 0.4021 μs |  21.939 μs |  0.98 |    0.05 | 0.0916 |     744 B |        1.09 |
|                             |            |            |           |           |            |       |         |        |           |             |
| **RecyclableBuffer_Json**       | **1000**       | **213.033 μs** | **4.2115 μs** | **4.3249 μs** | **212.673 μs** |  **1.00** |    **0.03** |      **-** |     **680 B** |        **1.00** |
| RecyclableMemoryStream_Json | 1000       | 212.541 μs | 4.1512 μs | 4.2630 μs | 212.890 μs |  1.00 |    0.03 |      - |     776 B |        1.14 |
