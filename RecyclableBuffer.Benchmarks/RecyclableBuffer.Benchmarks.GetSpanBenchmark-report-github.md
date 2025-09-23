```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                           | SizeHint | Mean         | Error      | StdDev     | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------------- |--------- |-------------:|-----------:|-----------:|-------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **0**        |     **48.27 ns** |   **0.863 ns** |   **0.807 ns** |   **0.64** |    **0.02** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 0        |     74.88 ns |   1.455 ns |   1.361 ns |   1.00 |    0.02 |   0.0280 |        - |        - |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 0        |    185.32 ns |   1.721 ns |   1.525 ns |   2.48 |    0.05 |   0.0446 |        - |        - |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 0        |     87.35 ns |   1.732 ns |   2.988 ns |   1.17 |    0.04 |   0.0293 |        - |        - |     184 B |        1.05 |
| DotNext_SparseBufferWriter       | 0        |    285.22 ns |   5.496 ns |   6.543 ns |   3.81 |    0.11 |   0.0353 |        - |        - |     224 B |        1.27 |
|                                  |          |              |            |            |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **8192**     |     **47.18 ns** |   **0.970 ns** |   **0.907 ns** |   **0.62** |    **0.01** |   **0.0127** |        **-** |        **-** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter      | 8192     |     75.80 ns |   1.016 ns |   0.950 ns |   1.00 |    0.02 |   0.0280 |        - |        - |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192     |    185.53 ns |   2.012 ns |   1.882 ns |   2.45 |    0.04 |   0.0446 |        - |        - |     280 B |        1.59 |
| DotNext_PoolingArrayBufferWriter | 8192     |     84.24 ns |   1.557 ns |   1.529 ns |   1.11 |    0.02 |   0.0293 |        - |        - |     184 B |        1.05 |
| DotNext_SparseBufferWriter       | 8192     |    293.33 ns |   5.802 ns |  10.898 ns |   3.87 |    0.15 |   0.0353 |        - |        - |     224 B |        1.27 |
|                                  |          |              |            |            |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **131073**   |     **73.79 ns** |   **1.459 ns** |   **1.845 ns** |   **1.04** |    **0.05** |   **0.0191** |        **-** |        **-** |     **120 B** |        **0.68** |
| MultipleSegmentBufferWriter      | 131073   |     71.38 ns |   1.441 ns |   3.341 ns |   1.00 |    0.07 |   0.0280 |        - |        - |     176 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 131073   | 17,621.51 ns | 240.763 ns | 213.430 ns | 247.39 |   11.59 | 333.3130 | 333.3130 | 333.3130 | 1049021 B |    5,960.35 |
| DotNext_PoolingArrayBufferWriter | 131073   |     84.43 ns |   1.637 ns |   2.549 ns |   1.19 |    0.06 |   0.0293 |        - |        - |     184 B |        1.05 |
| DotNext_SparseBufferWriter       | 131073   |    290.77 ns |   5.686 ns |   8.335 ns |   4.08 |    0.22 |   0.0353 |        - |        - |     224 B |        1.27 |
