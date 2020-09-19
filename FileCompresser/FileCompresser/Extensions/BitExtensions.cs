using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public static class BitExtensions
    {
        public static byte ConvertToByte(this BitArray bits)
        {
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }
    }
}
