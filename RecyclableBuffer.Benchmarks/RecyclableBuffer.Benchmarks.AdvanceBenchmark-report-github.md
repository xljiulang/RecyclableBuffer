```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3


```
| Method                              | AdvanceCount | Mean        | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------ |------------- |------------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **MultipleSegmentBufferWriter_Advance** | **1024**         |    **74.79 ns** | **0.605 ns** | **0.505 ns** |  **1.00** |    **0.01** | **0.0280** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_Advance      | 1024         |   189.91 ns | 2.794 ns | 2.990 ns |  2.54 |    0.04 | 0.0446 |     280 B |        1.59 |
|                                     |              |             |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_Advance** | **8192**         |    **74.75 ns** | **1.307 ns** | **1.996 ns** |  **1.00** |    **0.04** | **0.0280** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_Advance      | 8192         |   187.16 ns | 1.053 ns | 0.933 ns |  2.51 |    0.06 | 0.0446 |     280 B |        1.59 |
|                                     |              |             |          |          |       |         |        |           |             |
| **MultipleSegmentBufferWriter_Advance** | **131073**       |    **65.43 ns** | **1.078 ns** | **0.955 ns** |  **1.00** |    **0.02** | **0.0280** |     **176 B** |        **1.00** |
| RecyclableMemoryStream_Advance      | 131073       | 2,524.98 ns | 4.906 ns | 4.097 ns | 38.60 |    0.55 | 0.0534 |     344 B |        1.95 |
