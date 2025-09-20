using System.Buffers;
using WebApiClientCore.Internals;

namespace RecyclableBuffer.Tests
{
    public class SingleSegmentBufferWriterTests
    {
        [Fact]
        public void Constructor_WithInitialCapacity_ShouldAllocateBuffer()
        {
            var writer = new SingleSegmentBufferWriter(16);
            Assert.True(writer.WrittenSpan.Length == 0);
            Assert.True(writer.WrittenMemory.Length == 0);
            writer.Dispose();
        }

        [Fact]
        public void Constructor_WithPoolAndCapacity_ShouldAllocateBuffer()
        {
            var pool = BufferPool.Shared;
            var writer = new SingleSegmentBufferWriter(pool, 32);
            Assert.True(writer.WrittenSpan.Length == 0);
            writer.Dispose();
        }

        [Fact]
        public void GetMemory_And_Advance_ShouldWriteData()
        {
            var writer = new SingleSegmentBufferWriter(8);
            var memory = writer.GetMemory(4);
            Assert.True(memory.Length >= 4);

            var span = memory.Span;
            span[0] = 1;
            span[1] = 2;
            span[2] = 3;
            span[3] = 4;

            writer.Advance(4);

            var written = writer.WrittenSpan;
            Assert.Equal([1, 2, 3, 4], written.ToArray());
            writer.Dispose();
        }

        [Fact]
        public void GetSpan_And_Advance_ShouldWriteData()
        {
            var writer = new SingleSegmentBufferWriter(8);
            var span = writer.GetSpan(4);
            Assert.True(span.Length >= 4);

            span[0] = 10;
            span[1] = 20;
            span[2] = 30;
            span[3] = 40;

            writer.Advance(4);

            var written = writer.WrittenSpan;
            Assert.Equal([10, 20, 30, 40], written.ToArray());
            writer.Dispose();
        }

        [Fact]
        public void GetMemory_ShouldAutoResize_WhenInsufficient()
        {
            var writer = new SingleSegmentBufferWriter(2);
            var memory = writer.GetMemory(10);
            Assert.True(memory.Length >= 10);
            writer.Dispose();
        }

        [Fact]
        public void GetSpan_ShouldAutoResize_WhenInsufficient()
        {
            var writer = new SingleSegmentBufferWriter(2);
            writer.Write(new byte[] { 1 });

            var span = writer.GetSpan(127);
            Assert.True(span.Length >= 127);
            writer.Dispose();
        }

        [Fact]
        public void Advance_ShouldThrow_WhenDisposed()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.Advance(1));
        }

        [Fact]
        public void GetMemory_ShouldThrow_WhenDisposed()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetMemory(1));
        }

        [Fact]
        public void GetSpan_ShouldThrow_WhenDisposed()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetSpan(1));
        }

        [Fact]
        public void Dispose_ShouldBeIdempotent()
        {
            var writer = new SingleSegmentBufferWriter(8);
            writer.Dispose();
            writer.Dispose(); // ²»Ó¦Å×Òì³£
        }
    }
}