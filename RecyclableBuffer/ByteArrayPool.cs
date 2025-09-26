using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RecyclableBuffer
{
    /// <summary>
    /// 提供可回收的字节数组池实现，支持高效的缓冲区复用。
    /// <para>维护一个固定缓冲区大小且可扩容的桶，承载所有小于等于此缓冲区大小的 Rent 和 Return 操作</para>
    /// <para>由 Shared 实例承载大于此缓冲区大小的 Rent 和 Return 操作</para>
    /// </summary>
    public sealed class ByteArrayPool : ArrayPool<byte>
    {
        /// <summary>
        /// 获取静态线程本地存储的桶索引。
        /// </summary>
        private readonly int _stIndex;

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
        private static byte[]?[]? _stArrayBucket;

        /// <summary>
        /// 静态线程本地存储的桶数组长度。
        /// </summary>
        private static readonly int _stArrayCount = SelectBucketIndex(int.MaxValue) + 1;

        /// <summary>
        /// 获取一个使用 128KB 缓冲区大小且不限制数量的 <see cref="ByteArrayPool"/> 实例。
        /// </summary>
        public static ByteArrayPool Default { get; } = new ByteArrayPool(128 * 1024);

        /// <summary>
        /// 初始化 <see cref="ByteArrayPool"/> 实例，并指定缓冲区大小。
        /// </summary>
        /// <param name="arrayLength">每个缓冲区的字节大小。尝试租用大于此值的缓冲区时将从<see cref="ArrayPool{Byte}.Shared"/> 中获取</param>
        /// <param name="maxArrayCount">缓冲区的最高数量限制，null 表示不限制</param>
        public ByteArrayPool(int arrayLength, int? maxArrayCount = null)
        {
            var index = SelectBucketIndex(arrayLength);

            this._stIndex = index;
            this._arrayLength = 16 << index;
            this._arrayBucket = new ByteArrayBucket(maxArrayCount);
        }

        /// <summary>
        /// 选择适当的桶索引以容纳指定大小的缓冲区。
        /// </summary>
        /// <param name="arrayLength"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            var stBucket = _stArrayBucket;
            if (stBucket != null)
            {
                ref var stArrayRef = ref stBucket[this._stIndex];
                var stArray = stArrayRef;
                if (stArray != null)
                {
                    stArrayRef = null;
                    return stArray;
                }
            }

            return this._arrayBucket.Rent() ?? new byte[this._arrayLength];

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
            var stBucket = _stArrayBucket;
            if (stBucket == null)
            {
                stBucket = new byte[]?[_stArrayCount];
                _stArrayBucket = stBucket;
            }

            ref var stArrayRef = ref stBucket[this._stIndex];
            var stArray = stArrayRef;

            // 如果线程本地存储的桶已被占用，则归还至共享桶
            if (stArray != null)
            {
                this._arrayBucket.Return(stArray);
            }

            // 将当前数组存入线程本地存储的桶
            stArrayRef = array;
        }


        /// <summary>
        /// 表示用于存储和复用字节数组的桶，支持线程安全的租用和归还操作。
        /// </summary>
        private sealed class ByteArrayBucket
        {
            /// <summary>
            /// 缓冲区的最高数量限制
            /// </summary>
            private readonly int _maxCapacity;

            /// <summary>
            /// 当前桶中可用缓冲区的索引。
            /// </summary>
            private int _index = 0;

            /// <summary>
            /// 用于保护缓冲区数组的自旋锁。
            /// </summary>
            private SpinLock _lock = new(Debugger.IsAttached);

            /// <summary>
            /// 存储可复用的字节数组缓冲区。
            /// </summary>
            private byte[]?[] _buffers; 

            /// <summary>
            /// 初始化 <see cref="ByteArrayBucket"/> 实例。
            /// </summary>
            /// <param name="maxCapacity">
            /// 缓冲区的最高数量限制，null 表示不限制。
            /// </param>
            public ByteArrayBucket(int? maxCapacity)
            {
                if (maxCapacity > 0)
                {
                    this._maxCapacity = maxCapacity.Value;
                    this._buffers = new byte[Math.Min(maxCapacity.Value, Environment.ProcessorCount)][];
                }
                else
                {
                    this._maxCapacity = int.MaxValue;
                    this._buffers = new byte[Environment.ProcessorCount][];
                }
            }


            /// <summary>
            /// 从桶中租用一个字节数组，如果桶已满则自动扩容。
            /// </summary>
            /// <returns>可用的字节数组，如果没有则返回 null。</returns>
            public byte[]? Rent()
            {
                var lockTaken = false;
                var array = default(byte[]);

                try
                {
                    this._lock.Enter(ref lockTaken);

                    var capacity = this._buffers.Length;
                    if (this._index < capacity)
                    {
                        array = this._buffers[this._index];
                        this._buffers[_index++] = null;
                    }
                    else if (capacity < this._maxCapacity)
                    {
                        var newSize = Math.Min(capacity * 2, this._maxCapacity);
                        Array.Resize(ref this._buffers, newSize);

                        array = this._buffers[this._index];
                        this._buffers[_index++] = null;
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        this._lock.Exit(false);
                    }
                }

                return array;
            }



            /// <summary>
            /// 将字节数组归还到桶中，供后续复用。
            /// </summary>
            /// <param name="array">要归还的字节数组。</param>
            public void Return(byte[] array)
            {
                bool lockTaken = false;
                try
                {
                    this._lock.Enter(ref lockTaken);
                    if (this._index != 0)
                    {
                        this._index--;
                        this._buffers[this._index] = array;
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        this._lock.Exit(false);
                    }
                }
            }
        }
    }
}