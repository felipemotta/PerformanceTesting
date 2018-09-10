namespace DevExperience.Assembly.Performance.Tests
{
    using BenchmarkDotNet.Attributes;
    using DevExperience.Assembly.Loader;
    using DevExperience.Performance.Tests.Utilities;

    public class IsolatedBenchmark : IPerformanceTest
    {
        private MyArray array;
        public Isolated<Facade> Isolated;

        public IsolatedBenchmark() => this.Arrange();

        public void Arrange()
        {
            this.array = new  MyArray(new byte[200 * 1024 * 1024]);
        }

        [Benchmark]
        public void Act()
        {
            using (this.Isolated = new Isolated<Facade>())
            {
                this.Isolated.Value.DoSomething(this.array);
            }
        }
    }
}