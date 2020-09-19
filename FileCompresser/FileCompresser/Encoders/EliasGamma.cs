using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCompresser
{
    public class EliasGamma : IEncoder
    {
        public void Encode(string content)
        {
            //var bytes = new List<byte>();

            //bytes.Add(1); //indicator of the encoding type
            //bytes.Add(0);

            //File.WriteAllBytes(@"C:\Temp\teste.cod", bytes.ToArray());

            using (FileStream fileStream = File.Create(@"C:\Temp\teste.txt"))
            {
                fileStream.WriteByte(1);
                fileStream.WriteByte(0);

                foreach (var c in content)
                {
                    int pow = (int)(Math.Log(c) / Math.Log(2));
                    var biggestPow = (int)Math.Pow(2, pow);

                    var codeword = new StringBuilder();

                    for (int i = 0; i < pow; i++)
                        codeword.Append("0");

                    codeword.Append("1");

                    var leftOver = c - biggestPow;
                    codeword.Append(Convert.ToString(leftOver, 2).PadLeft(pow, '0'));

                    var bytes = Encoding.ASCII.GetBytes(codeword.ToString());

                    foreach (var b in bytes)
                        fileStream.WriteByte(b);                    
                }

                fileStream.Close();
            }
        }

        public void Decode(string content)
        {
            var texto = new StringBuilder();

            var bytes = Encoding.ASCII.GetBytes(content);

            foreach (var b in bytes)
            {
                var bits = new BitArray(b.SelfArray());
            }

            texto.Append('c');

            File.WriteAllText(@"C:\Temp\teste.dec", texto.ToString());
        }
    }
}
