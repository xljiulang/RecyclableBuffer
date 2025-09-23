using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示分段缓冲区写入器的抽象基类。
    /// </summary>
    [DebuggerDisplay("Length = {Length}")]
    public abstract class SegmentBufferWriter : IBufferWriter<byte>, IDisposable
    {
        /// <summary>
        /// 获取当前写入器已写入的总字节数。
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// 获取已写入的所有缓冲区组成的只读字节序列。
        /// </summary>
        public abstract ReadOnlySequence<byte> WrittenSequence { get; }

        /// <summary>
        /// 通知写入器已向缓冲区写入指定数量的字节。
        /// </summary>
        /// <param name="count">已写入的字节数。</param>
        public abstract void Advance(int count);

        /// <summary>
        /// 获取一个可用于写入的 <see cref="Memory{Byte}"/>，可指定最小容量。
        /// </summary>
        /// <param name="sizeHint">建议的最小容量（字节数）。</param>
        /// <returns>可写入的内存块。</returns>
        public abstract Memory<byte> GetMemory(int sizeHint = 0);

        /// <summary>
        /// 获取一个可用于写入的 <see cref="Span{Byte}"/>，可指定最小容量。
        /// </summary>
        /// <param name="sizeHint">建议的最小容量（字节数）。</param>
        /// <returns>可写入的内存块。</returns>
        public abstract Span<byte> GetSpan(int sizeHint = 0);

        /// <summary>
        /// 转换成只写的 <see cref="Stream"/>。
        /// </summary>
        /// <returns>包装的 <see cref="Stream"/> 实例。</returns>
        public Stream AsWritableStream()
        {
            return new WritableStream(this);
        }

        /// <summary>
        /// 转换成只读的 <see cref="Stream"/>。
        /// </summary>
        /// <returns>包装的 <see cref="Stream"/> 实例。</returns>
        public Stream AsReadableStream()
        {
            return new ReadableStream(this.WrittenSequence);
        }

        /// <summary>
        /// 释放所有租用的缓冲区并归还到池。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 析构函数（终结器），用于释放非托管资源。
        /// </summary>
        ~SegmentBufferWriter()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 释放缓冲区资源。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected abstract void Dispose(bool disposing);
    }
}
