using BenchmarkDotNet.Running;

namespace RecyclableBuffer.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var benchmark = new BuffersBenchmark();
            benchmark.Setup();
            benchmark.MultipleSegmentBufferWriter_10();
#endif
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
