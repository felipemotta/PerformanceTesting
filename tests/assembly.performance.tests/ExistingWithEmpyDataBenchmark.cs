namespace DevExperience.Assembly.Performance.Tests
{
    using BenchmarkDotNet.Attributes;
    using DevExperience.Performance.Tests.Utilities;

    public class ExistingWithEmpyDataBenchmarkTest : IPerformanceTest
    {
        private MyArray array;
        private Existing existing;

        public ExistingWithEmpyDataBenchmarkTest() => this.Arrange();

        public void Arrange()
        {
            this.array = new MyArray(new byte[0]);
            this.existing = new Existing();
        }

        [Benchmark]
        public void Act()
        {
            this.existing.DoSomething(this.array);
        }
    }
}