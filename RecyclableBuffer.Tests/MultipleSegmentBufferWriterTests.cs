using System.Buffers;

namespace RecyclableBuffer.Tests
{
    public class MultipleSegmentBufferWriterTests
    {
        [Fact]
        public void Constructor_InitializesWithDefaultPool()
        {
            using var writer = new MultipleSegmentBufferWriter();
            Assert.Equal(0, writer.WrittenSequence.Length);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenPoolIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MultipleSegmentBufferWriter(null!));
        }

        [Fact]
        public void Advance_ThrowsInvalidOperationException_IfNoBuffer()
        {
            using var writer = new MultipleSegmentBufferWriter();
            Assert.Throws<InvalidOperationException>(() => writer.Advance(1));
        }

        [Fact]
        public void Advance_IncreasesWrittenLength()
        {
            using var writer = new MultipleSegmentBufferWriter();
            var span = writer.GetSpan(4);
            span[0] = 1;
            span[1] = 2;
            span[2] = 3;
            span[3] = 4;
            writer.Advance(4);

            Assert.Equal(4, writer.WrittenSequence.Length);
            Assert.Equal([1, 2, 3, 4], writer.WrittenSequence.ToArray());
        }

        [Fact]
        public void GetMemory_AllocatesBuffer_WhenNoneExists()
        {
            using var writer = new MultipleSegmentBufferWriter();
            var mem = writer.GetMemory(8);
            Assert.True(mem.Length >= 8);
        }

        [Fact]
        public void GetMemory_ExpandsBuffer_WhenInsufficientSpace()
        {
            using var writer = new MultipleSegmentBufferWriter();
            writer.GetMemory(4);
            writer.Advance(4);

            var mem = writer.GetMemory(10);
            Assert.True(mem.Length >= 10);
        }

        [Fact]
        public void GetSpan_AllocatesBuffer_WhenNoneExists()
        {
            using var writer = new MultipleSegmentBufferWriter();
            var span = writer.GetSpan(8);
            Assert.True(span.Length >= 8);
        }

        [Fact]
        public void GetSpan_ExpandsBuffer_WhenInsufficientSpace()
        {
            using var writer = new MultipleSegmentBufferWriter();
            writer.GetSpan(4);
            writer.Advance(4);

            var span = writer.GetSpan(10);
            Assert.True(span.Length >= 10);
        }


        [Fact]
        public void Write_Buffer_Correctly()
        {
            using var writer = new MultipleSegmentBufferWriter();
            var bytes = new byte[1024 * 300];
            Random.Shared.NextBytes(bytes);
            writer.Write(bytes);

            Assert.Equal(bytes, writer.WrittenSequence.ToArray());
        }

        [Fact]
        public void WrittenSequence_ReturnsCorrectData_AcrossMultipleSegments()
        {
            using var writer = new MultipleSegmentBufferWriter();
            var span1 = writer.GetSpan(4);
            span1[0] = 10;
            span1[1] = 20;
            span1[2] = 30;
            span1[3] = 40;
            writer.Advance(4);

            var span2 = writer.GetSpan(4);
            span2[0] = 50;
            span2[1] = 60;
            span2[2] = 70;
            span2[3] = 80;
            writer.Advance(4);

            var seq = writer.WrittenSequence;
            Assert.Equal(8, seq.Length);
            Assert.Equal([10, 20, 30, 40, 50, 60, 70, 80], seq.ToArray());
        }

        [Fact]
        public void Advance_ThrowsInvalidOperationException_AfterDispose()
        {
            var writer = new MultipleSegmentBufferWriter();
            writer.Dispose();
            Assert.Throws<InvalidOperationException>(() => writer.Advance(1));
        }

        [Fact]
        public void GetMemory_ThrowsObjectDisposedException_AfterDispose()
        {
            var writer = new MultipleSegmentBufferWriter();
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetMemory(1));
        }

        [Fact]
        public void GetSpan_ThrowsObjectDisposedException_AfterDispose()
        {
            var writer = new MultipleSegmentBufferWriter();
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetSpan(1));
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            var writer = new MultipleSegmentBufferWriter();
            writer.Dispose();
            writer.Dispose(); // Should not throw
        }
    }
}