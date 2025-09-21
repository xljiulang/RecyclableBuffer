## RecyclableBuffer
基于 `ArrayPool<byte>` 实现的单内存片段和多内存片段可复用的缓冲区 。

### Nuget
[RecyclableBuffer](https://www.nuget.org/packages/RecyclableBuffer/)

### Benchmark
大部分情况下 [MultipleSegmentBufferWriter](https://github.com/xljiulang/RecyclableBuffer/blob/master/RecyclableBuffer.Benchmarks/RecyclableBuffer.Benchmarks.BuffersBenchmark-report-github.md) 的性能略微优于 [RecyclableMemoryStream](https://github.com/microsoft/Microsoft.IO.RecyclableMemoryStream)。


### 使用 SingleSegmentBufferWriter

**`IBufferWriter<byte>` 接口**
```c#
using var bufferWriter = new SingleSegmentBufferWriter(1024);

// 使用IBufferWriter<byte>接口写入一些数据
var span = bufferWriter.GetSpan(sizeof(int));
BinaryPrimitives.WriteInt32BigEndian(span, 42);
bufferWriter.Advance(sizeof(int));

// 获取已写入的数据跨度
var sequence = bufferWriter.WrittenSpan;
```
 
**Stream 接口**
```c#
using var bufferWriter = new SingleSegmentBufferWriter(1024);

// 使用Stream接口写入
var writableStream = bufferWriter.AsStream();
await JsonSerializer.SerializeAsync(writableStream, users);

// 获取已写入的数据跨度
var sequence = bufferWriter.WrittenSpan;

```


### 使用 MultipleSegmentBufferWriter

**`IBufferWriter<byte>` 接口**
```c#
using var bufferWriter = new MultipleSegmentBufferWriter();

// 使用IBufferWriter<byte>接口写入一些数据
var span = bufferWriter.GetSpan(sizeof(int));
BinaryPrimitives.WriteInt32BigEndian(span, 42);
bufferWriter.Advance(sizeof(int));

// 获取已写入的数据序列
var sequence = bufferWriter.WrittenSequence;
```
 
**Stream 接口**
```c#
using var bufferWriter = new MultipleSegmentBufferWriter();

// 使用Stream接口写入
var writableStream = bufferWriter.AsStream();
await JsonSerializer.SerializeAsync(writableStream, users);

// 获取已写入的数据序列
var sequence = bufferWriter.WrittenSequence;

```