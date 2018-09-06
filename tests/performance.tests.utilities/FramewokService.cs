namespace DevExperience.Performance.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DevExperience.Performance.Tests.Utilities.Links;
    using DevExperience.Performance.Tests.Utilities.Strategies;

    public class FramewokService : IFramewokService
    {
        private readonly IReadOnlyCollection<IFrameworkStrategy> frameworkStrategies;
        private readonly BaseStrategyLink benchmarkConfigPerFrameworkChain;

        public FramewokService(IReadOnlyCollection<IFrameworkStrategy> strategies, BaseStrategyLink benchmarkConfigPerFrameworkChain)
        {
            this.benchmarkConfigPerFrameworkChain = benchmarkConfigPerFrameworkChain;
            this.frameworkStrategies = strategies;
        }

        public FramewokService() 
            : this(new List<IFrameworkStrategy> {new NetFrameworkStrategy(), new MonoFrameworkStrategy()}.AsReadOnly(), 
                BenchmarkConfigPerFrameworkChain())
        {
        }

        public SupportedFrameworks ResolveExecutingFrameworkName()
        {
            var executedFramework = this.benchmarkConfigPerFrameworkChain.Execute(this.frameworkStrategies);
            if (executedFramework != SupportedFrameworks.Unknown)
            {
                return executedFramework;
            }

            var strategiesResult = this.frameworkStrategies.FirstOrDefault(fs => !string.IsNullOrWhiteSpace(fs.GetFrameworkName()));
            if (strategiesResult != null)
            {
                throw new NotSupportedException($"The Framework '{strategiesResult.GetFrameworkName()}' is not supported. Please specify a link in the chain to resolve its becnhmark configuration.");
            }

            var strategies = string.Join(Environment.NewLine, this.frameworkStrategies.Select(s => s.GetType()));
            throw new ArgumentNullException($"No strategy found for the target framework. Available Strategies:{Environment.NewLine}{strategies}", "targetFrameworkName");
        }

        private static BaseStrategyLink BenchmarkConfigPerFrameworkChain()
        {
            var chain = new MonoConfigLink();
            var coreLink = new CoreConfigLink();
            var clrLink = new ClrConfigLink();

            chain.SetSuccessor(coreLink);
            coreLink.SetSuccessor(clrLink);

            return chain;
        }
    }

    public interface IFramewokService
    {
        SupportedFrameworks ResolveExecutingFrameworkName();
    }
}