namespace DevExperience.Assembly.Performance.Tests.Configs
{
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Jobs;
    using DevExperience.Assembly.Performance.Tests.Utilities;

    public class MonoConfig : BaseConfig
    {
        public MonoConfig(int launchCount, int iterationCount) : base()
            => this.Add(Job.Mono
                .With(Platform.AnyCpu)
                .WithLaunchCount(launchCount)
                .WithIterationCount(iterationCount)
                .WithId(SupportedFrameworks.Mono.ToString()));

        public MonoConfig() : this(launchCount: 1, iterationCount: 5) { }
    }
}