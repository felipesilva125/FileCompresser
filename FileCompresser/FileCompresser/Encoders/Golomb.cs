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
                int prefixBits = Math.DivRem(c, K, out int leftOver);                

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
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.DECOMPRESSING_EXTENSION);

            var K = bytes[1]; //get K from encoding

            using (var stream = File.Create(path))
            {
                var fileContent = new StringBuilder();
                bytes = bytes.Skip(2).ToArray();

                var bits = new BitArray(bytes);
                var bools = new bool[bits.Length];
                bits.CopyTo(bools, 0);

                var builder = new StringBuilder();

                var qtyBits = (int)Math.Log(K, 2);
                int n = 0, i = 0;
                var countUnary = true;
                var leftOverBinary = new StringBuilder();

                foreach (var b in bools)
                {                               
                    if (countUnary)
                    {
                        if (!b)
                            n++;
                        else
                            countUnary = false;

                        continue;
                    }

                    if (i <= qtyBits)
                    {
                        leftOverBinary.Append(b ? "1" : "0");
                        i++;

                        if (i == qtyBits)
                        {
                            var leftOver = Convert.ToInt32(leftOverBinary.ToString(), 2);
                            var qty = n * K + leftOver;
                            fileContent.Append(((char)qty).ToString());

                            i = 0;
                            n = 0;
                            countUnary = true;
                            leftOverBinary = new StringBuilder();
                        }
                    }                    
                }

                var contentBytes = Encoding.ASCII.GetBytes(fileContent.ToString());
                var readOnlySpan = new ReadOnlySpan<byte>(contentBytes);

                stream.Write(readOnlySpan);

                var teste = builder.ToString();
            }
        }
    }
}
