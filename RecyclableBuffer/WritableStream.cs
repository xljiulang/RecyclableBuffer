using System;
using System.Buffers;
using System.IO;

namespace RecyclableBuffer
{
    sealed class WritableStream : Stream
    {
        private readonly SegmentBufferWriter _bufferWriter;

        public WritableStream(SegmentBufferWriter bufferWriter)
        {
            this._bufferWriter = bufferWriter;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => _bufferWriter.Length;

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void WriteByte(byte value)
        {
            this._bufferWriter.GetSpan(0)[0] = value;
            this._bufferWriter.Advance(sizeof(byte));
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._bufferWriter.Write(buffer.AsSpan(offset, count));
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            this._bufferWriter.Write(buffer);
        }
    }
}
