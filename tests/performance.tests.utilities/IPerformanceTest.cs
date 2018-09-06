namespace DevExperience.Performance.Tests.Utilities
{
    public interface IPerformanceTestBase
    {
        void Arrange();
    }

    public interface IPerformanceTest : IPerformanceTestBase
    {
        void Act();
    }

    public interface IPerformanceTest<out T> : IPerformanceTestBase
    {
        T Act();
    }
}