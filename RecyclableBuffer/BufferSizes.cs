using System.Numerics;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示缓冲区大小的配置选项。
    /// </summary>
    public struct BufferSizes
    {
        private int _largeSize;
        private int _smallSize;

        /// <summary>
        /// 获取用于首个分片的大缓冲区大小，单位为字节。
        /// 大缓冲区能降低小缓冲区数量过多而爆桶的几率，同时也能让数据尽量在单一数据块上。
        /// </summary>
        public int LargeSize
        {
            get => _largeSize;
            set { _largeSize = GetMaxSizeForBucket(value); }
        }

        /// <summary>
        /// 获取用于后续分片的小缓冲区大小，单位为字节。
        /// </summary>
        public int SmallSize
        {
            get => _smallSize;
            set { _smallSize = GetMaxSizeForBucket(value); }
        }

        private static int GetMaxSizeForBucket(int bufferSize)
        {
            var index = BitOperations.Log2((uint)(bufferSize - 1) | 0xFu) - 3;
            return 16 << index;
        }
    }
}
