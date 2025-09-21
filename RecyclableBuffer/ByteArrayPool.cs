using System;
using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace RecyclableBuffer
{
    /// <summary>
    /// 提供可回收的字节数组池实现，支持高效的缓冲区复用。
    /// <para>维护一个固定缓冲区大小且可扩容的桶，承载所有小于等于此缓冲区大小的 Rent 和 Return 操作</para>
    /// <para>由 Shared 实例承载大于此缓冲区大小的 Rent 和 Return 操作</para>
    /// </summary>
    [DebuggerDisplay("Capacity = {_arrayBucket.Capacity}")]
    public sealed class ByteArrayPool : ArrayPool<byte>
    {
        /// <summary>
        /// 每个缓冲区的字节大小。
        /// </summary>
        private readonly int _arrayLength;

        /// <summary>
        /// 用于存储可复用缓冲区的并发队列。
        /// </summary>
        private readonly ArrayBucket _arrayBucket = new();

        /// <summary>
        /// 获取一个使用 128KB 缓冲区大小且不限制数量的 <see cref="ByteArrayPool"/> 实例。
        /// </summary>
        public static ByteArrayPool Default { get; } = new ByteArrayPool(128 * 1024);

        /// <summary>
        /// 初始化 <see cref="ByteArrayPool"/> 实例，并指定缓冲区大小。
        /// </summary>
        /// <param name="arrayLength">每个缓冲区的字节大小。</param>
        public ByteArrayPool(int arrayLength)
        {
            var index = BitOperations.Log2((uint)(arrayLength - 1) | 0xFu) - 3;
            this._arrayLength = 16 << index;
        }

        /// <summary>
        /// 从池中租用一个字节数组，长度至少为 <paramref name="minimumLength"/>。
        /// </summary>
        /// <param name="minimumLength">所需的最小长度。</param>
        /// <returns>租用的字节数组。</returns>
        public override byte[] Rent(int minimumLength)
        {
            if (minimumLength > this._arrayLength)
            {
                return Shared.Rent(minimumLength);
            }
            else
            {
                return this._arrayBucket.Rent() ?? new byte[this._arrayLength];
            }
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
                Array.Clear(array);
            }
            this._arrayBucket.Return(array);
        }


        /// <summary>
        /// 表示用于存储和复用字节数组的桶，支持线程安全的租用和归还操作。
        /// </summary>
        [DebuggerDisplay("Capacity = {Capacity}")]
        private sealed class ArrayBucket
        {
            /// <summary>
            /// 当前桶中可用缓冲区的索引。
            /// </summary>
            private int _index = 0;

            /// <summary>
            /// 用于保护缓冲区数组的自旋锁。
            /// </summary>
            private SpinLock _lock = new SpinLock(Debugger.IsAttached);

            /// <summary>
            /// 存储可复用的字节数组缓冲区。
            /// </summary>
            private byte[]?[] _buffers = new byte[Environment.ProcessorCount][];

            /// <summary>
            /// 获取当前桶的容量（缓冲区数组长度）。
            /// </summary>
            public int Capacity => this._buffers.Length;

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

                    if (this._index >= this._buffers.Length)
                    {
                        Array.Resize(ref this._buffers, this._buffers.Length * 2);
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