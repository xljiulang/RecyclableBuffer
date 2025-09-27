
using RecyclableBuffer;
using System.Diagnostics;

/// <summary>
/// 可扩展字节数组桶，支持自动扩容和线程安全的租用与归还操作。
/// </summary>
sealed class SpinLockScalableByteArrayBucket : ByteArrayBucket
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

    /// <summary>
    /// 初始化 <see cref="ScalableByteArrayBucket"/> 实例。
    /// </summary>
    /// <param name="arrayLength">每个数组的长度。</param>
    public SpinLockScalableByteArrayBucket(int arrayLength)
        : base(arrayLength)
    {
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

            ref var arrayRef = ref this._buffers[this._index];
            array = arrayRef;
            arrayRef = null;
            this._index += 1;
        }
        finally
        {
            if (lockTaken)
            {
                this._lock.Exit(false);
            }
        }

        return array ?? new byte[this.ArrayLength];
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
                this._index -= 1;
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