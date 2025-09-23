using System.Buffers;

namespace RecyclableBuffer.Tests
{
    public class SingleSegmentBufferWriterTests
    {
        [Fact]
        public void Constructor_InitializesBufferWithMinimumLength()
        {
            using var writer = new SingleSegmentBufferWriter(16);
            Assert.True(writer.WrittenSpan.Length == 0);
            Assert.True(writer.WrittenMemory.Length == 0);
            Assert.True(writer.WrittenSequence.Length == 0);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenPoolIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SingleSegmentBufferWriter(8, null!));
        }

        [Fact]
        public void Constructor_WithPoolAndCapacity_ShouldAllocateBuffer()
        {
            var pool = ByteArrayPool.Shared;
            using var writer = new SingleSegmentBufferWriter(32, pool);
            Assert.True(writer.WrittenSpan.Length == 0);
        }

        [Fact]
        public void Advance_IncreasesWrittenLength()
        {
            using var writer = new SingleSegmentBufferWriter(8);
            var span = writer.GetSpan(4);
            span[0] = 1;
            span[1] = 2;
            span[2] = 3;
            span[3] = 4;
            writer.Advance(4);

            Assert.Equal(4, writer.WrittenSpan.Length);
            Assert.Equal(4, writer.WrittenMemory.Length);
            Assert.Equal(4, writer.WrittenSequence.Length);
            Assert.Equal([1, 2, 3, 4], writer.WrittenSpan.ToArray());
        }

        [Fact]
        public void GetMemory_ExpandsBuffer_WhenInsufficientSpace()
        {
            using var writer = new SingleSegmentBufferWriter(4);
            writer.GetMemory(4);
            writer.Advance(4);

            var mem = writer.GetMemory(10);
            Assert.True(mem.Length >= 10);
        }

        [Fact]
        public void GetSpan_ExpandsBuffer_WhenInsufficientSpace()
        {
            using var writer = new SingleSegmentBufferWriter(4);
            writer.GetSpan(4);
            writer.Advance(4);

            var span = writer.GetSpan(10);
            Assert.True(span.Length >= 10);
        }

        [Fact]
        public void Write_Buffer_Correctly()
        {
            using var writer = new SingleSegmentBufferWriter(1111);
            var bytes = new byte[1024 * 3 + 77];
            Random.Shared.NextBytes(bytes);
            writer.Write(bytes);

            Assert.Equal(bytes, writer.WrittenSequence.ToArray());
        }

        [Fact]
        public void WrittenSequence_ReturnsCorrectData()
        {
            using var writer = new SingleSegmentBufferWriter(8);
            var span = writer.GetSpan(4);
            span[0] = 10;
            span[1] = 20;
            span[2] = 30;
            span[3] = 40;
            writer.Advance(4);

            var seq = writer.WrittenSequence;
            Assert.Equal(4, seq.Length);
            Assert.Equal([10, 20, 30, 40], seq.ToArray());
        }

        [Fact]
        public void Advance_ThrowsObjectDisposedException_AfterDispose()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.Advance(1));
        }

        [Fact]
        public void GetMemory_ThrowsObjectDisposedException_AfterDispose()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetMemory(1));
        }

        [Fact]
        public void GetSpan_ThrowsObjectDisposedException_AfterDispose()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetSpan(1));
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            writer.Dispose(); // Should not throw
        }
    }
}