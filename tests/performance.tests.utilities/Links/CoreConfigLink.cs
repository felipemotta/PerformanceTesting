namespace DevExperience.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using DevExperience.Performance.Tests.Utilities.Strategies;

    public class CoreConfigLink : BaseStrategyLink
    {
        private const string Netcoreapp = ".NETCoreApp";

        public override SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies) 
            => strategies.Any(f => f.GetFrameworkName().StartsWith(Netcoreapp)) ? SupportedFrameworks.Core : base.Execute(strategies);
    }
}