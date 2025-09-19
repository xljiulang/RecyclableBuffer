using BenchmarkDotNet.Attributes;
using Microsoft.IO;
using System.Buffers;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class BuffersBenchmark
    {
        private static readonly BufferPool pool = BufferPool.Shared;
        private static readonly RecyclableMemoryStreamManager manager = new();

        [Params(1024, 2048, 4096)]
        public int BufferLength = 1024;

        private byte[] buffer = [];

        [GlobalSetup]
        public void Setup()
        {
            this.buffer = new byte[this.BufferLength];
        }

        [Benchmark(Baseline = true)]
        public void RecyclableBuffer_10()
        {
            using var bufferWriter = new RecyclableBufferWriter(pool);
            for (var i = 0; i < 10; i++)
            {
                bufferWriter.Write(this.buffer);
            }
        }

        [Benchmark]
        public void RecyclableMemoryStream_10()
        {
            using var bufferWriter = manager.GetStream();
            for (var i = 0; i < 10; i++)
            {
                bufferWriter.Write(this.buffer);
            }
        }
    }
}
