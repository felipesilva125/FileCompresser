using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileCompresser
{
    public class Golomb : IEncoder
    {
        public void Encode(string content)
        {
            int K = 64;

            //int idx = 2;

            var bytes = new List<byte>();//[(bits.Length / 8) + 2];

            bytes.Add(0); //indicator of the encoding type
            bytes.Add((byte)K); //value of divisor

            var bits = new StringBuilder();

            foreach (var c in content)
            {
                //var codeWord = new List<bool>();

                var qtyBits = (int)Math.Log(K, 2);                

                int prefixBits = c >> qtyBits; //Math.DivRem(c, K, out int leftOver);
                int leftOver = c & (K - 1);

                var builder = new StringBuilder();

                //prefix
                for (int i = 0; i < prefixBits; i++)
                    builder.Append("0"); // codeWord.Add(false);

                //stop bit
                builder.Append("1");//codeWord.Add(true);                

                var binary = Convert.ToString(leftOver, 2).PadLeft(qtyBits, '0');
                builder.Append(binary);

                //bits.Append(builder.ToString());

                bytes.Add(Convert.ToByte(builder.ToString(), 2));

                //foreach (var binValue in binary)
                //{
                //    if (binValue.Equals('0'))
                //        codeWord.Add(false);
                //    else
                //        codeWord.Add(true);
                //}

                //var bits = new BitArray(codeWord.ToArray());
                //bytes[idx++] = Encoding.ASCII.GetBytes();
            }            

            //var stringBits = bits.ToString();

            //var bytesAsStrings = stringBits.Select((c, i) => new { Char = c, Index = i })
            //                               .GroupBy(x => x.Index / 8)
            //                               .Select(g => new string(g.Select(x => x.Char).ToArray()));

            //bytes.AddRange(bytesAsStrings.Select(s => Convert.ToByte(s, 2)));
            
            //var bytes = new byte[(long)Math.Ceiling(bits.Length / 8d) + 2];           

            //bits.CopyTo(bytes, 2);            

            var result = Encoding.ASCII.GetString(bytes.ToArray());            
        }

        public void Decode(string content)
        {
            var bytes = Encoding.ASCII.GetBytes(content);

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
