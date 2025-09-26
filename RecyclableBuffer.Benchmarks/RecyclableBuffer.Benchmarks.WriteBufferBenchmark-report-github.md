```

BenchmarkDotNet v0.15.3, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.20 (8.0.20, 8.0.2025.41914), X64 RyuJIT x86-64-v3


```
| Method                              | BufferSize | Mean         | Error      | StdDev     | Median       | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------------------------ |----------- |-------------:|-----------:|-----------:|-------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **SingleSegmentBufferWriter**           | **1024**       |     **59.07 ns** |   **1.036 ns** |   **0.969 ns** |     **58.87 ns** |  **0.73** |    **0.06** | **0.0126** |      **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared  | 1024       |     81.67 ns |   2.425 ns |   7.075 ns |     78.64 ns |  1.01 |    0.12 | 0.0293 |      - |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Default | 1024       |     69.48 ns |   1.283 ns |   1.002 ns |     69.67 ns |  0.86 |    0.07 | 0.0293 |      - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream    | 1024       |    213.55 ns |   9.218 ns |  26.150 ns |    201.53 ns |  2.63 |    0.38 | 0.0446 |      - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter    | 1024       |    230.46 ns |   4.324 ns |   9.026 ns |    228.66 ns |  2.84 |    0.25 | 0.0293 |      - |     184 B |        1.00 |
| DotNext_SparseBufferWriter          | 1024       |    430.72 ns |   4.963 ns |   4.144 ns |    431.88 ns |  5.31 |    0.42 | 0.0353 |      - |     224 B |        1.22 |
|                                     |            |              |            |            |              |       |         |        |        |           |             |
| **SingleSegmentBufferWriter**           | **8192**       |    **111.05 ns** |   **1.597 ns** |   **1.415 ns** |    **110.78 ns** |  **0.88** |    **0.01** | **0.0126** |      **-** |      **80 B** |        **0.43** |
| MultipleSegmentBufferWriter_Shared  | 8192       |    125.69 ns |   0.675 ns |   0.564 ns |    125.66 ns |  1.00 |    0.01 | 0.0293 |      - |     184 B |        1.00 |
| MultipleSegmentBufferWriter_Default | 8192       |    107.77 ns |   2.194 ns |   5.931 ns |    106.01 ns |  0.86 |    0.05 | 0.0293 |      - |     184 B |        1.00 |
| Microsoft_RecyclableMemoryStream    | 8192       |    244.60 ns |   2.873 ns |   2.399 ns |    245.12 ns |  1.95 |    0.02 | 0.0443 |      - |     280 B |        1.52 |
| DotNext_PoolingArrayBufferWriter    | 8192       |    374.89 ns |   7.210 ns |   9.375 ns |    371.22 ns |  2.98 |    0.07 | 0.0291 |      - |     184 B |        1.00 |
| DotNext_SparseBufferWriter          | 8192       |    500.06 ns |   9.528 ns |  26.244 ns |    495.11 ns |  3.98 |    0.21 | 0.0496 |      - |     312 B |        1.70 |
|                                     |            |              |            |            |              |       |         |        |        |           |             |
| **SingleSegmentBufferWriter**           | **524288**     | **20,737.77 ns** | **350.927 ns** | **404.128 ns** | **20,603.31 ns** |  **1.99** |    **0.05** |      **-** |      **-** |     **160 B** |        **0.53** |
| MultipleSegmentBufferWriter_Shared  | 524288     | 10,416.66 ns | 166.734 ns | 147.806 ns | 10,380.29 ns |  1.00 |    0.02 | 0.0458 |      - |     304 B |        1.00 |
| MultipleSegmentBufferWriter_Default | 524288     |  9,559.85 ns |  83.089 ns |  69.383 ns |  9,563.54 ns |  0.92 |    0.01 | 0.0458 |      - |     304 B |        1.00 |
| Microsoft_RecyclableMemoryStream    | 524288     |  9,770.75 ns |  67.812 ns |  56.626 ns |  9,765.34 ns |  0.94 |    0.01 | 0.0763 |      - |     480 B |        1.58 |
| DotNext_PoolingArrayBufferWriter    | 524288     | 27,954.91 ns | 549.005 ns | 917.264 ns | 27,909.86 ns |  2.68 |    0.09 |      - |      - |     184 B |        0.61 |
| DotNext_SparseBufferWriter          | 524288     | 29,241.77 ns | 245.071 ns | 229.240 ns | 29,280.36 ns |  2.81 |    0.04 | 1.8005 | 0.0305 |   11400 B |       37.50 |
