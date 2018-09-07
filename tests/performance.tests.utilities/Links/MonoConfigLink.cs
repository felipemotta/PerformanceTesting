namespace DevExperience.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DevExperience.Performance.Tests.Utilities.Strategies;

    public class MonoConfigLink : BaseStrategyLink
    {
        private const string MonoRuntime = "Mono.Runtime";

        public override SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies, Assembly callingAssembly)
        {
            if (strategies.Any(f => f.GetFrameworkName(callingAssembly).StartsWith(MonoRuntime)))
            {
                return SupportedFrameworks.Mono;
            }

            return base.Execute(strategies, callingAssembly);
        }
    }
}