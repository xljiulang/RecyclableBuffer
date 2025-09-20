using System;
using System.Buffers;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示一个用于管理和复用缓冲区缓冲区的池。
    /// 提供高效的缓冲区租用与归还机制，减少频繁分配和回收带来的性能损耗。
    /// </summary>
    public class BufferPool
    {
        /// <summary>
        /// 用于实际租用和归还数组的 <see cref="ArrayPool{T}"/> 实例。
        /// </summary>
        private readonly ArrayPool<byte> _pool;

        /// <summary>
        /// 获取允许使用池中的最小缓冲区的大小（字节）。
        /// </summary>
        public int MinBufferSize { get; }

        /// <summary>
        /// 获取允许使用池中的最大缓冲区的大小（字节）。
        /// </summary>
        public int MaxBufferSize { get; }

        /// <summary>
        /// 获取每个桶允许的最大缓冲区数量。
        /// </summary>
        public int? MaxBuffersPerBucket { get; }

        /// <summary>
        /// 获取一个共享的 <see cref="BufferPool"/> 实例，适用于大多数通用场景。
        /// </summary>
        public static BufferPool Shared { get; } = new BufferPool(8 * 1024, 2 * 1024 * 1024, Environment.ProcessorCount, ArrayPool<byte>.Shared);

        /// <summary>
        /// 获取一个默认配置的 <see cref="BufferPool"/> 实例，适用于高并发场景。
        /// </summary>
        public static BufferPool Default { get; } = new BufferPool(8 * 1024, 2 * 1024 * 1024, 64);

        /// <summary>
        /// 使用指定的最小缓冲区大小、最大缓冲区大小和每个桶的最大缓冲区数量初始化 <see cref="BufferPool"/>。
        /// </summary>
        /// <param name="minBufferSize">允许的最小缓冲区大小（字节）。</param>
        /// <param name="maxBufferSize">允许的最大缓冲区大小（字节）。</param>
        /// <param name="maxBuffersPerBucket">每个桶允许的最大缓冲区数量。</param>
        public BufferPool(int minBufferSize, int maxBufferSize, int maxBuffersPerBucket)
            : this(minBufferSize, maxBufferSize, maxBuffersPerBucket, ArrayPool<byte>.Create(maxBufferSize, maxBuffersPerBucket))
        {
        }

        /// <summary>
        /// 使用指定的最小缓冲区大小、最大缓冲区大小、每个桶的最大缓冲区数量和底层 <see cref="ArrayPool{T}"/> 实例初始化 <see cref="BufferPool"/>。
        /// </summary>
        /// <param name="minBufferSize">允许的最小缓冲区大小（字节）。</param>
        /// <param name="maxBufferSize">允许的最大缓冲区大小（字节）。</param>
        /// <param name="maxBuffersPerBucket">每个桶允许的最大缓冲区数量。</param>
        /// <param name="pool">底层用于实际租用和归还数组的 <see cref="ArrayPool{T}"/> 实例。</param>
        public BufferPool(int minBufferSize, int maxBufferSize, int? maxBuffersPerBucket, ArrayPool<byte> pool)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(minBufferSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxBufferSize);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minBufferSize, maxBufferSize);

            this.MinBufferSize = minBufferSize;
            this.MaxBufferSize = maxBufferSize;
            this.MaxBuffersPerBucket = maxBuffersPerBucket;
            this._pool = pool;
        }

        /// <summary>
        /// 从缓冲区池中租用一个至少具有指定最小大小的缓冲区。
        /// </summary>
        /// <param name="minimumSize">所需的最小缓冲区大小（字节）。</param>
        /// <returns>租用的缓冲区。</returns>
        public byte[] Rent(int minimumSize)
        {
            return this._pool.Rent(minimumSize);
        }

        /// <summary>
        /// 将指定的缓冲区归还到缓冲区池中以供复用。
        /// </summary>
        /// <param name="buffer">要归还的缓冲区。</param>
        public void Return(byte[] buffer)
        {
            this._pool.Return(buffer);
        }

        /// <summary>
        /// 选择适合当前池配置的随机小缓冲区和大缓冲区大小。
        /// </summary>
        /// <returns>包含小缓冲区和大缓冲区大小的 <see cref="BufferSizes"/> 结构体。</returns>
        public virtual BufferSizes SelectBufferSizes()
        {
            // 7:3切分点
            var criticalSize = (this.MinBufferSize + this.MaxBufferSize) * 7 / 20;
            // 使用随机缓冲区大小，是为了降低多个RecyclableBufferWriter实例的缓冲区大小过于集中在某个固定值导致爆桶的几率
            var smallBufferSize = Random.Shared.Next(this.MinBufferSize, criticalSize);
            var largeBufferSize = Random.Shared.Next(criticalSize, this.MaxBufferSize);
            return new BufferSizes
            {
                SmallSize = smallBufferSize,
                LargeSize = largeBufferSize
            };
        }


        /// <summary>
        /// 获取一个 <see cref="RecyclableBufferWriter"/> 实例，使用当前缓冲区池进行管理。
        /// </summary>
        /// <returns>新创建的 <see cref="RecyclableBufferWriter"/> 实例。</returns>
        public RecyclableBufferWriter GetBufferWriter()
        {
            return new RecyclableBufferWriter(this);
        }
    }
}
