namespace DevExperience.Assembly.Performance.Tests
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using DevExperience.Assembly.Loader;
    using DevExperience.Assembly.Performance.Tests.Configs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CodeOnEvaluationTest
    {
        [TestMethod]
        public void PerformanceTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(StreamWithoutDomains), new ConfigFactory().Create());
        }
    }

    public class StreamWithoutDomains
    {
        [Benchmark]
        public byte[] BenchOk() => new byte[0];

        [Benchmark]
        public byte[] Bench200Mb() => new byte[200 * 1024 * 1024];

        [Benchmark]
        public void AppDomain200Mb()
        {
            using (var isolated = new Isolated<Facade>())
            {
                isolated.Value.DoSomething(new byte[200 * 1024 * 1024]);
            }
        }
    }
}
