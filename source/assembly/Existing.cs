namespace DevExperience.Assembly
{
    using System;
    using System.Threading;

    public class Existing
    {
        private byte[] otherArray;

        public int DoSomething(MyArray array)
        {
            var arrayBa = array.ba;
            var baLongLength = arrayBa.LongLength;
            //for (long i = 0; i < baLongLength; i++)
            //{
            //    arrayBa[i] = 1;
            //    //Thread.Sleep(1);
            //}

            return arrayBa.Length;
        }
    }
}
