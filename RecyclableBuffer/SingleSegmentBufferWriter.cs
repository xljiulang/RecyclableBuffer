using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示单内存段的可回收缓冲区写入器
    /// </summary>  
    public sealed class SingleSegmentBufferWriter : SegmentBufferWriter
    {
        private bool _disposed = false;
        private RentedBuffer _buffer;
        private readonly ArrayPool<byte> _pool;

        /// <inheritdoc/>
        public override int Length => this._buffer.Length;

        /// <summary>
        /// 获取已写入的字节数据的 <see cref="ReadOnlySpan{Byte}"/>。
        /// </summary>
        public ReadOnlySpan<byte> WrittenSpan => _buffer.WritternSpan;

        /// <summary>
        /// 获取已写入的字节数据的 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        public ReadOnlyMemory<byte> WrittenMemory => _buffer.WritternMemory;

        /// <inheritdoc/>
        public override ReadOnlySequence<byte> WrittenSequence => new(_buffer.WritternMemory);

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
        /// <param name="pool">用于租用缓冲区的池。</param>
        public SingleSegmentBufferWriter(int minimumLength, ArrayPool<byte> pool)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _buffer = new RentedBuffer(pool, minimumLength);
        }

        /// <summary>
        /// 通知写入器已写入指定数量的字节。
        /// </summary>
        /// <param name="count">已写入的字节数。</param>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public override void Advance(int count)
        {
            ThrowIfDisposed();
            _buffer.Advance(count);
        }

        /// <summary>
        /// 获取用于写入的 <see cref="Memory{Byte}"/>，可指定期望的最小长度。
        /// 若空间不足则自动扩容。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>满足长度要求的 <see cref="Memory{Byte}"/>。</returns>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public override Memory<byte> GetMemory(int sizeHint = 0)
        {
            ThrowIfDisposed();

            var memory = _buffer.GetMemory(sizeHint);
            if (memory.IsEmpty)
            {
                ResizeBuffer(sizeHint);
                memory = _buffer.GetMemory(sizeHint);
            }
            return memory;
        }

        /// <summary>
        /// 获取用于写入的 <see cref="Span{Byte}"/>，可指定期望的最小长度。
        /// 若空间不足则自动扩容。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>满足长度要求的 <see cref="Span{Byte}"/>。</returns>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public override Span<byte> GetSpan(int sizeHint = 0)
        {
            ThrowIfDisposed();

            var span = _buffer.GetSpan(sizeHint);
            if (span.IsEmpty)
            {
                ResizeBuffer(sizeHint);
                span = _buffer.GetSpan(sizeHint);
            }
            return span;
        }

        /// <summary>
        /// 扩容缓冲区以满足指定的最小空间需求。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度。</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResizeBuffer(int sizeHint)
        {
            if (sizeHint < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeHint));
            }

            var minimumLength = sizeHint + _buffer.Capacity + 1;
            var nextBuffer = new RentedBuffer(_pool, minimumLength);

            var source = _buffer.WritternSpan;
            if (source.Length > 0)
            {
                source.CopyTo(nextBuffer.GetSpan(0));
                nextBuffer.Advance(source.Length);
            }

            _buffer.Dispose();
            _buffer = nextBuffer;
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
            if (_disposed)
            {
                return;
            }

            _buffer.Dispose();
            _disposed = true;
        }
    }
}