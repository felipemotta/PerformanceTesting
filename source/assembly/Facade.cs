namespace DevExperience.Assembly
{
    using System;

    public class Facade : MarshalByRefObject
    {
        private readonly Existing existing = new Existing();

        public void DoSomething(byte[] array) => this.existing.DoSomething(array);
    }
}