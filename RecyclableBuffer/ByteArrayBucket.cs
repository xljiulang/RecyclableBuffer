namespace RecyclableBuffer
{
    /// <summary>
    /// 表示用于存储和复用字节数组的桶。
    /// <para>支持线程安全的租用和归还操作</para>
    /// <para>创建后的实例需要缓存使用直到进程结束</para>
    /// </summary>
    public abstract class ByteArrayBucket
    {       
        /// <summary>
        /// 获取当前数组长度对应的桶索引
        /// </summary>
        public int BucketIndex { get; }

        /// <summary>
        /// 获取桶中每个字节数组的长度（字节）。
        /// </summary>
        public int ArrayLength { get; }

        /// <summary>
        /// 用于存储和复用字节数组的桶
        /// </summary>
        /// <param name="arrayLength">桶中每个字节数组的长度（字节）</param>
        public ByteArrayBucket(int arrayLength)
        {
            var index = SelectBucketIndex(arrayLength);

            this.BucketIndex = index;
            this.ArrayLength = GetMaxSizeForBucket(index);
        }

        /// <summary>
        /// 选择适当的桶索引以容纳指定大小的缓冲区。
        /// </summary>
        /// <param name="arrayLength"></param>
        /// <returns></returns>
        private static int SelectBucketIndex(int arrayLength)
        {
            return Log2((uint)((arrayLength - 1) | 0xF)) - 3;

            static int Log2(uint value)
            {
                var log = 0;
                while ((value >>= 1) != 0)
                {
                    log++;
                }
                return log;
            }
        }

        private static int GetMaxSizeForBucket(int bucketIndex)
        {
            return 16 << bucketIndex;
        } 

        /// <summary>
        /// 租借一个字节数组。
        /// </summary>
        /// <returns>租借到的字节数组。</returns>
        public abstract byte[] Rent();

        /// <summary>
        /// 归还一个字节数组以供复用。
        /// </summary>
        /// <param name="array">要归还的字节数组。</param>
        public abstract void Return(byte[] array); 
    }
}
