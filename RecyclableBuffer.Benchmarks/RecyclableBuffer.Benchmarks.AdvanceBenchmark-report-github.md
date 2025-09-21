```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                              | AdvanceCount | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------ |------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_Advance** | **1024**         |    **77.35 ns** |  **1.215 ns** |  **1.136 ns** |  **1.00** |    **0.02** | **0.0280** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_Advance      | 1024         |   189.70 ns |  2.594 ns |  2.299 ns |  2.45 |    0.05 | 0.0446 |     280 B |        1.59 |
|                                     |              |             |           |           |       |         |        |           |             |
| **MultipleSegmentBufferWriter_Advance** | **131073**       |    **67.89 ns** |  **1.117 ns** |  **1.897 ns** |  **1.00** |    **0.04** | **0.0280** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_Advance      | 131073       | 2,547.85 ns | 21.890 ns | 18.279 ns | 37.56 |    1.05 | 0.0534 |     344 B |        1.95 |
