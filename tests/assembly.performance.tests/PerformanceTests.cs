namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using System.Threading;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using DevExperience.Assembly.Performance.Tests.Configs;
    using DevExperience.Performance.Tests.Utilities;
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

        [TestMethod]
        public void AccessingToDomainMemoryTest()
        {
            AppDomain.MonitoringIsEnabled = true;
            var isolatedBenchmark = new IsolatedBenchmark();
            isolatedBenchmark.Act();

            // Memory I get from Benchmark
            Console.WriteLine("AppDomain MonitoringTotalProcessorTime {0}", AppDomain.CurrentDomain.MonitoringTotalProcessorTime);
            Console.WriteLine("AppDomain MonitoringTotalAllocatedMemorySize {0}", AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize);

            // Memory I get from the specific domain
            Console.WriteLine("isolated MonitoringTotalProcessorTime {0}", isolatedBenchmark.Isolated.domain.MonitoringTotalProcessorTime);
            Console.WriteLine("isolated MonitoringTotalAllocatedMemorySize {0}", isolatedBenchmark.Isolated.domain.MonitoringTotalAllocatedMemorySize);
            Console.WriteLine("AppDomain MonitoringSurvivedProcessMemorySize {0}", AppDomain.MonitoringSurvivedProcessMemorySize);
        }

        [TestMethod]
        public void IsolatedMemoryTest()
        {
            var testcase = new IsolatedBenchmark();
            testcase.Arrange();

            var memoryWatcher = new MemoryWatcher();
            memoryWatcher.Start(TimeSpan.FromMilliseconds(10));

            testcase.Act();

            //var arr = new byte [200 * 1024 * 1024];

            //Console.WriteLine($"Arr length: {arr.Length}");

            memoryWatcher.Stop();
            
            Thread.Sleep(1000);

            Console.WriteLine(string.Join("\r\n", memoryWatcher.GetMeasuredTime()));
        }

        [TestMethod]
        public void IsolatedBenchmarkTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(IsolatedBenchmark), this.configFactory.Create());
        }

        [TestMethod]
        public void ExistingBenchmarkTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(ExistingBenchmark), this.configFactory.Create());
        }

        [TestMethod]
        public void ExistingWithEmpyDataBenchmarkTest()
        {
            Summary summary = BenchmarkRunner.Run(typeof(ExistingWithEmpyDataBenchmark), this.configFactory.Create());
        }
    }
}
