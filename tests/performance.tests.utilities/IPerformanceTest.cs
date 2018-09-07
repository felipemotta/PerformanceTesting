namespace DevExperience.Performance.Tests.Utilities
{
    using BenchmarkDotNet.Attributes;

    public interface IPerformanceTestBase
    {
        void Arrange();
    }

    public interface IPerformanceTest : IPerformanceTestBase
    {
        [Benchmark]
        void Act();
    }

    public interface IPerformanceTest<out T> : IPerformanceTestBase
    {
        [Benchmark]
        T Act();
    }
}