using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace RecyclableBuffer
{
    /// <summary>
    /// 提供可回收的字节数组池实现，支持高效的缓冲区复用。
    /// </summary>
    [DebuggerDisplay("ArrayLength = {_arrayLength}, ArrayCount = {_arrayCount}")]
    public class ByteArrayPool : ArrayPool<byte>
    {
        private int _arrayCount;

        /// <summary>
        /// 每个缓冲区的字节大小。
        /// </summary>
        private readonly int _arrayLength;

        /// <summary>
        /// 池中最大缓冲区数量的可选限制。
        /// </summary>
        private readonly int? _maxArrayCount;

        /// <summary>
        /// 用于存储可复用缓冲区的并发队列。
        /// </summary>
        private readonly ConcurrentQueue<byte[]> _arrayBucket = [];


        /// <summary>
        /// 获取一个使用 128KB 缓冲区大小且不限制数量的 <see cref="ByteArrayPool"/> 实例。
        /// </summary>
        public static ByteArrayPool Default { get; } = new ByteArrayPool(128 * 1024, null);

        /// <summary>
        /// 初始化 <see cref="ByteArrayPool"/> 实例，并指定缓冲区大小。
        /// </summary>
        /// <param name="arrayLength">每个缓冲区的字节大小。</param>
        /// <param name="maxArrayCount">池中最大缓冲区数量的可选限制。</param>
        public ByteArrayPool(int arrayLength, int? maxArrayCount)
        {
            var index = BitOperations.Log2((uint)(arrayLength - 1) | 0xFu) - 3;
            this._arrayLength = 16 << index;
            this._maxArrayCount = maxArrayCount;
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

            if (this._arrayBucket.TryDequeue(out var array))
            {
                Interlocked.Decrement(ref this._arrayCount);
                return array;
            }

            return new byte[this._arrayLength];
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

            if (Interlocked.Increment(ref this._arrayCount) > this._maxArrayCount)
            {
                Interlocked.Decrement(ref this._arrayCount);
            }
            else
            {
                this._arrayBucket.Enqueue(array);
            }
        }
    }
}
