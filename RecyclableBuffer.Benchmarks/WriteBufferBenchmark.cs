using BenchmarkDotNet.Attributes;
using Microsoft.IO;
using System.Buffers;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class WriteBufferBenchmark
    {
        private static readonly RecyclableMemoryStreamManager manager = new();

        [Params(1024, 8 * 1024, 128 * 1024 + 1)]
        public int BufferSize;

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
        public void RecyclableMemoryStream()
        {
            using var bufferWriter = manager.GetStream();
            bufferWriter.Write(this.buffer);
        }
    }
}
