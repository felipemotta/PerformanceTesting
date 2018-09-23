namespace DevExperience.Assembly.Performance.Tests.Utilities.Strategies
{
    using System.Reflection;

    public interface IFrameworkStrategy
    {
        string GetFrameworkName(Assembly callingAssembly);
    }
}