using System;
using System.Buffers;

namespace RecyclableBuffer
{
    /// <summary>
    /// 提供可回收的字节数组池实现，支持高效的缓冲区复用。 
    /// </summary>
    sealed class ByteArrayPool : ArrayPool<byte>
    {
        /// <summary>
        /// 获取静态线程本地存储的桶索引。
        /// </summary>
        private readonly int _tsIndex;

        /// <summary>
        /// 每个缓冲区的字节大小。
        /// </summary>
        private readonly int _arrayLength;

        /// <summary>
        /// 用于存储可复用缓冲区的并发队列。
        /// </summary>
        private readonly ByteArrayBucket _arrayBucket;

        /// <summary>
        /// 静态线程本地存储的缓冲区桶，用于减少锁竞争。
        /// </summary>
        [ThreadStatic]
        private static byte[]?[]? _tsArrayBucket;

        /// <summary>
        /// 静态线程本地存储的桶数组长度。
        /// </summary>
        private const int TS_ARRAY_COUNT = 28;

        /// <summary>
        /// 初始化 <see cref="ByteArrayPool"/> 实例。
        /// </summary>
        /// <param name="arrayBucket">用于存储和复用字节数组的桶。</param>
        public ByteArrayPool(ByteArrayBucket arrayBucket)
        {
            this._tsIndex = arrayBucket.GetBucketIndex();
            this._arrayLength = arrayBucket.ArrayLength;
            this._arrayBucket = arrayBucket;
        }

        /// <summary>
        /// 从池中租用一个字节数组，长度至少为 <paramref name="minimumLength"/>。
        /// </summary>
        /// <param name="minimumLength">所需的最小长度。</param>
        /// <returns>租用的字节数组。</returns>
        public override byte[] Rent(int minimumLength)
        {
            // minimumLength 对应 IBufferWriter.GetMemory 和 GetSpan 传入的 sizeHint
            // 正常情况下 sizeHint 都是 0 或者一个很小的值，只要小于 _arrayLength 直接从 _arrayBucket 租用
            // 但不排除用户传入了大于 _arrayLength 的 sizeHint，这种意外情况直接从 Shared 池中租用
            if (minimumLength > this._arrayLength)
            {
                return Shared.Rent(minimumLength);
            }

            // 优先从线程本地存储的桶中租用，减少锁竞争
            var tsBucket = _tsArrayBucket;
            if (tsBucket != null)
            {
                ref var tsArrayRef = ref tsBucket[this._tsIndex];
                var tsArray = tsArrayRef;
                if (tsArray != null)
                {
                    tsArrayRef = null;
                    return tsArray;
                }
            }

            return this._arrayBucket.Rent();
        }

        /// <summary>
        /// 将字节数组归还到池中，可选择清空内容。
        /// </summary>
        /// <param name="array">要归还的字节数组。</param>
        /// <param name="clearArray">是否清空数组内容。</param>
        public override void Return(byte[] array, bool clearArray = false)
        {
            if (array.Length < this._arrayLength)
            {
                return;
            }

            if (array.Length > this._arrayLength)
            {
                Shared.Return(array, clearArray);
                return;
            }

            if (clearArray)
            {
                Array.Clear(array, 0, array.Length);
            }

            // 优先归还至线程本地存储的桶，减少锁竞争
            var tsBucket = _tsArrayBucket;
            if (tsBucket == null)
            {
                tsBucket = new byte[]?[TS_ARRAY_COUNT];
                _tsArrayBucket = tsBucket;
            }

            ref var tsArrayRef = ref tsBucket[this._tsIndex];
            var tsArray = tsArrayRef;

            // 如果线程本地存储的桶已被占用，则归还至共享桶
            if (tsArray != null)
            {
                this._arrayBucket.Return(tsArray);
            }

            // 将当前数组存入线程本地存储的桶
            tsArrayRef = array;
        }
    }
}