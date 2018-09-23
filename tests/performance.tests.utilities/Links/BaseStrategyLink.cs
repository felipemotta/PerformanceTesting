namespace DevExperience.Assembly.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using System.Reflection;
    using DevExperience.Assembly.Performance.Tests.Utilities.Strategies;

    public abstract class BaseStrategyLink
    {
        private BaseStrategyLink nextBaseStrategyLink;

        public void SetSuccessor(BaseStrategyLink next) => this.nextBaseStrategyLink = next;

        public virtual SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies, Assembly callingAssembly)
        {
            if (this.nextBaseStrategyLink != null)
            {
                return this.nextBaseStrategyLink.Execute(strategies, callingAssembly);
            }

            return SupportedFrameworks.Unknown;
        }
    }
}