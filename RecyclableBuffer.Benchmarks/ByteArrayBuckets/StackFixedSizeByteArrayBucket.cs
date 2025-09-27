
using RecyclableBuffer;
using System.Collections.Concurrent;

sealed class StackFixedSizeByteArrayBucket : ByteArrayBucket
{
    private int _count;
    private readonly int _arrayCount;
    private readonly ConcurrentStack<byte[]> _buckets = [];

    public StackFixedSizeByteArrayBucket(int arrayLength, int arrayCount)
        : base(arrayLength)
    {
        this._arrayCount = arrayCount;
    }

    public override byte[] Rent()
    {
        if (_buckets.TryPop(out var array))
        {
            Interlocked.Decrement(ref _count);
            return array;
        }

        return new byte[ArrayLength];
    }

    public override void Return(byte[] array)
    {
        if (Interlocked.Increment(ref _count) <= _arrayCount)
        {
            _buckets.Push(array);
        }
        else
        {
            Interlocked.Decrement(ref _count);
        }
    }
}
