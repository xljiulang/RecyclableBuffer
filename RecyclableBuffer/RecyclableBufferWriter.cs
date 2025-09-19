using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace RecyclableBuffer
{
    /// <summary>
    /// 提供高效的可回收缓冲区写入器，支持分段写入和自动缓冲区池管理。
    /// </summary>
    public sealed class RecyclableBufferWriter : IBufferWriter<byte>, IDisposable
    {
        private bool _disposed = false;
        private readonly BufferPool _pool;
        private readonly List<RentedBuffer> _buffers = [];

        /// <summary>
        /// 获取已写入的所有缓冲区组成的只读字节序列。
        /// </summary>
        public ReadOnlySequence<byte> WrittenBuffer
        {
            get
            {
                var buffers = CollectionsMarshal.AsSpan(this._buffers);
                if (buffers.Length == 0)
                {
                    return ReadOnlySequence<byte>.Empty;
                }

                var first = new RentedSegment(buffers[0]);
                if (buffers.Length == 1)
                {
                    return new ReadOnlySequence<byte>(first.Memory);
                }

                var last = first;
                foreach (var buffer in buffers.Slice(1))
                {
                    last = last.Append(buffer);
                }

                return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
            }
        }

        /// <summary>
        /// 初始化 <see cref="RecyclableBufferWriter"/> 实例，使用默认缓冲区池。
        /// </summary>
        public RecyclableBufferWriter()
            : this(BufferPool.Default)
        {
        }

        /// <summary>
        /// 初始化 <see cref="RecyclableBufferWriter"/> 实例，使用指定的缓冲区池。
        /// </summary>
        /// <param name="pool">用于租用缓冲区的 <see cref="BufferPool"/> 实例。</param>
        public RecyclableBufferWriter(BufferPool pool)
        {
            ArgumentNullException.ThrowIfNull(pool);
            this._pool = pool;
        }

        /// <summary>
        /// 通知写入器已写入指定数量的字节。
        /// </summary>
        /// <param name="count">已写入的字节数。</param>
        public void Advance(int count)
        {
            CollectionsMarshal.AsSpan(this._buffers)[^1].Advance(count);
        }

        /// <summary>
        /// 获取用于写入的 <see cref="Memory{Byte}"/>，可指定期望的最小长度。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>可写入的 <see cref="Memory{Byte}"/>。</returns>
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            var buffer = this.GetOrAddRentedBuffer(sizeHint);
            var memory = buffer.GetMemory(sizeHint);
            if (memory.IsEmpty)
            {
                buffer = this.AddRentedBuffer(sizeHint);
                memory = buffer.GetMemory(sizeHint);
            }
            return memory;
        }

        /// <summary>
        /// 获取用于写入的 <see cref="Span{Byte}"/>，可指定期望的最小长度。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度，默认为 0。</param>
        /// <returns>可写入的 <see cref="Span{Byte}"/>。</returns>
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            var buffer = this.GetOrAddRentedBuffer(sizeHint);
            var span = buffer.GetSpan(sizeHint);
            if (span.IsEmpty)
            {
                buffer = this.AddRentedBuffer(sizeHint);
                span = buffer.GetSpan(sizeHint);
            }
            return span;
        }

        /// <summary>
        /// 获取当前可用的 <see cref="RentedBuffer"/>，如无则新建。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度。</param>
        /// <returns>可用的 <see cref="RentedBuffer"/>。</returns>
        private RentedBuffer GetOrAddRentedBuffer(int sizeHint)
        {
            return this._buffers.Count == 0
                ? AddRentedBuffer(sizeHint)
                : CollectionsMarshal.AsSpan(this._buffers)[^1];
        }

        /// <summary>
        /// 新增一个 <see cref="RentedBuffer"/> 并加入缓冲区列表。
        /// </summary>
        /// <param name="sizeHint">期望的最小长度。</param>
        /// <returns>新创建的 <see cref="RentedBuffer"/>。</returns>
        private RentedBuffer AddRentedBuffer(int sizeHint)
        {
            var minimumLength = sizeHint;
            if (minimumLength == 0)
            {
                minimumLength = Random.Shared.Next(2, _pool.MaxArrayLength);
            }

            var buffer = new RentedBuffer(_pool, minimumLength);
            this._buffers.Add(buffer);
            return buffer;
        }


        /// <summary>
        /// 将缓冲区内容包装为 <see cref="Stream"/>，可选是否拥有缓冲区写入器的所有权。
        /// </summary>
        /// <param name="ownsBufferWriter">是否在流释放时释放缓冲区写入器。</param>
        /// <returns>包装的 <see cref="Stream"/> 实例。</returns>
        public Stream AsStream(bool ownsBufferWriter = false)
        {
            return new RecyclableBufferWriterStream(this, ownsBufferWriter);
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
            foreach (var buffer in this._buffers)
            {
                buffer.Dispose();
            }
        }
    }
}
