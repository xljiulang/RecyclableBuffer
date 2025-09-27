using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using Microsoft.IO;
using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace RecyclableBuffer.Benchmarks
{

    [MemoryDiagnoser]
    public class WriteBufferParallelBenchmark
    {
        const int ARRAY_LENGTH = 128 * 1024;
        private static readonly RecyclableMemoryStreamManager manager = new();
        private static readonly ByteArrayBucket fixedSizeByteArrayBucket = ByteArrayBucket.CreateFixedSize(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket stackFixedSizeByteArrayBucket = new StackFixedSizeByteArrayBucket(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket stackScalableByteArrayBucket = new StackScalableByteArrayBucket(ARRAY_LENGTH);

        private static readonly ArrayPool<byte> configurableArrayPool = ArrayPool<byte>.Create(ARRAY_LENGTH, 100);

        public const int COUNT = 1_0000;

        [Params(1024, 8 * 1024, 512 * 1024)]
        public int BufferSize = 1024;

        private byte[] buffer = [];

        private static readonly ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount - 1
        };

        [GlobalSetup]
        public void Setup()
        {
            this.buffer = new byte[this.BufferSize];
        }

        [Benchmark]
        public void SingleSegmentBufferWriter()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new SingleSegmentBufferWriter(ARRAY_LENGTH);
                WriteBuffer(target);
            });
        }


        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter_Shared()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new MultipleSegmentBufferWriter();
                WriteBuffer(target);
            });
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_Configurable()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new MultipleSegmentBufferWriter(ARRAY_LENGTH, configurableArrayPool);
                WriteBuffer(target);
            });
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_Scalable()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                WriteBuffer(target);
            });
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_StackScalable()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new MultipleSegmentBufferWriter(stackScalableByteArrayBucket);
                WriteBuffer(target);
            });
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_FixedSize()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new MultipleSegmentBufferWriter(fixedSizeByteArrayBucket);
                WriteBuffer(target);
            });
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_StackFixedSize()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new MultipleSegmentBufferWriter(stackFixedSizeByteArrayBucket);
                WriteBuffer(target);
            });
        }

        [Benchmark]
        public void Microsoft_RecyclableMemoryStream()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = manager.GetStream();
                WriteBuffer(target);
            });
        }


        [Benchmark]
        public void DotNext_PoolingArrayBufferWriter()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new PoolingArrayBufferWriter<byte>();
                WriteBuffer(target);
            });
        }

        [Benchmark]
        public void DotNext_SparseBufferWriter()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new SparseBufferWriter<byte>();
                WriteBuffer(target);
            });
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteBuffer(IBufferWriter<byte> bufferWriter)
        {
            bufferWriter.Write(this.buffer);
        }


        private sealed class StackFixedSizeByteArrayBucket : ByteArrayBucket
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

        private sealed class StackScalableByteArrayBucket : ByteArrayBucket
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
    }
}