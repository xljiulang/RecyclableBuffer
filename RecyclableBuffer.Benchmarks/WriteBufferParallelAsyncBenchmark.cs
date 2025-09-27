using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using Microsoft.IO;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace RecyclableBuffer.Benchmarks
{

    [MemoryDiagnoser]
    public class WriteBufferParallelAsyncBenchmark
    {
        const int COUNT = 1_0000;
        const int ARRAY_LENGTH = 128 * 1024;
        private static readonly RecyclableMemoryStreamManager manager = new();
        private static readonly ByteArrayBucket spinLockFixedSizeByteArrayBucket = ByteArrayBucket.CreateFixedSize(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket spinLockStackFixedSizeByteArrayBucket = new StackFixedSizeByteArrayBucket(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket stackScalableByteArrayBucket = new StackScalableByteArrayBucket(ARRAY_LENGTH);
        private static readonly ArrayPool<byte> configurableArrayPool = ArrayPool<byte>.Create(ARRAY_LENGTH, 100);
        private static readonly ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 };
        private static readonly IPEndPoint remoteEndPoint = new(IPAddress.Loopback, 443);

        [Params(1024, 8 * 1024, 512 * 1024)]
        public int BufferSize = 1024;

        private byte[] _buffer = [];

        [GlobalSetup]
        public void Setup()
        {
            this._buffer = new byte[this.BufferSize];
        }

        [Benchmark]
        public async Task SingleSegmentBufferWriter()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new SingleSegmentBufferWriter(ARRAY_LENGTH);
                target.Write(this._buffer);

                await SendToAsync(target.WrittenSequence, ct);
            });
        }

        [Benchmark(Baseline = true)]
        public async Task MultipleSegmentBufferWriter_Shared()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(ARRAY_LENGTH);
                target.Write(this._buffer);

                await SendToAsync(target.WrittenSequence, ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_Configurable()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(ARRAY_LENGTH, configurableArrayPool);
                target.Write(this._buffer);

                await SendToAsync(target.WrittenSequence, ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_Scalable_SpinLock()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(ByteArrayBucket.DefaultScalable);
                target.Write(this._buffer);

                await SendToAsync(target.WrittenSequence, ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_Scalable_Stack()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(stackScalableByteArrayBucket);
                target.Write(this._buffer);

                await SendToAsync(target.WrittenSequence, ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_FixedSize_SpinLock()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(spinLockFixedSizeByteArrayBucket);
                target.Write(this._buffer);

                await SendToAsync(target.WrittenSequence, ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_FixedSize_Stack()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(spinLockStackFixedSizeByteArrayBucket);
                target.Write(this._buffer);

                await SendToAsync(target.WrittenSequence, ct);
            });
        }

        [Benchmark]
        public async Task Microsoft_RecyclableMemoryStream()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = manager.GetStream();
                target.Write(this._buffer);

                await SendToAsync(target.GetReadOnlySequence(), ct);
            });
        }


        [Benchmark]
        public async Task DotNext_PoolingArrayBufferWriter()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new PoolingArrayBufferWriter<byte>();
                target.Write(this._buffer);

                await SendToAsync(new ReadOnlySequence<byte>(target.WrittenMemory), ct);
            });
        }

        [Benchmark]
        public async Task DotNext_SparseBufferWriter()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new SparseBufferWriter<byte>(ARRAY_LENGTH);
                target.Write(this._buffer);

                await SendToAsync(target.ToReadOnlySequence(), ct);
            });
        }

        private static async ValueTask SendToAsync(ReadOnlySequence<byte> sequence, CancellationToken cancellationToken)
        {
            const int CHUNK_SIZE = 8 * 1024;
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            while (sequence.Length > CHUNK_SIZE)
            {
                var data = sequence.Slice(0, CHUNK_SIZE);
                foreach (var memory in data)
                {
                    await socket.SendToAsync(memory, remoteEndPoint, cancellationToken);
                }
                sequence = sequence.Slice(data.End);
            }

            foreach (var memory in sequence)
            {
                await socket.SendToAsync(memory, remoteEndPoint, cancellationToken);
            }
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