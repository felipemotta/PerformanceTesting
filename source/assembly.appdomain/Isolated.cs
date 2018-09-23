namespace DevExperience.Assembly.Loader
{
    using System;

    public sealed class Isolated<T> : IIsolated<T> where T : MarshalByRefObject
    {
        public AppDomain domain;

        public Isolated()
        {
            this.domain = AppDomain.CreateDomain($"Isolated:{Guid.NewGuid()}", null, AppDomain.CurrentDomain.SetupInformation);

            var type = typeof(T);

            this.DomainInstance = (T) this.domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T DomainInstance { get; }

        public void Dispose()
        {
            if (this.domain != null)
            {
                AppDomain.Unload(this.domain);

                this.domain = null;
            }
        }
    }
}
