namespace DevExperience.Assembly
{
    using System;

    public class Existing
    {
        private byte[] otherArray;

        public int DoSomething(byte[] array)
        {
            this.otherArray = (byte[])array.Clone();
            return array.Length;
        }
    }
}
