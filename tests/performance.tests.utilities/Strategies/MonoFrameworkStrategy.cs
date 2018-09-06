namespace DevExperience.Performance.Tests.Utilities.Strategies
{
    using System;

    public class MonoFrameworkStrategy : IFrameworkStrategy
    {
        public string GetFrameworkName() => Type.GetType("Mono.Runtime") != null ? "Mono.Runtime" : string.Empty;
    }
}