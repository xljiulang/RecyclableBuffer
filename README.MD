## RecyclableBuffer
基于 `ArrayPool<byte>` 实现的可复用 `IBufferWriter<byte>` 。大部分情况下性能略优于 [RecyclableMemoryStream](https://github.com/microsoft/Microsoft.IO.RecyclableMemoryStream)


### 使用示例
**`IBufferWriter<byte>` 接口**
```c#
using var bufferWriter = new RecyclableBufferWriter();

// 使用IBufferWriter<byte>接口写入一些数据
var span = bufferWriter.GetSpan(sizeof(int));
BinaryPrimitives.WriteInt32BigEndian(span, 42);
bufferWriter.Advance(sizeof(int));

// 获取已写入的数据序列
var sequence = bufferWriter.WrittenSequence;
```
 
**Stream 接口**
```c#
using var bufferWriter = new RecyclableBufferWriter();

// 使用Stream接口写入
var writableStream = bufferWriter.AsStream();
await JsonSerializer.SerializeAsync(writableStream, users);

// 获取已写入的数据序列
var sequence = bufferWriter.WrittenSequence;

```