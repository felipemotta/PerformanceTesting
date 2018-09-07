namespace DevExperience.Assembly
{
    using System;

    public class Facade : MarshalByRefObject
    {
        private readonly Existing existing = new Existing();

        public int DoSomething(byte[] array)
        {
            return this.existing.DoSomething(array);
        }
    }
}