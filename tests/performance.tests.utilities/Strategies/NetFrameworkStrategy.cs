namespace DevExperience.Performance.Tests.Utilities.Strategies
{
    using System.Reflection;
    using System.Runtime.Versioning;

    public class NetFrameworkStrategy : IFrameworkStrategy
    {
        public string GetFrameworkName() => this.GetType().Assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? string.Empty;
    }
}