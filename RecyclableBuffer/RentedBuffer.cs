using System;
using System.Buffers;
using System.Diagnostics;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示一个从 <see cref="ArrayPool{T}"/> 租用的可回收缓冲区。
    /// </summary>
    [DebuggerDisplay("Length = {Length}, Capacity = {Capacity}")]
    sealed class RentedBuffer : IDisposable
    {
        /// <summary>
        /// 当前缓冲区已使用的长度。
        /// </summary>
        private int _length = 0;

        /// <summary>
        /// 实际租用的字节数组缓冲区。
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        /// 用于租用和归还缓冲区的数组池。
        /// </summary>
        private readonly BufferPool _pool;

        /// <summary>
        /// 获取当前缓冲区已使用的长度。
        /// </summary>
        public int Length => this._length;

        /// <summary>
        /// 获取缓冲区的总容量。
        /// </summary>
        public int Capacity => this._buffer.Length;

        /// <summary>
        /// 获取当前已使用部分的 <see cref="Span{Byte}"/>。
        /// </summary>
        public Span<byte> WritternSpan => this._buffer.AsSpan(0, this._length);

        /// <summary>
        /// 获取当前已使用部分的 <see cref="Memory{Byte}"/>。
        /// </summary>
        public Memory<byte> WritternMemory => this._buffer.AsMemory(0, this._length);

        /// <summary>
        /// 初始化 <see cref="RentedBuffer"/> 实例，并从指定的数组池租用一个最小长度的缓冲区。
        /// </summary>
        /// <param name="pool">用于租用缓冲区的数组池。</param>
        /// <param name="minimumSize">缓冲区的最小长度。</param>
        public RentedBuffer(BufferPool pool, int minimumSize)
        {
            this._pool = pool;
            this._buffer = pool.Rent(minimumSize);
        }

        /// <summary>
        /// 增加已使用的缓冲区长度。
        /// </summary>
        /// <param name="count">要增加的字节数。</param>
        /// <exception cref="ArgumentOutOfRangeException">如果 <paramref name="count"/> 超过剩余空间，则抛出异常。</exception>
        public void Advance(int count)
        {
            var freeSize = this._buffer.Length - this._length;
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, freeSize);

            this._length += count;
        }

        /// <summary>
        /// 获取当前未使用部分的 <see cref="Span{Byte}"/>，可指定期望的最小长度。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>满足长度要求的 <see cref="Span{Byte}"/>，否则返回空。</returns>
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            var span = this._buffer.AsSpan(this._length);
            if (span.IsEmpty)
            {
                return Span<byte>.Empty;
            }

            if (sizeHint == 0)
            {
                return span;
            }

            if (span.Length < sizeHint)
            {
                return Span<byte>.Empty;
            }
            return span;
        }

        /// <summary>
        /// 获取当前未使用部分的 <see cref="Memory{Byte}"/>，可指定期望的最小长度。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>满足长度要求的 <see cref="Memory{Byte}"/>，否则返回空。</returns>
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            var memory = this._buffer.AsMemory(this._length);
            if (memory.IsEmpty)
            {
                return Memory<byte>.Empty;
            }

            if (sizeHint == 0)
            {
                return memory;
            }

            if (memory.Length < sizeHint)
            {
                return Memory<byte>.Empty;
            }
            return memory;
        }

        /// <summary>
        /// 归还缓冲区到数组池。
        /// </summary>
        public void Dispose()
        {
            this._length = 0;
            this._pool.Return(this._buffer);
        }
    }
}
