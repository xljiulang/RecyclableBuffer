using System.Collections.Concurrent;
using System.Threading;

namespace RecyclableBuffer.Buckets
{
    /// <summary>
    /// 使用 <see cref="ConcurrentStack{T}"/> 实现的固定大小的字节数组桶。 
    /// <para>当桶已满时，归还的数组将被丢弃。</para>
    /// </summary>
    public sealed class FixedSizeStackByteArrayBucket : ByteArrayBucket
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
        private readonly ConcurrentStack<byte[]> _buffers = [];

        /// <summary>
        /// 初始化 <see cref="FixedSizeStackByteArrayBucket"/> 实例。
        /// </summary>
        /// <param name="arrayLength">每个字节数组的长度（字节）。</param>
        /// <param name="arrayCount">桶中最大可存储的字节数组数量。</param>
        public FixedSizeStackByteArrayBucket(int arrayLength, int arrayCount)
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
            if (_buffers.TryPop(out var array))
            {
                Interlocked.Decrement(ref _count);
                return array;
            }

            return new byte[ArrayLength];
        }

        /// <summary>
        /// 将字节数组归还到桶中以供复用。如果桶已满则丢弃该数组。
        /// </summary>
        /// <param name="array">要归还的字节数组。</param>
        public override void Return(byte[] array)
        {
            if (Interlocked.Increment(ref _count) <= _arrayCount)
            {
                _buffers.Push(array);
            }
            else
            {
                Interlocked.Decrement(ref _count);
            }
        }
    }
}