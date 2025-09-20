using RecyclableBuffer;
using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace WebApiClientCore.Internals
{
    /// <summary>
    /// 表示单内存段的可回收缓冲区写入器
    /// </summary>
    [DebuggerDisplay("WrittenBytes = {WrittenSpan.Length}")]
    public sealed class SingleSegmentBufferWriter : IBufferWriter<byte>, IDisposable
    {
        private bool _disposed = false;
        private RentedBuffer _buffer;
        private readonly BufferPool _pool;

        /// <summary>
        /// 获取已写入的字节数据的 <see cref="ReadOnlySpan{Byte}"/>。
        /// </summary>
        public ReadOnlySpan<byte> WrittenSpan => this._buffer.WritternSpan;

        /// <summary>
        /// 获取已写入的字节数据的 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        public ReadOnlyMemory<byte> WrittenMemory => this._buffer.WritternMemory;

        /// <summary>
        /// 使用指定的初始容量和 <see cref="BufferPool.Shared"/> 初始化 <see cref="SingleSegmentBufferWriter"/> 实例。
        /// </summary>
        /// <param name="minimumSize">缓冲区的最小容量（字节）。</param>
        public SingleSegmentBufferWriter(int minimumSize)
            : this(BufferPool.Shared, minimumSize)
        {
        }

        /// <summary>
        /// 使用指定的缓冲区池和初始容量初始化 <see cref="SingleSegmentBufferWriter"/> 实例。
        /// </summary>
        /// <param name="pool">用于租用缓冲区的池。</param>
        /// <param name="minimumSize">缓冲区的最小容量（字节）。</param>
        public SingleSegmentBufferWriter(BufferPool pool, int minimumSize)
        {
            this._pool = pool;
            this._buffer = new RentedBuffer(pool, minimumSize);
        }

        /// <summary>
        /// 通知写入器已写入指定数量的字节。
        /// </summary>
        /// <param name="count">已写入的字节数。</param>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public void Advance(int count)
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);
            this._buffer.Advance(count);
        }

        /// <summary>
        /// 获取用于写入的 <see cref="Memory{Byte}"/>，可指定期望的最小长度。
        /// 若空间不足则自动扩容。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>满足长度要求的 <see cref="Memory{Byte}"/>。</returns>
        /// <exception cref="ObjectDisposedException">对象已释放时抛出。</exception>
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);

            var memory = this._buffer.GetMemory(sizeHint);
            if (memory.IsEmpty)
            {
                this.ResizeBuffer(sizeHint);
                memory = this._buffer.GetMemory(sizeHint);
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
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);

            var span = this._buffer.GetSpan(sizeHint);
            if (span.IsEmpty)
            {
                this.ResizeBuffer(sizeHint);
                span = this._buffer.GetSpan(sizeHint);
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
            ArgumentOutOfRangeException.ThrowIfNegative(sizeHint);

            var minimumSize = sizeHint + this._buffer.Capacity + 1;
            var nextBuffer = new RentedBuffer(this._pool, minimumSize);

            var source = this._buffer.WritternSpan;
            if (source.Length > 0)
            {
                source.CopyTo(nextBuffer.GetSpan());
                nextBuffer.Advance(source.Length);
            }

            this._buffer.Dispose();
            this._buffer = nextBuffer;
        }

        /// <summary>
        /// 将缓冲区内容包装为 <see cref="Stream"/>，可选是否拥有缓冲区写入器的所有权。
        /// </summary>
        /// <returns>包装的 <see cref="Stream"/> 实例。</returns>
        public Stream AsStream()
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);
            return new BufferWriterStream(this);
        }

        /// <summary>
        /// 释放所有租用的缓冲区并归还到池。
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
            this._buffer.Dispose();
        }
    }
}