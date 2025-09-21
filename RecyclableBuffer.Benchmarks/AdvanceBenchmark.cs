using BenchmarkDotNet.Attributes;
using Microsoft.IO;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class AdvanceBenchmark
    {
        private static readonly RecyclableMemoryStreamManager manager = new();

        [Params(1024, 8 * 1024, 128 * 1024 + 1)]
        public int AdvanceCount;

        [Benchmark(Baseline = true)]
        public void MultipleSegmentBufferWriter_Advance()
        {
            using var bufferWriter = new MultipleSegmentBufferWriter();
            var span = bufferWriter.GetSpan(this.AdvanceCount);
            bufferWriter.Advance(this.AdvanceCount);
        }

        [Benchmark]
        public void RecyclableMemoryStream_Advance()
        {
            using var bufferWriter = manager.GetStream();
            var span = bufferWriter.GetSpan(this.AdvanceCount);
            bufferWriter.Advance(this.AdvanceCount);
        }
    }
}
