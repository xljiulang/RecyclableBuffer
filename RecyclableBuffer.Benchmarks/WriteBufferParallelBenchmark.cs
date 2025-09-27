using System.Buffers;

namespace RecyclableBuffer.Benchmarks
{
    public class WriteBufferParallelBenchmark : WriteBufferParallelAsyncBenchmark
    {
        protected override ValueTask SendToAsync(ReadOnlySequence<byte> sequence, CancellationToken cancellationToken)
        {
            return ValueTask.CompletedTask;
        }
    }
}