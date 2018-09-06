namespace DevExperience.Performance.Tests.Utilities
{
    using BenchmarkDotNet.Attributes;

    public abstract class RunnerBase<T> where T : IPerformanceTestBase, new()
    {
        protected T PerformanceTest;

        public void Initialize()
        {
            this.PerformanceTest = new T();
            this.PerformanceTest.Arrange();
        }
    }

    public class Runner<T> : RunnerBase<T> where T : IPerformanceTest, new()
    {
        public Runner() => this.Initialize();

        [Benchmark]
        public void Act() => this.PerformanceTest.Act();
    }

    public class Runner<T, TOut> : RunnerBase<T> where T : IPerformanceTest<TOut>, new()
    {
        public Runner() => this.Initialize();

        [Benchmark]
        public TOut Act() => this.PerformanceTest.Act();
    }
}