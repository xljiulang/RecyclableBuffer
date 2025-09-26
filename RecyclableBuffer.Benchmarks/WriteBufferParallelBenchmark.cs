using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using Microsoft.IO;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace RecyclableBuffer.Benchmarks
{

    [MemoryDiagnoser]
    public class WriteBufferParallelBenchmark
    {
        private static readonly RecyclableMemoryStreamManager manager = new();

        public const int COUNT = 1_0000;

        [Params(1024, 8 * 1024, 512 * 1024)]
        public int BufferSize = 1024;

        private byte[] buffer = [];

        private static readonly ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount / 2
        };

        [GlobalSetup]
        public void Setup()
        {
            this.buffer = new byte[this.BufferSize];
        }

        [Benchmark]
        public void SingleSegmentBufferWriter()
        {
            const int DefaultInitialCapacity = 128 * 1024;
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new SingleSegmentBufferWriter(DefaultInitialCapacity);
                WriteBuffer(target);
            });
        }

        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter()
        {
            Parallel.For(0, COUNT, options, _ =>
            {
                using var target = new MultipleSegmentBufferWriter();
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
    }
}