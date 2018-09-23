namespace DevExperience.Assembly.Performance.Tests
{
    using BenchmarkDotNet.Attributes;
    using DevExperience.Performance.Tests.Utilities;

    public class ExistingBenchmarkTest : IPerformanceTest
    {
        private MyArray array;
        private Existing existing;

        public ExistingBenchmarkTest() => this.Arrange();

        public void Arrange()
        {
            this.array = new MyArray(new byte[200 * 1024 * 1024]);
            this.existing = new Existing();
        }

        [Benchmark]
        public void Act()
        {
            this.existing.DoSomething(this.array);
        }
    }
}