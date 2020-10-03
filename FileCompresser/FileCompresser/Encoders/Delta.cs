using System.Text;

namespace FileCompresser
{
    public class Delta : IEncoder
    {
        public void Encode(string content, string fileName)
        {
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.COMPRESSING_EXTENSION);

            byte[] bytes = Encoding.UTF8.GetBytes(content);

            byte last = 0;
            byte original;
            int i;
            for (i = 0; i < bytes.Length; i++)
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

            File.WriteAllBytes(path, shiftRight);
        }

        public void Decode(byte[] bytes, string fileName)
        {            
            string path = Path.Combine(FileController.FILE_PATH, fileName);
            path = Path.ChangeExtension(path, FileController.DECOMPRESSING_EXTENSION);

            byte[] arqBytes = bytes;
            byte[] decoded = new byte[bytes.Length - 2];   // heading is not needed

            byte last = 0;
            int count = 0;
            for (int i = 2; i < bytes.Length; i++)  // skip the first 2 elements (heading)
            {
                bytes[i] += last;
                last = bytes[i];
                decoded[count++] = last;
            }

            File.WriteAllBytes(path, decoded);
        }
    }
}
