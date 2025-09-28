namespace RecyclableBuffer.Benchmarks
{
    public class WriteBufferParallelBenchmark : WriteBufferParallelAsyncBenchmark
    {
        protected override ValueTask SendToAsync(Stream readableStream, CancellationToken cancellationToken)
        {
            return ValueTask.CompletedTask;
        }
    }
}