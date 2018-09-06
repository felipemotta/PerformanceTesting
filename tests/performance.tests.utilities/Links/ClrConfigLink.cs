namespace DevExperience.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using DevExperience.Performance.Tests.Utilities.Strategies;

    public class ClrConfigLink : BaseStrategyLink
    {
        private const string Netframework = ".NETFramework";

        public override SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies) 
            => strategies.Any(f => f.GetFrameworkName().StartsWith(Netframework)) ? 
                SupportedFrameworks.Clr : 
                base.Execute(strategies);
    }
}