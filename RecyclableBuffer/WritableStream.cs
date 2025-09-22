using System;
using System.Buffers;
using System.IO;

namespace RecyclableBuffer
{
    sealed class WritableStream : Stream
    {
        private readonly IBufferWriter<byte> _bufferWriter;

        public WritableStream(IBufferWriter<byte> _bufferWriter)
        {
            this._bufferWriter = _bufferWriter;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

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
            var span = this._bufferWriter.GetSpan(1);
            span[0] = value;
            this._bufferWriter.Advance(1);
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
