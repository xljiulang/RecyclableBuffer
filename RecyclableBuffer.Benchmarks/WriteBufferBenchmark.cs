using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using Microsoft.IO;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class WriteBufferBenchmark
    {
        private static readonly RecyclableMemoryStreamManager manager = new();

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
            const int DefaultInitialCapacity = 128 * 1024;
            using var target = new SingleSegmentBufferWriter(DefaultInitialCapacity);
            WriteBuffer(target);
        }

        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter_Shared()
        {
            using var target = new MultipleSegmentBufferWriter();
            WriteBuffer(target);
        }

        [Benchmark]
        public void MultipleSegmentBufferWriter_Default()
        {
            using var target = new MultipleSegmentBufferWriter(ByteArrayPool.Default);
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
            using var target = new SparseBufferWriter<byte>();
            WriteBuffer(target);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteBuffer(IBufferWriter<byte> bufferWriter)
        {
            bufferWriter.Write(this.buffer);
        }
    }
}
