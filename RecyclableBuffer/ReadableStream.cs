using System;
using System.Buffers;
using System.IO;

namespace RecyclableBuffer
{
    sealed class ReadableStream : Stream
    {
        private long _position = 0L;
        private readonly ReadOnlySequence<byte> _sequence;

        public ReadableStream(ReadOnlyMemory<byte> sequence)
            : this(new ReadOnlySequence<byte>(sequence))
        {
        }

        public ReadableStream(ReadOnlySequence<byte> sequence)
        {
            this._sequence = sequence;
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => _sequence.Length;

        public override long Position
        {
            get => this._position;
            set
            {
                if (value < 0 || value > this._sequence.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                this._position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.Read(buffer.AsSpan(offset, count));
        }

        public override int Read(Span<byte> buffer)
        {
            if (this._position >= this._sequence.Length)
            {
                return 0;
            }

            var remaining = this._sequence.Length - this._position;
            var bytesToRead = (int)Math.Min(buffer.Length, remaining);

            this._sequence.Slice(this._position, bytesToRead).CopyTo(buffer);
            this._position += bytesToRead;
            return bytesToRead;
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            var newPosition = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => this._position + offset,
                SeekOrigin.End => this._sequence.Length + offset,
                _ => throw new ArgumentException("Invalid seek origin", nameof(origin)),
            };

            if (newPosition < 0 || newPosition > this._sequence.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            this._position = newPosition;
            return newPosition;
        }

        public override void Flush()
        {
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}