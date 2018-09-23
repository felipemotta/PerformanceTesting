namespace DevExperience.Assembly.Performance.Tests.Configs
{
    using System;
    using System.Reflection;
    using BenchmarkDotNet.Configs;
    using DevExperience.Assembly.Performance.Tests.Utilities;

    public class ConfigFactory
    {
        public IFramewokService FrameworkService { get; }

        public ConfigFactory() : this(new FramewokService(Assembly.GetExecutingAssembly())) {}

        public ConfigFactory(IFramewokService framewokService) => this.FrameworkService = framewokService;

        public IConfig Create()
        {
            var framework = this.FrameworkService.ResolveExecutingFrameworkName();

            switch (framework)
            {
                case SupportedFrameworks.Clr:
                    return new ClrConfig();
                case SupportedFrameworks.Core:
                    return new CoreConfig();
                case SupportedFrameworks.Mono:
                    return new MonoConfig();
                case SupportedFrameworks.Unknown:
                    return new BaseConfig();
                default:
                    throw new ArgumentOutOfRangeException($"The framework {framework.ToString()} identifier was not expected.");
            }
        }
    }
}