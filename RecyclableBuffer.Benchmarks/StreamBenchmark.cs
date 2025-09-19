using BenchmarkDotNet.Attributes;
using Microsoft.IO;
using System.Text.Json;

namespace RecyclableBuffer.Benchmarks
{
    [MemoryDiagnoser]
    public class StreamBenchmark
    {
        private User[] users = [];
        private static readonly RecyclableMemoryStreamManager manager = new();

        [Params(10, 100, 1000)]
        public int UserLength;


        [GlobalSetup]
        public void Setup()
        {
            this.users = Enumerable.Range(1, UserLength).Select(i => new User { Id = i }).ToArray();
        }

        [Benchmark(Baseline = true)]
        public async Task RecyclableBuffer()
        {
            using var bufferWriter = new RecyclableBufferWriter();
            var stream = bufferWriter.AsStream();
            await JsonSerializer.SerializeAsync(stream, this.users);
        }

        [Benchmark]
        public async Task RecyclableMemoryStream()
        {
            using var stream = manager.GetStream();
            await JsonSerializer.SerializeAsync(stream, this.users);
        }


        public class User
        {
            public int Id { get; set; }
            public string? Name { get; set; } = "RecyclableBuffer";

            public int Age { get; set; } = 18;

            public string? Address { get; set; } = "Some Address";

            public string? Email { get; set; } = "Some Email";

            public string? Phone { get; set; } = "10086";

            public DateTimeOffset? BirthDate { get; set; } = DateTimeOffset.UtcNow;
        }
    }
}
