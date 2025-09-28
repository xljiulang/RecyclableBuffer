using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using Microsoft.IO;
using RecyclableBuffer.Buckets;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class WriteBufferBenchmark
    {
        const int ARRAY_LENGTH = 128 * 1024;
        private static readonly RecyclableMemoryStreamManager manager = new();
        private static readonly ByteArrayBucket fixedSizeSpinLock = new FixedSizeSpinLockByteArrayBucket(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket fixedSizeStack = new FixedSizeStackByteArrayBucket(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket scalableSpinLock = new ScalableSpinLockByteArrayBucket(ARRAY_LENGTH);
        private static readonly ByteArrayBucket scalableStack = new ScalableStackByteArrayBucket(ARRAY_LENGTH);
        private static readonly ArrayPool<byte> configurableArrayPool = ArrayPool<byte>.Create(ARRAY_LENGTH, 100);

        [Params(1024, 8 * 1024, 512 * 1024)]
        public int BufferSize = 1024 * 1024;

        private byte[] buffer = [];

        [GlobalSetup]
        public void Setup()
        {
            this.buffer = new byte[this.BufferSize];
        }

        [Benchmark]
        public void SingleSegmentBufferWriter()
        {
            using var target = new SingleSegmentBufferWriter(ARRAY_LENGTH);
            WriteBuffer(target);
        }

        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter_Shared()
        {
            using var target = new MultipleSegmentBufferWriter(ARRAY_LENGTH, ArrayPool<byte>.Shared);
            WriteBuffer(target);
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_Configurable()
        {
            using var target = new MultipleSegmentBufferWriter(ARRAY_LENGTH, configurableArrayPool);
            WriteBuffer(target);
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_Scalable_SpinLock()
        {
            using var target = new MultipleSegmentBufferWriter(scalableSpinLock);
            WriteBuffer(target);
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_Scalable_Stack()
        {
            using var target = new MultipleSegmentBufferWriter(scalableStack);
            WriteBuffer(target);
        }


        [Benchmark]
        public void MultipleSegmentBufferWriter_FixedSize_SpinLock()
        {
            using var target = new MultipleSegmentBufferWriter(fixedSizeSpinLock);
            WriteBuffer(target);
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_FixedSize_Stack()
        {
            using var target = new MultipleSegmentBufferWriter(fixedSizeStack);
            WriteBuffer(target);
        }

        [Benchmark]
        public void Microsoft_RecyclableMemoryStream()
        {
            using var target = manager.GetStream();
            WriteBuffer(target);
        }

        [Benchmark]
        public void DotNext_PoolingArrayBufferWriter()
        {
            using var target = new PoolingArrayBufferWriter<byte>();
            WriteBuffer(target);
        }

        [Benchmark]
        public void DotNext_SparseBufferWriter()
        {
            using var target = new SparseBufferWriter<byte>(ARRAY_LENGTH);
            WriteBuffer(target);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteBuffer(IBufferWriter<byte> bufferWriter)
        {
            bufferWriter.Write(this.buffer);
        }
    }
}