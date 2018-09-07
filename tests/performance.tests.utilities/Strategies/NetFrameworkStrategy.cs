namespace DevExperience.Performance.Tests.Utilities.Strategies
{
    using System;
    using System.Reflection;
    using System.Runtime.Versioning;

    public class NetFrameworkStrategy : IFrameworkStrategy
    {
        public string GetFrameworkName(Assembly callingAssembly)
        {
            return callingAssembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? string.Empty;
        }
    }
}