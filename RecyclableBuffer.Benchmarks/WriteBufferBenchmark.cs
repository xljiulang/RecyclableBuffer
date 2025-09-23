using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using Microsoft.IO;
using System.Buffers;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class WriteBufferBenchmark
    {
        private static readonly RecyclableMemoryStreamManager manager = new();

        [Params(1024, 8 * 1024, 128 * 1024 + 1)]
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
            const int DefaultInitialCapacity = 128 * 1024;
            using var bufferWriter = new SingleSegmentBufferWriter(DefaultInitialCapacity);
            bufferWriter.Write(this.buffer);
        }

        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter()
        {
            using var bufferWriter = new MultipleSegmentBufferWriter();
            bufferWriter.Write(this.buffer);
        }

        [Benchmark]
        public void Microsoft_RecyclableMemoryStream()
        {
            using var bufferWriter = manager.GetStream();
            bufferWriter.Write(this.buffer);
        }

        [Benchmark]
        public void DotNext_PoolingArrayBufferWriter()
        {
            using var bufferWriter = new PoolingArrayBufferWriter<byte>();
            bufferWriter.Write(this.buffer);
        }

        [Benchmark]
        public void DotNext_SparseBufferWriter()
        {
            using var bufferWriter = new SparseBufferWriter<byte>();
            bufferWriter.Write(this.buffer);
        }
    }
}
