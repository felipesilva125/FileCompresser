using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public class Delta : IEncoder
    {
        // NOT WORKING RIGTH
        public string Encode(string content)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(content);

            byte last = 0;
            byte original;
            int i;
            for (i = 0; i < bytes.Length; i++) // encode with the deltas
            {
                original = bytes[i];
                bytes[i] -= last;
                last = original;
            }

            byte[] shiftRight = new byte[bytes.Length+2];
            for (i = 0; i < bytes.Length; i++)
            {
                shiftRight[(i + 2) % shiftRight.Length] = bytes[i];
            }
            
            shiftRight[0] = 4; // Delta number
            shiftRight[1] = 0; // Only for Golomb K

            // problem - some bytes dont have corresponding character
            // decoder will have problems to decode, leading to execution errors
            string result = Encoding.ASCII.GetString(shiftRight);

            return result;
        }

        public string Decode(string content)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(content);
            byte[] decoded = new byte[bytes.Length - 2];   // heading is not needed

            byte last = 0;
            int count = 0;
            for (int i = 2; i < bytes.Length; i++)  // skip the first 2 elements (heading)
            {
                bytes[i] += last;
                last = bytes[i];
                decoded[count++] = last;
            }

            string result = Encoding.ASCII.GetString(decoded);
            return result;
        }
    }
}
