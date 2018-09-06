namespace DevExperience.Assembly
{
    using System;

    public interface IIsolated<out T> : IDisposable
    {
        T Value { get; }
    }
}