using System;

namespace FileCompresser
{
    public class Program
    {
        public static void Main()
        {            
            try
            {
                var fileName = RequestData.RequestFileName();
                var codificationType = RequestData.RequestCodificationType();

                var encoder = EncoderFactory.Create(codificationType);

                compressFile(fileName, encoder);
                decompressFile(fileName, encoder);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Environment.Exit(0);
            }

            Console.WriteLine("Compressing and decompressing made successfully!");
        }                

        private static void compressFile(string fileName, IEncoder encoder)
        {
            var fileContent = FileController.ReadFileContentString(fileName, FileController.READING_EXTENSION);
            encoder.Encode(fileContent, fileName);            
        }

        private static void decompressFile(string fileName, IEncoder encoder)
        {
            var fileContent = FileController.ReadFileContent(fileName, FileController.COMPRESSING_EXTENSION);            
            encoder.Decode(fileContent, fileName);            
        }
    }
}
