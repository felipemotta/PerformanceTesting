namespace DevExperience.Assembly.Performance.Tests.Configs
{
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Jobs;
    using DevExperience.Assembly.Performance.Tests.Utilities;

    public class ClrConfig : BaseConfig
    {
        public ClrConfig(int launchCount, int iterationCount) : base() 
            => this.Add(Job.Clr
                .With(Platform.AnyCpu)
                .WithLaunchCount(launchCount)
                .WithIterationCount(iterationCount)
                .WithId(SupportedFrameworks.Clr.ToString()));

        public ClrConfig() : this(launchCount: 1, iterationCount: 5) { }
    }
}