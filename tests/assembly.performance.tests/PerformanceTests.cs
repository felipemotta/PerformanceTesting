namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using System.Linq;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using DevExperience.Assembly.Loader;
    using DevExperience.Assembly.Performance.Tests.Configs;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PerformanceTests
    {
        private ConfigFactory configFactory;
        
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.MonitoringIsEnabled = true;
            this.configFactory = new ConfigFactory();
        }

        //[TestMethod]
        //public void AccessingToDomainMemoryTest()
        //{
        //    AppDomain.MonitoringIsEnabled = true;
        //    var isolatedBenchmark = new IsolatedBenchmark();
        //    isolatedBenchmark.Act();

        //    // Memory I get from Benchmark
        //    Console.WriteLine("AppDomain MonitoringTotalProcessorTime {0}", AppDomain.CurrentDomain.MonitoringTotalProcessorTime);
        //    Console.WriteLine("AppDomain MonitoringTotalAllocatedMemorySize {0}", AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize);

        //    // Memory I get from the specific domain
        //    Console.WriteLine("isolated MonitoringTotalProcessorTime {0}", isolatedBenchmark.Isolated.domain.MonitoringTotalProcessorTime);
        //    Console.WriteLine("isolated MonitoringTotalAllocatedMemorySize {0}", isolatedBenchmark.Isolated.domain.MonitoringTotalAllocatedMemorySize);
        //    Console.WriteLine("AppDomain MonitoringSurvivedProcessMemorySize {0}", AppDomain.MonitoringSurvivedProcessMemorySize);


        //}

        [TestMethod]
        public void IsolatedBenchmarkTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(IsolatedBenchmark), this.configFactory.Create());

            summary.Reports.First().GcStats.GetTotalAllocatedBytes(true).Should().BeLessOrEqualTo(200*1024*1024); //2MB LOGIC + 2MB COPY ARRAY (but not displayed because it is in an appdomain)
        }

        [TestMethod]
        public void ExistingBenchmarkTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(ExistingBenchmark), this.configFactory.Create());

            summary.Reports.First().GcStats.GetTotalAllocatedBytes(true).Should().BeLessOrEqualTo(400 * 1024 * 1024); //2MB LOGIC + 2MB COPY ARRAY

        }

        [TestMethod]
        public void ExistingWithEmpyDataBenchmarkTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(ExistingWithEmpyDataBenchmark), this.configFactory.Create());

            summary.Reports.First().GcStats.GetTotalAllocatedBytes(true).Should().BeCloseTo(0, 2); //25KB
        }
    }
}
