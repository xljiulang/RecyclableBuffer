```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                           | SizeHint | Mean         | Error      | StdDev     | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------------- |--------- |-------------:|-----------:|-----------:|-------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **0**        |     **45.94 ns** |   **0.354 ns** |   **0.295 ns** |   **0.64** |    **0.01** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 0        |     72.04 ns |   1.399 ns |   1.309 ns |   1.00 |    0.02 |   0.0280 |        - |        - |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 0        |    181.81 ns |   2.192 ns |   2.051 ns |   2.52 |    0.05 |   0.0446 |        - |        - |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 0        |     84.31 ns |   1.611 ns |   1.507 ns |   1.17 |    0.03 |   0.0293 |        - |        - |     184 B |        1.05 |
|                                  |          |              |            |            |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **8192**     |     **46.11 ns** |   **0.895 ns** |   **0.995 ns** |   **0.62** |    **0.01** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 8192     |     74.56 ns |   0.885 ns |   0.828 ns |   1.00 |    0.02 |   0.0280 |        - |        - |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192     |    188.17 ns |   3.622 ns |   4.581 ns |   2.52 |    0.07 |   0.0446 |        - |        - |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 8192     |     85.25 ns |   1.703 ns |   1.961 ns |   1.14 |    0.03 |   0.0293 |        - |        - |     184 B |        1.05 |
|                                  |          |              |            |            |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **131073**   |     **63.87 ns** |   **1.169 ns** |   **1.094 ns** |   **0.98** |    **0.02** |   **0.0191** |        **-** |        **-** |     **120 B** |        **0.68** |
| MultipleSegmentBufferWriter      | 131073   |     64.88 ns |   1.326 ns |   1.240 ns |   1.00 |    0.03 |   0.0280 |        - |        - |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 131073   | 17,663.30 ns | 323.159 ns | 345.777 ns | 272.34 |    7.22 | 333.3130 | 333.3130 | 333.3130 | 1049021 B |    5,960.35 |
| DotNext_PoolingArrayBufferWriter | 131073   |     83.30 ns |   1.593 ns |   1.636 ns |   1.28 |    0.03 |   0.0293 |        - |        - |     184 B |        1.05 |
