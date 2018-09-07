namespace DevExperience.Assembly
{
    using System;

    public class Existing
    {
        private byte[] otherArray;

        public int DoSomething(byte[] array)
        {
            Array.Copy(array, this.otherArray, array.Length);
            return array.Length;
        }
    }
}
