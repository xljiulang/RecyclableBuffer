using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示多内存段的可回收缓冲区写入器
    /// </summary> 
    public sealed class MultipleSegmentBufferWriter : SegmentBufferWriter
    {
        private long _length = 0;
        private bool _disposed = false;
        private RentedBuffer? _lastBuffer;
        private readonly ByteArrayPool _pool;

        /// <summary>
        /// 当前写入器持有的所有租用缓冲区列表。
        /// </summary>
        private readonly List<RentedBuffer> _buffers = [];

        /// <inheritdoc/>
        public override long Length
        {
            get
            {
                ThrowIfDisposed();
                return this._length;
            }
        }

        /// <inheritdoc/>
        public override ReadOnlySequence<byte> WrittenSequence
        {
            get
            {
                this.ThrowIfDisposed();

                var buffers = this._buffers;
                if (buffers.Count == 0)
                {
                    return ReadOnlySequence<byte>.Empty;
                }

                var first = new RentedSegment(buffers[0]);
                if (buffers.Count == 1)
                {
                    return new ReadOnlySequence<byte>(first.Memory);
                }

                var last = first;
                for (var i = 1; i < buffers.Count; i++)
                {
                    var buffer = buffers[i];
                    last = last.Append(buffer);
                }

                return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
            }
        }

        /// <summary>
        /// 初始化 <see cref="MultipleSegmentBufferWriter"/> 实例，使用 <see cref="ByteArrayPool.Default"/> 缓冲区池。
        /// </summary> 
        public MultipleSegmentBufferWriter()
            : this(ByteArrayPool.Default)
        {
        }

        /// <summary>
        /// 初始化 <see cref="MultipleSegmentBufferWriter"/> 实例，使用指定的缓冲区池。
        /// </summary>
        /// <param name="pool">用于租用缓冲区的 <see cref="ByteArrayPool"/> 实例。</param>
        public MultipleSegmentBufferWriter(ByteArrayPool pool)
        {
            this._pool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        /// <summary>
        /// 通知写入器已写入指定数量的字节。
        /// </summary>
        /// <param name="count">已写入的字节数。</param>
        /// <exception cref="InvalidOperationException">如果没有可用缓冲区则抛出异常。</exception>      
        public override void Advance(int count)
        {
            if (this._lastBuffer == null)
            {
                Throw();
            }
            else
            {
                this._lastBuffer.Advance(count);
                this._length += count;
            }

            static void Throw()
            {
                throw new InvalidOperationException("No buffer available to advance. Call GetMemory or GetSpan before calling Advance.");
            }
        }

        /// <summary>
        /// 获取用于写入的 <see cref="Memory{Byte}"/>，可指定期望的最小长度。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>可写入的 <see cref="Memory{Byte}"/>。</returns>      
        public override Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (this._lastBuffer == null)
            {
                this._lastBuffer = this.AddRentedBuffer(sizeHint);
            }

            var memory = this._lastBuffer.GetMemory(sizeHint);
            if (memory.IsEmpty)
            {
                this._lastBuffer = this.AddRentedBuffer(sizeHint);
                memory = this._lastBuffer.GetMemory(sizeHint);

                Debug.Assert(memory.Length > 0);
            }
            return memory;
        }

        /// <summary>
        /// 获取用于写入的 <see cref="Span{Byte}"/>，可指定期望的最小长度。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>可写入的 <see cref="Span{Byte}"/>。</returns>
        public override Span<byte> GetSpan(int sizeHint = 0)
        {
            if (this._lastBuffer == null)
            {
                this._lastBuffer = this.AddRentedBuffer(sizeHint);
            }

            var span = this._lastBuffer.GetSpan(sizeHint);
            if (span.IsEmpty)
            {
                this._lastBuffer = this.AddRentedBuffer(sizeHint);
                span = this._lastBuffer.GetSpan(sizeHint);

                Debug.Assert(span.Length > 0);
            }
            return span;
        }


        /// <summary>
        /// 新增一个 <see cref="RentedBuffer"/> 并加入缓冲区列表。
        /// </summary>
        /// <param name="sizeHint">缓冲区大小。</param> 
        /// <returns>新创建的 <see cref="RentedBuffer"/>。</returns>     
        private RentedBuffer AddRentedBuffer(int sizeHint)
        {
            this.ThrowIfDisposed();

            var buffer = new RentedBuffer(_pool, sizeHint);
            this._buffers.Add(buffer);
            return buffer;
        }

        /// <inheritdoc/>      
        public override Stream AsReadableStream()
        {
            ThrowIfDisposed();
            return new BufferWriterReadableStream(this);
        }


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
            if (this._disposed)
            {
                return;
            }

            foreach (var buffer in this._buffers)
            {
                buffer.Dispose();
            }

            this._buffers.Clear();
            this._lastBuffer = null;
            this._length = 0;

            this._disposed = true;
        }


        private sealed class BufferWriterReadableStream : ReadableStream
        {
            private ReadOnlySequence<byte>? _writtenSequence;
            private readonly MultipleSegmentBufferWriter _bufferWriter;

            public override long Length => this._bufferWriter.Length;

            public BufferWriterReadableStream(MultipleSegmentBufferWriter bufferWriter)
            {
                this._bufferWriter = bufferWriter;
            }

            public override void CopyTo(Stream destination, int bufferSize)
            {
                var sequence = this.GetWrittenSequence().Slice(this.Position);
                foreach (var segment in sequence)
                {
                    destination.Write(segment.Span);
                }
            }

            public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
            {
                var sequence = this.GetWrittenSequence().Slice(this.Position);
                foreach (var segment in sequence)
                {
                    await destination.WriteAsync(segment, cancellationToken);
                }
            }

            public override int Read(Span<byte> buffer)
            {
                var writtenSequence = this.GetWrittenSequence();
                var remaining = writtenSequence.Length - this.Position;

                if (remaining <= 0L)
                {
                    return 0;
                }

                var bytesToRead = (int)Math.Min(buffer.Length, remaining);
                writtenSequence.Slice(this.Position, bytesToRead).CopyTo(buffer);

                this.Position += bytesToRead;
                return bytesToRead;
            }


            private ReadOnlySequence<byte> GetWrittenSequence()
            {
                if (this._writtenSequence == null ||
                    this._writtenSequence.Value.Length < this._bufferWriter.Length)
                {
                    var writtenSequence = this._bufferWriter.WrittenSequence;
                    this._writtenSequence = writtenSequence;
                    return writtenSequence;
                }

                return this._writtenSequence.Value;
            }
        }
    }
}
