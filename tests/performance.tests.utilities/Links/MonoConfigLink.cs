namespace DevExperience.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using DevExperience.Performance.Tests.Utilities.Strategies;

    public class MonoConfigLink : BaseStrategyLink
    {
        private const string MonoRuntime = "Mono.Runtime";

        public override SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies)
            => strategies.Any(f => f.GetFrameworkName().StartsWith(MonoRuntime)) ? 
                SupportedFrameworks.Mono : 
                base.Execute(strategies);
    }
}