using System;
using System.Buffers;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示一个可回收的字节数组池，基于 <see cref="ArrayPool{T}"/> 实现。
    /// 用于高效地租用和归还字节数组，减少内存分配和垃圾回收压力。
    /// </summary>
    public sealed class BufferPool
    {
        private readonly ArrayPool<byte> _pool;

        /// <summary>
        /// 获取池中允许的最小数组长度。
        /// </summary>
        public int MinArrayLength { get; }

        /// <summary>
        /// 获取池中允许的最大数组长度。
        /// </summary>
        public int MaxArrayLength { get; }

        /// <summary>
        /// 获取每个桶允许的最大数组数量。
        /// </summary>
        public int MaxArraysPerBucket { get; }

        /// <summary>
        /// 获取共享的 <see cref="BufferPool"/> 实例。
        /// 默认最小数组长度为 8KB，最大数组长度为 1MB，每个桶最多 50 个数组。
        /// </summary>
        public static BufferPool Shared { get; } = new BufferPool(8 * 1024, 1024 * 1024, 50, ArrayPool<byte>.Shared);

        /// <summary>
        /// 初始化 <see cref="BufferPool"/> 的新实例。
        /// </summary>
        /// <param name="minArrayLength">池中允许的最小数组长度</param>
        /// <param name="maxArrayLength">池中允许的最大数组长度。</param>
        /// <param name="maxArraysPerBucket">每个桶允许的最大数组数量。</param>
        public BufferPool(int minArrayLength, int maxArrayLength, int maxArraysPerBucket)
            : this(minArrayLength, maxArrayLength, maxArraysPerBucket, ArrayPool<byte>.Create(maxArrayLength, maxArraysPerBucket))
        {
        }

        private BufferPool(int minArrayLength, int maxArrayLength, int maxArraysPerBucket, ArrayPool<byte> pool)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minArrayLength, maxArrayLength);

            this.MinArrayLength = minArrayLength;
            this.MaxArrayLength = maxArrayLength;
            this.MaxArraysPerBucket = maxArraysPerBucket;
            this._pool = pool;
        }

        /// <summary>
        /// 从池中租用一个至少指定长度的字节数组。
        /// </summary>
        /// <param name="minimumLength">字节数组的最小长度。</param>
        /// <returns>租用的字节数组。</returns>
        public byte[] Rent(int minimumLength)
        { 
            return this._pool.Rent(minimumLength);
        }

        /// <summary>
        /// 将字节数组归还到池中。
        /// </summary>
        /// <param name="buffer">要归还的字节数组。</param>
        public void Return(byte[] buffer)
        {
            this._pool.Return(buffer);
        }
    }
}
