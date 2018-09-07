namespace DevExperience.Assembly.Performance.Tests
{
    using BenchmarkDotNet.Attributes;
    using DevExperience.Performance.Tests.Utilities;

    public class ExistingWithEmpyDataBenchmark : IPerformanceTest
    {
        private byte[] array;
        private Existing existing;

        public ExistingWithEmpyDataBenchmark() => this.Arrange();

        public void Arrange()
        {
            this.array = new byte[0];
            this.existing = new Existing();
        }

        [Benchmark]
        public void Act()
        {
            this.existing.DoSomething(this.array);
        }
    }
}