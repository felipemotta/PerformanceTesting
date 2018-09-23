namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using System.Threading;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using DevExperience.Assembly.Performance.Tests.Configs;
    using DevExperience.Performance.Tests.Utilities;
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

        [TestMethod]
        public void AccessingToDomainMemoryTest()
        {
            // Arrange
            var isolatedBenchmark = new IsolatedBenchmarkTest();

            // Act
            isolatedBenchmark.Act();

            // Assert
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
            // Arrange
            var testcase = new IsolatedBenchmarkTest();
            testcase.Arrange();

            var memoryWatcher = new MemoryWatcher();

            // Act
            memoryWatcher.Start(TimeSpan.FromMilliseconds(10));
            testcase.Act();
            memoryWatcher.Stop();
           
            // Assert
            Console.WriteLine(string.Join("\r\n", memoryWatcher.GetMeasuredTime()));
        }

        [TestMethod]
        public void IsolatedBenchmarkTest()
        {
            // Arrange
            var testCaseType = typeof(IsolatedBenchmarkTest);
            var benchmarkConfig = this.configFactory.Create();
            
            // Act
            Summary summary = BenchmarkRunner.Run(testCaseType, benchmarkConfig);

            // Assert
            summary.Should().NotBeNull();
        }

        [TestMethod]
        public void ExistingBenchmarkTest()
        {
            // Arrange
            var testCaseType = typeof(ExistingBenchmarkTest);
            var benchmarkConfig = this.configFactory.Create();
            
            // Act
            Summary summary = BenchmarkRunner.Run(testCaseType, benchmarkConfig);

            // Assert
            summary.Should().NotBeNull();
        }

        [TestMethod]
        public void ExistingWithEmpyDataBenchmarkTest()
        {
            // Arrange
            var testCaseType = typeof(ExistingWithEmpyDataBenchmarkTest);
            var benchmarkConfig = this.configFactory.Create();

            // Act
            Summary summary = BenchmarkRunner.Run(testCaseType, benchmarkConfig);

            // Assert
            summary.Should().NotBeNull();
        }
    }
}
