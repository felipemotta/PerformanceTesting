namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using DevExperience.Assembly.Loader;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CodeOnEvaluationTest
    {
        public class Config : ManualConfig
        {
            public Config()
            {
                this.Add(Job.Clr.WithLaunchCount(1).WithIterationCount(5));
            }
        }

        [TestMethod]
        public void PerformanceTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(StreamWithoutDomains), new Config());
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
