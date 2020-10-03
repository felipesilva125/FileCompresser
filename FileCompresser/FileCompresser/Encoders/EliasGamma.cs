using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCompresser
{
    public class EliasGamma : IEncoder
    {
        public void Encode(string content, string fileName)
        {
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.COMPRESSING_EXTENSION);

            var bools = new List<bool>();

            foreach (var c in content)
            {
                int pow = (int)(Math.Log(c) / Math.Log(2));
                var biggestPow = (int)Math.Pow(2, pow);

                var codeword = new StringBuilder();

                for (int i = 0; i < pow; i++)
                    bools.Add(false);//codeword.Append("0");                

                bools.Add(true);//codeword.Append("1");

                var leftOver = c - biggestPow;
                //codeword.Append(Convert.ToString(leftOver, 2).PadLeft(pow, '0'));

                var binaryString = Convert.ToString(leftOver, 2).PadLeft(pow, '0');
                foreach (var bit in binaryString)
                    bools.Add(bit == '1');

                //var bytes = Encoding.ASCII.GetBytes(codeword.ToString());

                //foreach (var b in bytes)
                //    fileStream.WriteByte(b);
            }

            var bytes = new byte[(int)Math.Ceiling(bools.Count / 8d)];

            var bits = new BitArray(bools.ToArray());
            bits.CopyTo(bytes, 0);

            //var bytes = new List<byte>();

            //int skip = 0;
            //int qtyList = bools.Count() <= 8 ? 1 : (int)Math.Ceiling(bools.Count / 8d);

            //for (int i = 1; i <= qtyList; i++)
            //{
            //    bytes.Add(ConvertBoolArrayToByte(bools.Skip(skip).Take(8).ToArray()));
            //    skip += 8;
            //}

            var byteList = bytes.ToList();
            byteList.Insert(0, 1);
            byteList.Insert(1, 0);
            
            File.WriteAllBytes(path, byteList.ToArray());
        }

        public void Decode(byte[] bytes, string fileName)
        {
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.DECOMPRESSING_EXTENSION);

            using (FileStream fileStream = File.Create(path))
            {
                var fileContent = new StringBuilder();                
                bytes = bytes.Skip(2).ToArray();

                var bits = new BitArray(bytes);
                var bools = new bool[bits.Length];
                bits.CopyTo(bools, 0);

                int n = 0, i = 0;
                bool countUnary = true;
                var leftOverBinary = new StringBuilder();

                foreach (var b in bools)
                {
                    //var byteChar = Convert.ToChar(b);
                    if (countUnary)
                    {
                        if (!b)
                            n++;
                        else
                            countUnary = false;

                        continue;
                    }

                    if (i <= n)
                    {
                        leftOverBinary.Append(b ? "1" : "0");
                        i++;

                        if (i == n)
                        {
                            var leftOver = Convert.ToInt32(leftOverBinary.ToString(), 2);
                            var asc = (int)Math.Pow(2, n) + leftOver;
                            fileContent.Append(((char)asc).ToString());

                            i = 0;
                            n = 0;
                            countUnary = true;
                            leftOverBinary = new StringBuilder();
                        }
                    }
                }

                var contentBytes = Encoding.ASCII.GetBytes(fileContent.ToString());
                var readOnlySpan = new ReadOnlySpan<byte>(contentBytes);

                fileStream.Write(readOnlySpan);
            }
        }
    }
}
