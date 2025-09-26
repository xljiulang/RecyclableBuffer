```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
Intel Core i7-8565U CPU 1.80GHz (Max: 1.99GHz) (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                           | SizeHint | Mean         | Error        | StdDev       | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------------- |--------- |-------------:|-------------:|-------------:|-------------:|-------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **SingleSegmentBufferWriter**        | **0**        |     **98.31 ns** |     **1.984 ns** |     **3.577 ns** |     **97.86 ns** |   **0.87** |    **0.05** |   **0.0191** |        **-** |        **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 0        |    113.25 ns |     2.289 ns |     4.778 ns |    112.01 ns |   1.00 |    0.06 |   0.0440 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 0        |    300.54 ns |     5.992 ns |    13.277 ns |    296.61 ns |   2.66 |    0.16 |   0.0668 |        - |        - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 0        |    172.35 ns |     3.342 ns |     5.002 ns |    171.84 ns |   1.52 |    0.08 |   0.0439 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 0        |    632.19 ns |    12.607 ns |    31.162 ns |    618.94 ns |   5.59 |    0.36 |   0.0534 |        - |        - |     224 B |        1.22 |
|                                  |          |              |              |              |              |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **8192**     |    **115.00 ns** |     **6.676 ns** |    **19.475 ns** |    **115.04 ns** |   **0.89** |    **0.16** |   **0.0191** |        **-** |        **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter      | 8192     |    129.11 ns |     2.587 ns |     5.677 ns |    128.67 ns |   1.00 |    0.06 |   0.0439 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 8192     |    327.15 ns |     6.583 ns |    17.797 ns |    326.07 ns |   2.54 |    0.17 |   0.0668 |        - |        - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter | 8192     |    201.61 ns |     3.977 ns |     7.756 ns |    201.02 ns |   1.56 |    0.09 |   0.0439 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 8192     |    726.79 ns |    20.511 ns |    59.179 ns |    717.82 ns |   5.64 |    0.52 |   0.0534 |        - |        - |     224 B |        1.22 |
|                                  |          |              |              |              |              |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**        | **131073**   |    **147.65 ns** |     **2.978 ns** |     **6.473 ns** |    **146.92 ns** |   **1.04** |    **0.07** |   **0.0286** |        **-** |        **-** |     **120 B** |        **0.65** |
| MultipleSegmentBufferWriter      | 131073   |    142.35 ns |     2.882 ns |     6.680 ns |    141.41 ns |   1.00 |    0.07 |   0.0439 |        - |        - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream | 131073   | 51,646.03 ns | 1,827.024 ns | 5,387.021 ns | 50,679.62 ns | 363.59 |   41.26 | 333.3130 | 333.3130 | 333.3130 | 1049015 B |    5,701.17 |
| DotNext_PoolingArrayBufferWriter | 131073   |    218.55 ns |     4.405 ns |     9.194 ns |    217.98 ns |   1.54 |    0.10 |   0.0439 |        - |        - |     184 B |        1.00 |
| DotNext_SparseBufferWriter       | 131073   |    931.68 ns |    44.801 ns |   132.096 ns |    908.68 ns |   6.56 |    0.97 |   0.0534 |        - |        - |     224 B |        1.22 |
