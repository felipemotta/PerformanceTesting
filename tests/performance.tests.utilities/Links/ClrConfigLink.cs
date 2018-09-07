namespace DevExperience.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DevExperience.Performance.Tests.Utilities.Strategies;

    public class ClrConfigLink : BaseStrategyLink
    {
        private const string Netframework = ".NETFramework";

        public override SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies, Assembly callingAssembly)
        {
            if (strategies.Any(f => f.GetFrameworkName(callingAssembly).StartsWith(Netframework)))
            {
                return SupportedFrameworks.Clr;
            }

            return base.Execute(strategies, callingAssembly);
        }
    }
}