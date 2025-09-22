```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
Intel Core i7-8565U CPU 1.80GHz (Max: 1.99GHz) (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                      | SizeHint | Mean        | Error     | StdDev    | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------- |--------- |------------:|----------:|----------:|-------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **SingleSegmentBufferWriter**   | **0**        |    **104.9 ns** |   **2.05 ns** |   **2.28 ns** |   **0.80** |    **0.03** |   **0.0191** |        **-** |        **-** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter | 0        |    131.7 ns |   2.62 ns |   4.08 ns |   1.00 |    0.04 |   0.0420 |        - |        - |     176 B |        1.00 |
| RecyclableMemoryStream      | 0        |    315.5 ns |   5.55 ns |   4.64 ns |   2.40 |    0.08 |   0.0668 |        - |        - |     280 B |        1.59 |
|                             |          |             |           |           |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**   | **8192**     |    **106.0 ns** |   **2.08 ns** |   **2.23 ns** |   **0.81** |    **0.02** |   **0.0191** |        **-** |        **-** |      **80 B** |        **0.45** |
| MultipleSegmentBufferWriter | 8192     |    130.6 ns |   2.51 ns |   1.96 ns |   1.00 |    0.02 |   0.0420 |        - |        - |     176 B |        1.00 |
| RecyclableMemoryStream      | 8192     |    309.0 ns |   6.14 ns |   5.45 ns |   2.37 |    0.05 |   0.0668 |        - |        - |     280 B |        1.59 |
|                             |          |             |           |           |        |         |          |          |          |           |             |
| **SingleSegmentBufferWriter**   | **131073**   |    **146.0 ns** |   **2.93 ns** |   **2.88 ns** |   **1.05** |    **0.03** |   **0.0286** |        **-** |        **-** |     **120 B** |        **0.68** |
| MultipleSegmentBufferWriter | 131073   |    139.2 ns |   2.75 ns |   3.76 ns |   1.00 |    0.04 |   0.0420 |        - |        - |     176 B |        1.00 |
| RecyclableMemoryStream      | 131073   | 40,636.0 ns | 397.91 ns | 332.27 ns | 292.08 |    8.09 | 333.3130 | 333.3130 | 333.3130 | 1049020 B |    5,960.34 |
