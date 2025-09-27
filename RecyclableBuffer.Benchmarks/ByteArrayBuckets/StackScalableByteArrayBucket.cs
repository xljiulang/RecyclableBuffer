
using RecyclableBuffer;
using System.Collections.Concurrent;

sealed class StackScalableByteArrayBucket : ByteArrayBucket
{
    private readonly ConcurrentStack<byte[]> _buckets = [];

    public StackScalableByteArrayBucket(int arrayLength)
        : base(arrayLength)
    {
    }

    public override byte[] Rent()
    {
        if (_buckets.TryPop(out var array))
        {
            return array;
        }

        return new byte[ArrayLength];
    }

    public override void Return(byte[] array)
    {
        _buckets.Push(array);
    }
}
