using System;
using System.Collections;
using System.Text;

namespace FileCompresser
{
    public class Golomb : IEncoder
    {
        public string Encode(string content)
        {
            var codeWords = new bool[Encoding.ASCII.GetByteCount(content) * 8];
            long idx = 0;

            int K = 64; 

            foreach (var c in content)
            {
                int prefixBits = c / K;

                //prefix
                for (int i = 0; i < prefixBits; i++)
                    codeWords[idx++] = false;

                //stop bit
                codeWords[idx++] = true;

                //sufix
                uint r = 0x1;
                for (int i = 0; i < (int)Math.Log(K, 2); i++)
                {
                    codeWords[idx++] = (r & c) > 0; 
                    r <<= 1;
                }
            }

            var bits = new BitArray(codeWords);
            var bytes = new byte[(long)Math.Ceiling(bits.Length / 8d) + 2];

            bytes[0] = 0; //indicator of the encoding type
            bytes[1] = (byte)K; //value of divisor

            bits.CopyTo(bytes, 2);

            var result = Encoding.ASCII.GetString(bytes).Substring(0, (int)idx / 8);            

            return result;
        }

        public string Decode(string content)
        {
            throw new NotImplementedException();
        }
    }
}
