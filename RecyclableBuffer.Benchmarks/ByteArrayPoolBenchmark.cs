using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System.Buffers;
using System.Collections.Concurrent;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    [HardwareCounters(HardwareCounter.CacheMisses, HardwareCounter.BranchMispredictions)]
    public class ByteArrayPoolBenchmark
    {
        private const int COUNT = 1_0000;
        private const int BUFFER_SIZE = 1024;

        [Benchmark(Baseline = true)]
        public void ArrayPool_Shared_Parallel()
        {
            Parallel.For(0, COUNT, _ =>
            {
                var array = ArrayPool<byte>.Shared.Rent(BUFFER_SIZE);
                ArrayPool<byte>.Shared.Return(array);
            });
        }

        [Benchmark]
        public void ByteArrayPool_Default_Parallel()
        {
            Parallel.For(0, COUNT, _ =>
            {
                var array = ByteArrayPool.Default.Rent(BUFFER_SIZE);
                ByteArrayPool.Default.Return(array);
            });
        }

        [Benchmark]
        public void QueueArrayPool_Default_Parallel()
        {
            Parallel.For(0, COUNT, _ =>
            {
                var array = QueueArrayPool.Default.Rent(BUFFER_SIZE);
                QueueArrayPool.Default.Return(array);
            });
        }

        private sealed class QueueArrayPool : ArrayPool<byte>
        {
            private readonly int _arrayLength;
            private readonly ConcurrentQueue<byte[]> _quque = [];

            public static QueueArrayPool Default { get; } = new QueueArrayPool(128 * 1024);

            public QueueArrayPool(int arrayLength)
            {
                this._arrayLength = arrayLength;
            }

            public override byte[] Rent(int minimumLength)
            {
                if (minimumLength > _arrayLength)
                {
                    return Shared.Rent(minimumLength);
                }

                if (_quque.TryDequeue(out var array))
                {
                    return array;
                }
                return new byte[_arrayLength];
            }

            public override void Return(byte[] array, bool clearArray = false)
            {
                if (array.Length <= _arrayLength)
                {
                    return;
                }

                if (array.Length > _arrayLength)
                {
                    Shared.Return(array, clearArray);
                    return;
                }

                if (clearArray)
                {
                    array.AsSpan().Clear();
                }
                _quque.Enqueue(array);
            }
        }
    }
}
