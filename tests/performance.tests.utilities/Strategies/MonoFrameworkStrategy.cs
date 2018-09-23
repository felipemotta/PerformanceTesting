namespace DevExperience.Assembly.Performance.Tests.Utilities.Strategies
{
    using System;
    using System.Reflection;

    public class MonoFrameworkStrategy : IFrameworkStrategy
    {
        public string GetFrameworkName(Assembly callingAssembly)
        {
            return Type.GetType("Mono.Runtime") != null ? "Mono.Runtime" : string.Empty;
        }
    }
}