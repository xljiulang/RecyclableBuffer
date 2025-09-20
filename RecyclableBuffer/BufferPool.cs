using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Numerics;

namespace RecyclableBuffer
{
    /// <summary>
    /// 提供可回收的字节数组池实现，支持高效的缓冲区复用。
    /// </summary>
    public sealed class BufferPool : ArrayPool<byte>
    {
        /// <summary>
        /// 缓冲区的标准大小。
        /// </summary>
        private readonly int _bufferSize;

        /// <summary>
        /// 用于存储可复用缓冲区的并发队列。
        /// </summary>
        private readonly ConcurrentQueue<byte[]> _buffers = [];

        /// <summary>
        /// 获取一个使用 128KB 缓冲区大小的 <see cref="BufferPool"/> 实例。
        /// </summary>
        public static BufferPool Size128KB { get; } = new BufferPool(128 * 1024);

        /// <summary>
        /// 初始化 <see cref="BufferPool"/> 实例，并指定缓冲区大小。
        /// </summary>
        /// <param name="bufferSize">缓冲区的大小。</param>
        public BufferPool(int bufferSize)
        {
            var index = BitOperations.Log2((uint)(bufferSize - 1) | 0xFu) - 3;
            this._bufferSize = 16 << index;
        }

        /// <summary>
        /// 从池中租用一个字节数组，长度至少为 <paramref name="minimumLength"/>。
        /// </summary>
        /// <param name="minimumLength">所需的最小长度。</param>
        /// <returns>租用的字节数组。</returns>
        public override byte[] Rent(int minimumLength)
        {
            if (minimumLength > this._bufferSize)
            {
                return Shared.Rent(minimumLength);
            }

            if (this._buffers.TryDequeue(out var array))
            {
                return array;
            }

            return new byte[this._bufferSize];
        }

        /// <summary>
        /// 将字节数组归还到池中，可选择清空内容。
        /// </summary>
        /// <param name="array">要归还的字节数组。</param>
        /// <param name="clearArray">是否清空数组内容。</param>
        public override void Return(byte[] array, bool clearArray = false)
        {
            if (array.Length < this._bufferSize)
            {
                return;
            }

            if (array.Length > this._bufferSize)
            {
                Shared.Return(array, clearArray);
                return;
            }

            if (clearArray)
            {
                Array.Clear(array);
            }
            this._buffers.Enqueue(array);
        }
    }
}
