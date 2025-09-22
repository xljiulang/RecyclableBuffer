using BenchmarkDotNet.Attributes;
using Microsoft.IO;

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
            using var bufferWriter = new SingleSegmentBufferWriter(DefaultInitialCapacity);
            var span = bufferWriter.GetSpan(this.SizeHint);
        }

        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter()
        {
            using var bufferWriter = new MultipleSegmentBufferWriter();
            var span = bufferWriter.GetSpan(this.SizeHint);
        }

        [Benchmark]
        public void RecyclableMemoryStream()
        {
            using var bufferWriter = manager.GetStream();
            var span = bufferWriter.GetSpan(this.SizeHint);
        }
    }
}
