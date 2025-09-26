using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示用于存储和复用字节数组的桶，支持线程安全的租用和归还操作。
    /// </summary>
    public abstract class ByteArrayBucket
    {
        /// <summary>
        /// 默认可扩展桶，初始每个数组长度为 128KB。
        /// </summary>
        public static readonly ByteArrayBucket DefaultScalable = new ScalableByteArrayBucket(128 * 1024);

        /// <summary>
        /// 获取桶中每个字节数组的长度（字节）。
        /// </summary>
        public abstract int ArrayLength { get; }

        /// <summary>
        /// 获取当前数组长度对应的桶索引。
        /// </summary>
        public int GetBucketIndex()
        {
            return SelectBucketIndex(this.ArrayLength);
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
        /// 获取指定数组长度对应的最大数组长度。
        /// </summary>
        /// <param name="arrayLength"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected internal static int GetMaxArrayLength(int arrayLength)
        {
            var index = SelectBucketIndex(arrayLength);
            return 16 << index;
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
        /// 创建一个可扩展的字节数组桶。
        /// </summary>
        /// <param name="arrayLength">每个数组的长度。</param>
        /// <returns>可扩展字节数组桶实例。</returns>
        public static ByteArrayBucket CreateScalable(int arrayLength)
        {
            return new ScalableByteArrayBucket(arrayLength);
        }

        /// <summary>
        /// 创建一个固定大小的字节数组桶，使用指定最大数组数量。
        /// </summary>
        /// <param name="arrayLength">每个数组的长度。</param>
        /// <param name="arrayCount">数组数量。</param>
        /// <returns>固定大小字节数组桶实例。</returns>
        public static ByteArrayBucket CreateFixedSize(int arrayLength, int arrayCount)
        {
            return new FixedSizeByteArrayBucket(arrayLength, arrayCount);
        }

        /// <summary>
        /// 固定大小字节数组桶，使用指定最大数组数量的 <see cref="ArrayPool{T}"/> 实现。
        /// </summary>
        private sealed class FixedSizeByteArrayBucket : ByteArrayBucket
        {
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
            private readonly byte[]?[] _buffers;


            private readonly int _arrayLength;

            /// <inheritdoc/>
            public override int ArrayLength => this._arrayLength;

            /// <summary>
            /// 初始化 <see cref="ScalableByteArrayBucket"/> 实例。
            /// </summary>
            /// <param name="arrayLength">每个数组的长度。</param>
            /// <param name="arrayCount">数组数量。</param>
            public FixedSizeByteArrayBucket(int arrayLength, int arrayCount)
            {
                this._buffers = new byte[arrayCount][];
                this._arrayLength = GetMaxArrayLength(arrayLength);
            }

            /// <summary>
            /// 从桶中租用一个字节数组，如果桶已满则自动扩容。
            /// </summary>
            /// <returns>租借到的字节数组。</returns>
            public override byte[] Rent()
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
                }
                finally
                {
                    if (lockTaken)
                    {
                        this._lock.Exit(false);
                    }
                }

                return array ?? new byte[this._arrayLength];
            }

            /// <summary>
            /// 将字节数组归还到桶中，供后续复用。
            /// </summary>
            /// <param name="array">要归还的字节数组。</param>
            public override void Return(byte[] array)
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

        /// <summary>
        /// 可扩展字节数组桶，支持自动扩容和线程安全的租用与归还操作。
        /// </summary>
        private sealed class ScalableByteArrayBucket : ByteArrayBucket
        {
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
            private byte[]?[] _buffers = new byte[Environment.ProcessorCount][];

            private readonly int _arrayLength;

            /// <inheritdoc/>
            public override int ArrayLength => this._arrayLength;

            /// <summary>
            /// 初始化 <see cref="ScalableByteArrayBucket"/> 实例。
            /// </summary>
            /// <param name="arrayLength">每个数组的长度。</param>
            public ScalableByteArrayBucket(int arrayLength)
            {
                this._arrayLength = GetMaxArrayLength(arrayLength);
            }

            /// <summary>
            /// 从桶中租用一个字节数组，如果桶已满则自动扩容。
            /// </summary>
            /// <returns>租借到的字节数组。</returns>
            public override byte[] Rent()
            {
                var lockTaken = false;
                var array = default(byte[]);

                try
                {
                    this._lock.Enter(ref lockTaken);

                    var capacity = this._buffers.Length;
                    if (this._index >= capacity)
                    {
                        Array.Resize(ref this._buffers, capacity * 2);
                    }

                    array = this._buffers[this._index];
                    this._buffers[_index++] = null;
                }
                finally
                {
                    if (lockTaken)
                    {
                        this._lock.Exit(false);
                    }
                }

                return array ?? new byte[this._arrayLength];
            }

            /// <summary>
            /// 将字节数组归还到桶中，供后续复用。
            /// </summary>
            /// <param name="array">要归还的字节数组。</param>
            public override void Return(byte[] array)
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
