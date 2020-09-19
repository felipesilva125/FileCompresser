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
        public void Encode(string content)
        {
            string path = Path.Combine(FileController.FILE_PATH, "teste");
            path = Path.ChangeExtension(path, FileController.COMPRESSING_EXTENSION);

            using (FileStream fileStream = File.Create(path))
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
            string path = Path.Combine(FileController.FILE_PATH, "teste");
            path = Path.ChangeExtension(path, FileController.DECOMPRESSING_EXTENSION);

            using (FileStream fileStream = File.Create(path))
            {
                var fileContent = new StringBuilder();
                var bytes = Encoding.ASCII.GetBytes(content);
                bytes = bytes.Skip(2).ToArray();

                int n = 0, i = 0;
                bool countUnary = true;
                var leftOverBinary = new StringBuilder();

                foreach (var b in bytes)
                {
                    var byteChar = Convert.ToChar(b);
                    if (countUnary)
                    {
                        if (byteChar.Equals('0'))
                            n++;
                        else if (byteChar.Equals('1'))
                            countUnary = false;

                        continue;
                    }

                    if (i <= n)
                    {
                        leftOverBinary.Append(byteChar.ToString());
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
