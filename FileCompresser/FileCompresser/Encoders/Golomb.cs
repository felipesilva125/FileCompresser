using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCompresser
{
    public class Golomb : IEncoder
    {
        public void Encode(string content, string fileName)
        {
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.COMPRESSING_EXTENSION);

            int K = RequestData.RequestDivisor();

            var bools = new List<bool>();
            var qtyBits = (int)Math.Log(K, 2);

            foreach (var c in content)
            {                                
                int prefixBits = c >> qtyBits; //Math.DivRem(c, K, out int leftOver);
                int leftOver = c & (K - 1);

                var builder = new StringBuilder();

                //prefix
                for (int i = 0; i < prefixBits; i++)
                    bools.Add(false);

                //stop bit
                bools.Add(true);

                var binary = Convert.ToString(leftOver, 2).PadLeft(qtyBits, '0');
                foreach (var bit in binary)
                    bools.Add(bit == '1');
            }

            var bytes = new byte[(int)Math.Ceiling(bools.Count / 8d)];

            var bits = new BitArray(bools.ToArray());
            bits.CopyTo(bytes, 0);

            var byteList = bytes.ToList();
            byteList.Insert(0, 0); //indicator of the encoding type
            byteList.Insert(1, (byte)K); //value of divisor

            File.WriteAllBytes(path, byteList.ToArray());            
        }

        public void Decode(byte[] bytes, string fileName)
        {            
            var K = bytes[1]; //get K from encoding

            var bytesWithoutK = new byte[bytes.Length - 2];
            Array.Copy(bytes, 2, bytesWithoutK, 0, bytes.Length - 2);

            var builder = new StringBuilder();

            foreach (var codeWord in bytesWithoutK)
            {
                var bits = new BitArray(codeWord.SelfArray());

                var aaaaa = Convert.ToString(codeWord, 2);

                var x = (int)Math.Log(K, 2);

                int prefix = 0;
                for (int i = 0; i < x - 1; i++)
                {
                    if (!bits[i])
                        prefix++;
                }

                var binarySufix = new StringBuilder();
                for (int i = x; i < bits.Length; i++)
                {
                    if (bits[i])
                        binarySufix.Append("1");
                    else
                        binarySufix.Append("0");
                }

                int sufix = Convert.ToInt32(binarySufix.ToString(), 2);
                int result = prefix + sufix;

                builder.Append(Encoding.ASCII.GetString(((byte)result).SelfArray()));
            }

            var teste = builder.ToString();
        }
    }
}
