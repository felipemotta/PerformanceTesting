namespace DevExperience.Assembly
{
    using System;

    public class Facade : MarshalByRefObject
    {
        private readonly Existing existing = new Existing();

        public int DoSomething(MyArray array)
        {
            return this.existing.DoSomething(array);
        }
    }

    public class MyArray : MarshalByRefObject
    {
        public MyArray(byte[] newba)
        {
            this.ba = newba;
        }
        public byte[] ba { get;  }
    }
}