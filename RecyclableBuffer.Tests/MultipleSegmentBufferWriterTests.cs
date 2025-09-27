using System.Buffers;

namespace RecyclableBuffer.Tests
{
    public class MultipleSegmentBufferWriterTests
    {
        private const int COUNT = 10;

        [Fact]
        public void Constructor_InitializesWithDefaultPool()
        {
            using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
            Assert.Equal(0, writer.WrittenSequence.Length);
        }
         

        [Fact]
        public void Advance_ThrowsInvalidOperationException_IfNoBuffer()
        {
            using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
            Assert.Throws<InvalidOperationException>(() => writer.Advance(1));
        }

        [Fact]
        public void Advance_IncreasesWrittenLength()
        {
            for (var i = 0; i < COUNT; i++)
            {
                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                var span = writer.GetSpan(4);
                span[0] = 1;
                span[1] = 2;
                span[2] = 3;
                span[3] = 4;
                writer.Advance(4);

                Assert.Equal(4, writer.WrittenSequence.Length);
                Assert.Equal([1, 2, 3, 4], writer.WrittenSequence.ToArray());
            }
        }

        [Fact]
        public void GetMemory_AllocatesBuffer_WhenNoneExists()
        {
            for (var i = 0; i < COUNT; i++)
            {
                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                var mem = writer.GetMemory(8);
                Assert.True(mem.Length >= 8);
            }
        }

        [Fact]
        public void GetMemory_ExpandsBuffer_WhenInsufficientSpace()
        {
            for (var i = 0; i < COUNT; i++)
            {
                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                writer.GetMemory(4);
                writer.Advance(4);

                var mem = writer.GetMemory(10);
                Assert.True(mem.Length >= 10);
            }
        }

        [Fact]
        public void GetSpan_AllocatesBuffer_WhenNoneExists()
        {
            for (var i = 0; i < COUNT; i++)
            {
                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                var span = writer.GetSpan(8);
                Assert.True(span.Length >= 8);
            }
        }

        [Fact]
        public void GetSpan_ExpandsBuffer_WhenInsufficientSpace()
        {
            for (var i = 0; i < COUNT; i++)
            {
                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                writer.GetSpan(4);
                writer.Advance(4);

                var span = writer.GetSpan(10);
                Assert.True(span.Length >= 10);
            }
        }


        [Fact]
        public void Write_Buffer_Correctly()
        {
            var bytes = new byte[1024 * 300 + 1];
            for (var i = 0; i < COUNT; i++)
            {
                Random.Shared.NextBytes(bytes);

                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                writer.Write(bytes);
                Assert.Equal(bytes, writer.WrittenSequence.ToArray());
            }
        }

        [Fact]
        public void Write_Read_Stream_Correctly()
        {
            var bytes = new byte[1024 * 300 + 1];
            for (var i = 0; i < COUNT; i++)
            {
                Random.Shared.NextBytes(bytes);

                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                writer.AsWritableStream().Write(bytes);

                var memoryStream = new MemoryStream();

                var readableStream = writer.AsReadableStream();
                readableStream.CopyTo(memoryStream);
                Assert.Equal(bytes, memoryStream.ToArray());

                readableStream.Position = 0L;
                var buffer = new byte[bytes.Length];
                readableStream.ReadAtLeast(buffer, bytes.Length);
                Assert.Equal(bytes, buffer);
            }
        }

        [Fact]
        public void WrittenSequence_ReturnsCorrectData_AcrossMultipleSegments()
        {
            for (var i = 0; i < COUNT; i++)
            {
                using var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
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
        }

        [Fact]
        public void Advance_ThrowsInvalidOperationException_AfterDispose()
        {
            var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
            writer.Dispose();
            Assert.Throws<InvalidOperationException>(() => writer.Advance(1));
        }

        [Fact]
        public void GetMemory_ThrowsObjectDisposedException_AfterDispose()
        {
            var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetMemory(1));
        }

        [Fact]
        public void GetSpan_ThrowsObjectDisposedException_AfterDispose()
        {
            var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.GetSpan(1));
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            var writer = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
            writer.Dispose();
            writer.Dispose(); // Should not throw
        }
    }
}