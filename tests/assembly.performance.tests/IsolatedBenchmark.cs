namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using BenchmarkDotNet.Attributes;
    using DevExperience.Assembly.Loader;
    using DevExperience.Assembly.Performance.Tests.Utilities;

    public class IsolatedBenchmarkTest : IPerformanceTest, IDisposable
    {
        private MyArray array;
        public Isolated<Facade> Isolated;

        public IsolatedBenchmarkTest() => this.Arrange();

        public void Arrange() => this.array = new MyArray(new byte[200 * 1024 * 1024]);

        [Benchmark]
        public void Act()
        {
            this.Isolated = new Isolated<Facade>();
            this.Isolated.DomainInstance.DoSomething(this.array);
        }

        public void Dispose() => this.Isolated?.Dispose();
    }
}