namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using System.Security.Cryptography;
    using BenchmarkDotNet.Attributes;
    using DevExperience.Assembly.Performance.Tests.Utilities;

    public class MicroBenchmarkExampleTest : IPerformanceTest
    {
        private const int N = 10000;
        private readonly byte[] data;

        private readonly MD5 md5 = MD5.Create();
        private readonly SHA256 sha256 = SHA256.Create();

        public MicroBenchmarkExampleTest()
        {
            this.data = new byte[N];

            new Random(42).NextBytes(this.data);

            this.md5 = MD5.Create();
            this.sha256 = SHA256.Create();
        }

        [Benchmark(Baseline = true, Description = "Md5")]
        public void Act() => this.md5.ComputeHash(this.data);

        [Benchmark(Description = "Sha256")]
        public void Sha256Act() => this.sha256.ComputeHash(this.data);

        public void Arrange()
        {
            throw new NotImplementedException();
        }
    }
}