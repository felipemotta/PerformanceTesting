namespace DevExperience.Assembly.Performance.Tests.Configs
{
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Toolchains.InProcess;
    using DevExperience.Assembly.Performance.Tests.Utilities;

    public class CoreConfig : BaseConfig
    {
        public CoreConfig(int launchCount, int iterationCount) : base()
            => this.Add(Job.Core
                .With(Platform.AnyCpu)
                .With(InProcessToolchain.Instance)
                .WithLaunchCount(launchCount)
                .WithIterationCount(iterationCount)
                .WithId(SupportedFrameworks.Core.ToString())
                .AsBaseline());

        public CoreConfig() : this(launchCount: 1, iterationCount: 5) { }

    }
}