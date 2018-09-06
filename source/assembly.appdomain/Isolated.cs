namespace DevExperience.Assembly.Loader
{
    using System;

    public sealed class Isolated<T> : IIsolated<T> where T : MarshalByRefObject
    {
        private AppDomain domain;

        public Isolated()
        {
            this.domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), null, AppDomain.CurrentDomain.SetupInformation);

            Type type = typeof(T);

            this.Value = (T)this.domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Value { get; }

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
