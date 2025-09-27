using BenchmarkDotNet.Running;

namespace RecyclableBuffer.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var benchmark = new WriteBufferBenchmark();
            benchmark.Setup();
            benchmark.MultipleSegmentBufferWriter_Shared();
            benchmark.MultipleSegmentBufferWriter_Shared();
#endif
            BenchmarkRunner.Run<WriteBufferParallelAsyncBenchmark>();
            BenchmarkRunner.Run<WriteBufferBenchmark>();
        }
    }
}
