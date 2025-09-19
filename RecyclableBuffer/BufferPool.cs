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
        /// 获取池中允许的最大数组长度。
        /// </summary>
        public int MaxArrayLength { get; }

        /// <summary>
        /// 获取每个桶允许的最大数组数量。
        /// </summary>
        public int MaxArraysPerBucket { get; }

        /// <summary>
        /// 获取默认的 <see cref="BufferPool"/> 实例。
        /// 默认最大数组长度为 128KB，每个桶最多 100 个数组。
        /// </summary>
        public static BufferPool Default { get; } = new BufferPool(128 * 1024, 100);

        /// <summary>
        /// 初始化 <see cref="BufferPool"/> 的新实例。
        /// </summary>
        /// <param name="maxArrayLength">池中允许的最大数组长度。</param>
        /// <param name="maxArraysPerBucket">每个桶允许的最大数组数量。</param>
        public BufferPool(int maxArrayLength, int maxArraysPerBucket)
        {
            this.MaxArrayLength = maxArrayLength;
            this.MaxArraysPerBucket = maxArraysPerBucket;
            this._pool = ArrayPool<byte>.Create(maxArrayLength, maxArraysPerBucket);
        }

        /// <summary>
        /// 从池中租用一个至少指定长度的字节数组。
        /// </summary>
        /// <param name="minimumLength">所需的最小数组长度。</param>
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
