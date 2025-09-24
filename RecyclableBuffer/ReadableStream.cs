using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RecyclableBuffer
{
    abstract class ReadableStream : Stream
    {
        private long _position = 0L;

        public sealed override bool CanRead => true;
        public sealed override bool CanSeek => true;
        public sealed override bool CanWrite => false;

        public sealed override long Position
        {
            get => this._position;
            set
            {
                if (value < 0 || value > this.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                this._position = value;
            }
        }

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            return this.Read(buffer.AsSpan(offset, count));
        }

        public abstract override int Read(Span<byte> buffer);

        public abstract override void CopyTo(Stream destination, int bufferSize);

        public abstract override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken);

        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            var newPosition = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => this._position + offset,
                SeekOrigin.End => this.Length + offset,
                _ => throw new ArgumentException("Invalid seek origin", nameof(origin)),
            };

            if (newPosition < 0 || newPosition > this.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            this._position = newPosition;
            return newPosition;
        }

        public sealed override void Flush()
        {
        }

        public sealed override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}