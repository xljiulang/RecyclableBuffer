using System.Collections.Concurrent;

namespace RecyclableBuffer.Buckets
{
    /// <summary>
    /// 使用 <see cref="ConcurrentStack{T}"/> 实现的可扩展字节数组桶。 
    /// </summary>
    public sealed class ScalableStackByteArrayBucket : ByteArrayBucket
    {
        /// <summary>
        /// 存储可复用的字节数组缓冲区的线程安全栈。
        /// </summary>
        private readonly ConcurrentStack<byte[]> _buffers = [];

        /// <summary>
        /// 初始化 <see cref="ScalableStackByteArrayBucket"/> 实例。
        /// </summary>
        /// <param name="arrayLength">每个数组的长度（字节）。</param>
        public ScalableStackByteArrayBucket(int arrayLength)
            : base(arrayLength)
        {
        }

        /// <summary>
        /// 从桶中租用一个字节数组，如果桶为空则新建一个。
        /// </summary>
        /// <returns>租借到的字节数组。</returns>
        public override byte[] Rent()
        {
            return _buffers.TryPop(out var array) ? array : new byte[ArrayLength];
        }

        /// <summary>
        /// 将字节数组归还到桶中，供后续复用。
        /// </summary>
        /// <param name="array">要归还的字节数组。</param>
        public override void Return(byte[] array)
        {
            _buffers.Push(array);
        }
    }
}