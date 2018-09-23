namespace DevExperience.Assembly.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DevExperience.Assembly.Performance.Tests.Utilities.Strategies;

    public class CoreConfigLink : BaseStrategyLink
    {
        private const string Netcoreapp = ".NETCoreApp";

        public override SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies, Assembly callingAssembly)
        {
            if (strategies.Any(f => f.GetFrameworkName(callingAssembly).StartsWith(Netcoreapp)))
            {
                return SupportedFrameworks.Core;
            }

            return base.Execute(strategies, callingAssembly);
        }
    }
}