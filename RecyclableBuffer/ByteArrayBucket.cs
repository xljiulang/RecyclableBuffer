using System.Collections.Concurrent;
using System.Threading;

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
        /// 获取当前数组长度对应的桶索引。
        /// </summary>
        public int BucketIndex { get; }

        /// <summary>
        /// 获取桶中每个字节数组的长度（字节）。
        /// </summary>
        public int ArrayLength { get; }

        /// <summary>
        /// 用于存储和复用字节数组的桶。
        /// </summary>
        /// <param name="arrayLength">桶中每个字节数组的长度（字节）。</param>
        public ByteArrayBucket(int arrayLength)
        {
            var index = SelectBucketIndex(arrayLength);

            this.BucketIndex = index;
            this.ArrayLength = GetMaxSizeForBucket(index);
        }

        /// <summary>
        /// 选择适当的桶索引以容纳指定大小的缓冲区。
        /// </summary>
        /// <param name="arrayLength">目标缓冲区长度（字节）。</param>
        /// <returns>桶索引。</returns>
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

        /// <summary>
        /// 获取指定桶索引对应的最大字节数组长度。
        /// </summary>
        /// <param name="bucketIndex">桶索引。</param>
        /// <returns>最大字节数组长度。</returns>
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

        /// <summary>
        /// 创建一个可扩展的字节数组桶实例。
        /// </summary>
        /// <param name="arrayLength">桶中每个字节数组的长度（字节）。</param>
        /// <returns>可扩展的字节数组桶实例。</returns>
        public static ByteArrayBucket Create(int arrayLength)
        {
            return new ScalableByteArrayBucket(arrayLength);
        }

        /// <summary>
        /// 创建一个固定大小的字节数组桶实例。
        /// </summary>
        /// <param name="arrayLength">桶中每个字节数组的长度（字节）。</param>
        /// <param name="arrayCount">桶中最大可存储的字节数组数量。</param>
        /// <returns>固定大小的字节数组桶实例。</returns>
        public static ByteArrayBucket Create(int arrayLength, int arrayCount)
        {
            return new FixedSizeByteArrayBucket(arrayLength, arrayCount);
        }

        /// <summary>
        /// 使用 <see cref="ConcurrentStack{T}"/> 实现的可扩展字节数组桶。 
        /// </summary>
        private sealed class ScalableByteArrayBucket : ByteArrayBucket
        {
            /// <summary>
            /// 存储可复用的字节数组缓冲区的线程安全栈。
            /// </summary>
            private readonly ConcurrentStack<byte[]> _arrayStack = [];

            /// <summary>
            /// 初始化 <see cref="ScalableByteArrayBucket"/> 实例。
            /// </summary>
            /// <param name="arrayLength">每个数组的长度（字节）。</param>
            public ScalableByteArrayBucket(int arrayLength)
                : base(arrayLength)
            {
            }

            /// <summary>
            /// 从桶中租用一个字节数组，如果桶为空则新建一个。
            /// </summary>
            /// <returns>租借到的字节数组。</returns>
            public override byte[] Rent()
            {
                return this._arrayStack.TryPop(out var array) ? array : new byte[this.ArrayLength];
            }

            /// <summary>
            /// 将字节数组归还到桶中，供后续复用。
            /// </summary>
            /// <param name="array">要归还的字节数组。</param>
            public override void Return(byte[] array)
            {
                this._arrayStack.Push(array);
            }
        }

        /// <summary>
        /// 使用 <see cref="ConcurrentStack{T}"/> 实现的固定大小的字节数组桶。 
        /// <para>当桶已满时，归还的数组将被丢弃。</para>
        /// </summary>
        private sealed class FixedSizeByteArrayBucket : ByteArrayBucket
        {
            /// <summary>
            /// 当前桶中已存储的字节数组数量。
            /// </summary>
            private int _count;

            /// <summary>
            /// 桶允许存储的最大字节数组数量。
            /// </summary>
            private readonly int _arrayCount;

            /// <summary>
            /// 用于存储可复用字节数组的线程安全栈。
            /// </summary>
            private readonly ConcurrentStack<byte[]> _arrayStack = [];

            /// <summary>
            /// 初始化 <see cref="FixedSizeByteArrayBucket"/> 实例。
            /// </summary>
            /// <param name="arrayLength">每个字节数组的长度（字节）。</param>
            /// <param name="arrayCount">桶中最大可存储的字节数组数量。</param>
            public FixedSizeByteArrayBucket(int arrayLength, int arrayCount)
                : base(arrayLength)
            {
                this._arrayCount = arrayCount;
            }

            /// <summary>
            /// 从桶中租用一个字节数组。如果桶中有可用数组则返回，否则新建一个数组。
            /// </summary>
            /// <returns>租借到的字节数组。</returns>
            public override byte[] Rent()
            {
                if (this._arrayStack.TryPop(out var array))
                {
                    Interlocked.Decrement(ref this._count);
                    return array;
                }

                return new byte[this.ArrayLength];
            }

            /// <summary>
            /// 将字节数组归还到桶中以供复用。如果桶已满则丢弃该数组。
            /// </summary>
            /// <param name="array">要归还的字节数组。</param>
            public override void Return(byte[] array)
            {
                if (Interlocked.Increment(ref this._count) <= this._arrayCount)
                {
                    this._arrayStack.Push(array);
                }
                else
                {
                    Interlocked.Decrement(ref this._count);
                }
            }
        }
    }
}
