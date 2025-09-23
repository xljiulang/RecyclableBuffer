using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using Microsoft.IO;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class GetSpanBenchmark
    {
        private static readonly RecyclableMemoryStreamManager manager = new();

        [Params(0, 8 * 1024, 128 * 1024 + 1)]
        public int SizeHint;

        [Benchmark]
        public void SingleSegmentBufferWriter()
        {
            const int DefaultInitialCapacity = 128 * 1024;
            using var target = new SingleSegmentBufferWriter(DefaultInitialCapacity);
            GetSpan(target);
        }

        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter()
        {
            using var target = new MultipleSegmentBufferWriter();
            GetSpan(target);
        }

        [Benchmark]
        public void Microsoft_RecyclableMemoryStream()
        {
            using var target = manager.GetStream();
            GetSpan(target);
        }

        [Benchmark]
        public void DotNext_PoolingArrayBufferWriter()
        {
            using var target = new PoolingArrayBufferWriter<byte>();
            GetSpan(target);
        }

        [Benchmark]
        public void DotNext_SparseBufferWriter()
        {
            using var target = new SparseBufferWriter<byte>();
            GetSpan(target);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]  
        private void GetSpan(IBufferWriter<byte> bufferWriter)
        {
            bufferWriter.GetSpan(this.SizeHint);
        }
    }
}
