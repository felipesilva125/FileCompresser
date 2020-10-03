using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCompresser
{
    public class Unary : IEncoder
    {
        public void Encode(string content, string fileName)
        {
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.COMPRESSING_EXTENSION);

            var bools = new List<bool>();
            var bytes = Encoding.ASCII.GetBytes(content);

            foreach (var @byte in bytes)
            {
                var valueByte = @byte;
                var count = 0;

                while (count < valueByte)
                {
                    bools.Add(false); // 1 
                    count++;
                }
                bools.Add(true); // 0
            }

            var convertBoolsInBytes = new byte[(int)Math.Ceiling(bools.Count / 8d)];
            var bits = new BitArray(bools.ToArray());
            bits.CopyTo(convertBoolsInBytes, 0);

            var byteList = convertBoolsInBytes.ToList();
            byteList.Insert(0, 3);
            byteList.Insert(1, 0);
            File.WriteAllBytes(path, byteList.ToArray());
        }

        public void Decode(byte[] bytes, string fileName)
        {
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.DECOMPRESSING_EXTENSION);

            bytes = bytes.Skip(2).ToArray();

            var bits = new BitArray(bytes);
            var bools = new bool[bits.Length];
            bits.CopyTo(bools, 0);

            var bytesToSave = new List<byte>();

            byte n = 0;

            foreach (var b in bools)
            {
                if (!b)
                    n++;
                else
                {
                    bytesToSave.Add(n);
                    n = 0;
                }
            }
            File.WriteAllBytes(path, bytesToSave.ToArray());
        }
    }
}
