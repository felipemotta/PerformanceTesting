namespace DevExperience.Performance.Tests.Utilities.Links
{
    using System.Collections.Generic;
    using DevExperience.Performance.Tests.Utilities.Strategies;

    public abstract class BaseStrategyLink
    {
        private BaseStrategyLink nextBaseStrategyLink;

        public void SetSuccessor(BaseStrategyLink next) => this.nextBaseStrategyLink = next;

        public virtual SupportedFrameworks Execute(IReadOnlyCollection<IFrameworkStrategy> strategies)
        {
            if (this.nextBaseStrategyLink != null)
            {
                return this.nextBaseStrategyLink.Execute(strategies);
            }

            return SupportedFrameworks.Unknown;
        }
    }
}