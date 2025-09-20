using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示多内存段的可回收缓冲区写入器
    /// </summary>
    [DebuggerDisplay("WrittenBytes = {WrittenSequence.Length}, BuffersCount = {_buffers.Count}")]
    public sealed class MultipleSegmentBufferWriter : IBufferWriter<byte>, IDisposable
    {
        private bool _disposed = false;
        private RentedBuffer? _lastBuffer;
        private readonly BufferPool _pool;

        /// <summary>
        /// 当前写入器持有的所有租用缓冲区列表。
        /// </summary>
        private readonly List<RentedBuffer> _buffers = [];

        /// <summary>
        /// 获取已写入的所有缓冲区组成的只读字节序列。
        /// </summary>
        public ReadOnlySequence<byte> WrittenSequence => this.GetWrittenSequence();

        /// <summary>
        /// 初始化 <see cref="MultipleSegmentBufferWriter"/> 实例，使用 <see cref="BufferPool.Size128KB"/> 缓冲区池。
        /// </summary> 
        public MultipleSegmentBufferWriter()
            : this(BufferPool.Size128KB)
        {
        }

        /// <summary>
        /// 初始化 <see cref="MultipleSegmentBufferWriter"/> 实例，使用指定的缓冲区池。
        /// </summary>
        /// <param name="pool">用于租用缓冲区的 <see cref="BufferPool"/> 实例。</param>
        public MultipleSegmentBufferWriter(BufferPool pool)
        {
            ArgumentNullException.ThrowIfNull(pool);
            this._pool = pool;
        }

        /// <summary>
        /// 通知写入器已写入指定数量的字节。
        /// </summary>
        /// <param name="count">已写入的字节数。</param>
        /// <exception cref="InvalidOperationException">如果没有可用缓冲区则抛出异常。</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            if (this._lastBuffer == null)
            {
                Throw();
            }
            else
            {
                this._lastBuffer.Advance(count);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetMemory(int sizeHint = 0)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetSpan(int sizeHint = 0)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private RentedBuffer AddRentedBuffer(int sizeHint)
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);

            var buffer = new RentedBuffer(_pool, sizeHint);
            this._buffers.Add(buffer);
            return buffer;
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
        /// 获取已写入的所有缓冲区组成的只读字节序列。
        /// </summary>
        /// <returns>只读字节序列。</returns>
        private ReadOnlySequence<byte> GetWrittenSequence()
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);

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

            this._lastBuffer = null;
            this._buffers.Clear();
        }
    }
}
