namespace DevExperience.Assembly.Performance.Tests
{
    using BenchmarkDotNet.Attributes;
    using DevExperience.Performance.Tests.Utilities;

    public class ExistingBenchmark : IPerformanceTest
    {
        private byte[] array;
        private Existing existing;

        public ExistingBenchmark() => this.Arrange();

        public void Arrange()
        {
            this.array = new byte[200 * 1024 * 1024];
            this.existing = new Existing();
        }

        [Benchmark]
        public void Act()
        {
            this.existing.DoSomething(this.array);
        }
    }
}