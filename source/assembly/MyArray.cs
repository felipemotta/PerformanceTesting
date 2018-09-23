namespace DevExperience.Assembly
{
    using System;

    public class MyArray : MarshalByRefObject
    {
        public MyArray(byte[] newByteArray) => this.ByteArray = newByteArray;

        public byte[] ByteArray { get;  }
    }
}