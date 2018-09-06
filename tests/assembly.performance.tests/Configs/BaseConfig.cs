namespace DevExperience.Assembly.Performance.Tests.Configs
{
    using System.Linq;
    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Exporters.Csv;
    using BenchmarkDotNet.Validators;

    public class BaseConfig : ManualConfig
    {
        public BaseConfig()
        {
            this.Add(JitOptimizationsValidator.DontFailOnError); // ALLOW NON-OPTIMIZED DLLS
            this.Add(DefaultConfig.Instance.GetLoggers().ToArray()); // manual config has no loggers by default
            this.Add(DefaultConfig.Instance.GetExporters().ToArray()); // manual config has no exporters by default
            this.Add(DefaultConfig.Instance.GetColumnProviders().ToArray()); // manual config has no columns by default
            this.Add(CsvExporter.Default);
            this.Add(MemoryDiagnoser.Default);
            this.Add(ExecutionValidator.DontFailOnError);
            this.Add(StatisticColumn.AllStatistics);
        }
    }
}