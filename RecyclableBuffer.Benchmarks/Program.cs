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
            benchmark.MultipleSegmentBufferWriter();
#endif
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
