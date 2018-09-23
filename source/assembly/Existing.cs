namespace DevExperience.Assembly
{
    public class Existing
    {
        public int DoSomething(MyArray array)
        {
            // Consumes the size defined for the byte array
            var arrayBa = array.ByteArray; 

            // Some Dummy logic
            var baLongLength = arrayBa.LongLength;
            return arrayBa.Length;
        }
    }
}
