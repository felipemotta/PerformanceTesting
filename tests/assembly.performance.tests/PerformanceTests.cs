namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using DevExperience.Assembly.Performance.Tests.Configs;
    using DevExperience.Assembly.Performance.Tests.Utilities;
    using DevExperience.Assembly.Performance.Tests.Utilities.Roche.RMS.Calculations.Testing.Utilities.Performance.Extensions;
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
        public void _Info()
        {
            // Arrange
            var isolatedBenchmark = new IsolatedBenchmarkTest();

            // Act
            isolatedBenchmark.Act();

            // Assert
            // Memory I get from Benchmark
            Console.WriteLine($"AppDomain MonitoringTotalProcessorTime {AppDomain.CurrentDomain.MonitoringTotalProcessorTime}");
            Console.WriteLine($"AppDomain MonitoringTotalAllocatedMemorySize {AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize}");

            // Memory I get from the specific domain
            Console.WriteLine($"Isolated MonitoringTotalProcessorTime {isolatedBenchmark.Isolated.domain.MonitoringTotalProcessorTime}");
            Console.WriteLine($"Isolated MonitoringTotalAllocatedMemorySize {isolatedBenchmark.Isolated.domain.MonitoringTotalAllocatedMemorySize}");
            Console.WriteLine($"AppDomain MonitoringSurvivedProcessMemorySize {AppDomain.MonitoringSurvivedProcessMemorySize}");

            isolatedBenchmark.Dispose();
        }

        [TestMethod]
        public void Benchmarking_MemoryWatcher_IsolatedMemoryTest()
        {
            // Arrange
            var isolatedBenchmark = new IsolatedBenchmarkTest();
            isolatedBenchmark.Arrange();

            IMemoryWatcher memoryWatcher = new MemoryWatcher();

            // Act
            memoryWatcher.Start(TimeSpan.FromMilliseconds(10));
            isolatedBenchmark.Act();
            memoryWatcher.Stop();
           
            // Assert
            Console.WriteLine("List of measured memory");
            Console.WriteLine(string.Join(Environment.NewLine, memoryWatcher.GetMeasuredMemory()));
            Console.WriteLine($"MaxMemory = {memoryWatcher.GetMaxMemory()}");
            Console.WriteLine($"MeanMemory = {memoryWatcher.GetMeanMemory()}");
            isolatedBenchmark.Dispose();
        }

        [TestMethod]
        public void Benchmarking_BenchmarkRunner_IsolatedBenchmarkTest()
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
        public void Benchmarking_BenchmarkRunner_ExistingBenchmarkTest()
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
        public void Benchmarking_BenchmarkRunner_ExistingWithEmpyDataBenchmarkTest()
        {
            // Arrange
            var testCaseType = typeof(ExistingWithEmpyDataBenchmarkTest);
            var benchmarkConfig = this.configFactory.Create();

            // Act
            Summary summary = BenchmarkRunner.Run(testCaseType, benchmarkConfig);

            // Assert
            summary.Should().NotBeNull();
        }

        [TestMethod]
        public void MicroBenchmark_BenchmarkRunner_ExistingWithEmpyDataBenchmarkTest()
        {
            // Arrange
            var testCaseType = typeof(MicroBenchmarkExampleTest);
            var benchmarkConfig = this.configFactory.Create();

            // Act
            Summary summary = BenchmarkRunner.Run(testCaseType, benchmarkConfig);

            // Assert
            summary.Should().NotBeNull();
        }
    }
}
