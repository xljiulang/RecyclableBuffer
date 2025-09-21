using System.Buffers;

namespace RecyclableBuffer.Tests
{
    public class MultipleSegmentBufferWriterTests
    {
        [Fact]
        public void Constructor_Default_UsesSharedPool()
        {
            using var writer = new RecyclableBuffer.MultipleSegmentBufferWriter();
            Assert.NotNull(writer);
        }

        [Fact]
        public void Constructor_WithPool_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RecyclableBuffer.MultipleSegmentBufferWriter(null!));
        }

        [Fact]
        public void GetMemory_ReturnsWritableMemory()
        {
            using var writer = new RecyclableBuffer.MultipleSegmentBufferWriter();
            var memory = writer.GetMemory(16);
            Assert.True(memory.Length >= 16);
        }

        [Fact]
        public void GetSpan_ReturnsWritableSpan()
        {
            using var writer = new RecyclableBuffer.MultipleSegmentBufferWriter();
            var span = writer.GetSpan(32);
            Assert.True(span.Length >= 32);
        }

        [Fact]
        public void Advance_UpdatesWrittenBuffer()
        {
            using var writer = new RecyclableBuffer.MultipleSegmentBufferWriter();
            var span = writer.GetSpan(8);
            span[0] = 42;
            writer.Advance(1);

            var sequence = writer.WrittenSequence;
            Assert.Equal(1, sequence.Length);
            Assert.Equal(42, sequence.First.Span[0]);
        }

        [Fact]
        public void WrittenBuffer_EmptyWhenNoData()
        {
            using var writer = new RecyclableBuffer.MultipleSegmentBufferWriter();
            Assert.Equal(0, writer.WrittenSequence.Length);
        }

        [Fact]
        public void AsStream_WritesData_VerifyWithWrittenBuffer()
        {
            using var writer = new RecyclableBuffer.MultipleSegmentBufferWriter();
            using var stream = writer.AsWritableStream();

            var data = new byte[] { 1, 2, 3, 4 };
            stream.Write(data, 0, data.Length);

            var sequence = writer.WrittenSequence;
            Assert.Equal(data.Length, sequence.Length);
            Assert.Equal(data, sequence.ToArray());
        }

        [Fact]
        public void Dispose_ReleasesBuffers()
        {
            using var writer = new RecyclableBuffer.MultipleSegmentBufferWriter();
            writer.GetSpan(8);
            writer.Dispose();

            // Dispose again should not throw
            writer.Dispose();
        }
    }
}