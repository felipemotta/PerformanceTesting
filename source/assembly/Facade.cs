namespace DevExperience.Assembly
{
    using System;

    public class Facade : MarshalByRefObject
    {
        private readonly Existing existing = new Existing();

        public int DoSomething(MyArray array) => this.existing.DoSomething(array);
    }
}