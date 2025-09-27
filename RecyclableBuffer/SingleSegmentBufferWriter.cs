using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示单内存段的可回收缓冲区写入器
    /// </summary>  
    public sealed class SingleSegmentBufferWriter : SegmentBufferWriter
    {
        private bool _disposed = false;
        private RentedBuffer _buffer;
        private readonly ArrayPool<byte> _arrayPool;

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public override long Length
        {
            get
            {
                ThrowIfDisposed();
                return this._buffer.Length;
            }
        }

        /// <summary>
        /// 获取已写入的字节数据的 <see cref="ReadOnlySpan{Byte}"/>。
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public ReadOnlySpan<byte> WrittenSpan
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.WritternSpan;
            }
        }

        /// <summary>
        /// 获取已写入的字节数据的 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public ReadOnlyMemory<byte> WrittenMemory
        {
            get
            {
                ThrowIfDisposed();
                return _buffer.WritternMemory;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public override ReadOnlySequence<byte> WrittenSequence
        {
            get
            {
                ThrowIfDisposed();
                return new(_buffer.WritternMemory);
            }
        }

        /// <summary>
        /// 使用指定的初始容量和 <see cref="ArrayPool{Byte}.Shared"/> 初始化 <see cref="SingleSegmentBufferWriter"/> 实例。
        /// </summary>
        /// <param name="minimumLength">缓冲区的最小容量（字节）。</param>
        public SingleSegmentBufferWriter(int minimumLength)
            : this(minimumLength, ArrayPool<byte>.Shared)
        {
        }

        /// <summary>
        /// 使用指定的缓冲区池和初始容量初始化 <see cref="SingleSegmentBufferWriter"/> 实例。
        /// </summary>
        /// <param name="minimumLength">缓冲区的最小容量（字节）。</param>
        /// <param name="arrayPool">用于租用缓冲区的池。</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SingleSegmentBufferWriter(int minimumLength, ArrayPool<byte> arrayPool)
        {
            _arrayPool = arrayPool ?? throw new ArgumentNullException(nameof(arrayPool));
            _buffer = new RentedBuffer(arrayPool, minimumLength);
        }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果 <paramref name="count"/> 超过剩余空间，则抛出异常。</exception> 
        public override void Advance(int count)
        {
            ThrowIfDisposed();
            _buffer.Advance(count);
        }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public override Memory<byte> GetMemory(int sizeHint = 0)
        {
            ThrowIfDisposed();

            var memory = _buffer.GetMemory(sizeHint);
            if (memory.IsEmpty)
            {
                ResizeBuffer(sizeHint);
                memory = _buffer.GetMemory(sizeHint);

                Debug.Assert(memory.Length > 0);
            }
            return memory;
        }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public override Span<byte> GetSpan(int sizeHint = 0)
        {
            ThrowIfDisposed();

            var span = _buffer.GetSpan(sizeHint);
            if (span.IsEmpty)
            {
                ResizeBuffer(sizeHint);
                span = _buffer.GetSpan(sizeHint);

                Debug.Assert(span.Length > 0);
            }
            return span;
        }

        /// <summary>
        /// 扩容缓冲区以满足指定的最小空间需求。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度。</param>
        private void ResizeBuffer(int sizeHint)
        {
            if (sizeHint < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeHint));
            }

            var minimumLength = sizeHint + _buffer.Capacity + 1;
            var nextBuffer = new RentedBuffer(_arrayPool, minimumLength);

            var source = _buffer.WritternSpan;
            if (source.Length > 0)
            {
                source.CopyTo(nextBuffer.GetSpan(0));
                nextBuffer.Advance(source.Length);
            }

            _buffer.Dispose();
            _buffer = nextBuffer;
        }


        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>"
        public override Stream AsReadableStream()
        {
            ThrowIfDisposed();
            return new BufferWriterReadableStream(this);
        }

        /// <summary>
        /// 如果对象已释放则抛出 <see cref="ObjectDisposedException"/> 异常。
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                Throw();
            }

            static void Throw()
            {
                throw new ObjectDisposedException(nameof(SingleSegmentBufferWriter));
            }
        }

        /// <inheritdoc/>        
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _buffer.Dispose();
            _disposed = true;
        }


        private sealed class BufferWriterReadableStream : ReadableStream
        {
            private readonly SingleSegmentBufferWriter _bufferWriter;

            public override long Length => this._bufferWriter.Length;

            public BufferWriterReadableStream(SingleSegmentBufferWriter bufferWriter)
            {
                this._bufferWriter = bufferWriter;
            }

            public override void CopyTo(Stream destination, int bufferSize)
            {
                var span = this._bufferWriter.WrittenSpan.Slice((int)this.Position);
                destination.Write(span);
            }

            public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
            {
                var memory = this._bufferWriter.WrittenMemory.Slice((int)this.Position);
                return destination.WriteAsync(memory, cancellationToken).AsTask();
            }

            public override int Read(Span<byte> buffer)
            {
                var writtenSpan = this._bufferWriter.WrittenSpan;
                var remaining = writtenSpan.Length - this.Position;

                if (remaining <= 0L)
                {
                    return 0;
                }

                var bytesToRead = (int)Math.Min(buffer.Length, remaining);
                writtenSpan.Slice((int)this.Position, bytesToRead).CopyTo(buffer);

                this.Position += bytesToRead;
                return bytesToRead;
            }
        }
    }
}