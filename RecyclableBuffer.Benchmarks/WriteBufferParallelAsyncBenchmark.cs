using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using DotNext.IO;
using Microsoft.IO;
using RecyclableBuffer.Buckets;
using System.Buffers;

namespace RecyclableBuffer.Benchmarks
{

    [MemoryDiagnoser]
    public class WriteBufferParallelAsyncBenchmark
    {
        const int COUNT = 1_0000;
        const int ARRAY_LENGTH = 128 * 1024;
        private static readonly RecyclableMemoryStreamManager manager = new();
        private static readonly ByteArrayBucket fixedSizeSpinLock = new FixedSizeSpinLockByteArrayBucket(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket fixedSizeStack = new FixedSizeStackByteArrayBucket(ARRAY_LENGTH, 32);
        private static readonly ByteArrayBucket scalableSpinLock = new ScalableSpinLockByteArrayBucket(ARRAY_LENGTH);
        private static readonly ByteArrayBucket scalableStack = new ScalableStackByteArrayBucket(ARRAY_LENGTH);
        private static readonly ArrayPool<byte> configurableArrayPool = ArrayPool<byte>.Create(ARRAY_LENGTH, 100);
        private static readonly ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 };

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

                await SendToAsync(target.AsReadableStream(), ct);
            });
        }

        [Benchmark(Baseline = true)]
        public async Task MultipleSegmentBufferWriter_Shared()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(ARRAY_LENGTH, ArrayPool<byte>.Shared);
                target.Write(this._buffer);

                await SendToAsync(target.AsReadableStream(), ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_Configurable()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(ARRAY_LENGTH, configurableArrayPool);
                target.Write(this._buffer);

                await SendToAsync(target.AsReadableStream(), ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_Scalable_SpinLock()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(scalableSpinLock);
                target.Write(this._buffer);

                await SendToAsync(target.AsReadableStream(), ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_Scalable_Stack()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(scalableStack);
                target.Write(this._buffer);

                await SendToAsync(target.AsReadableStream(), ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_FixedSize_SpinLock()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(fixedSizeSpinLock);
                target.Write(this._buffer);

                await SendToAsync(target.AsReadableStream(), ct);
            });
        }

        [Benchmark]
        public async Task MultipleSegmentBufferWriter_FixedSize_Stack()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new MultipleSegmentBufferWriter(fixedSizeStack);
                target.Write(this._buffer);

                await SendToAsync(target.AsReadableStream(), ct);
            });
        }

        [Benchmark]
        public async Task Microsoft_RecyclableMemoryStream()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = manager.GetStream();
                target.Write(this._buffer);

                await SendToAsync(target, ct);
            });
        }


        [Benchmark]
        public async Task DotNext_PoolingArrayBufferWriter()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new PoolingArrayBufferWriter<byte>();
                target.Write(this._buffer);

                await SendToAsync(target.WrittenMemory.AsStream(), ct);
            });
        }

        [Benchmark]
        public async Task DotNext_SparseBufferWriter()
        {
            await Parallel.ForAsync(0, COUNT, parallelOptions, async (_, ct) =>
            {
                using var target = new SparseBufferWriter<byte>(ARRAY_LENGTH);
                target.Write(this._buffer);

                await SendToAsync(target.AsStream(readable: true), ct);
            });
        }

        protected virtual async ValueTask SendToAsync(Stream readableStream, CancellationToken cancellationToken)
        {
            // 这里不做任何实际操作，只模拟异步等待
            // 因为实际应用中，写入操作通常是异步的
            // 异步会导致缓冲区的租用和归还行为大概率产生在不同的线程上
            // 而不同的 ArrayPool 模型在这种场景下的表现差异更明显
            await Task.Yield();
        }
    }
}