using System;
using System.IO;

namespace FileCompresser
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Inform the file name:");
            var fileName = Console.ReadLine();

            try
            {
                compressFile(fileName);
                decompressFile(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Environment.Exit(0);
            }

            Console.WriteLine("Compressing and decompressing made successfully!");
        }        

        private static void compressFile(string fileName)
        {
            var fileContent = FileController.ReadFileContent(fileName, FileController.READING_EXTENSION);

            //passa pelo encoder

            FileController.WriteFileContent(fileName, FileController.COMPRESSING_EXTENSION, fileContent);
        }

        private static void decompressFile(string fileName)
        {            
            var fileContent = FileController.ReadFileContent(fileName, FileController.COMPRESSING_EXTENSION);

            //passa pelo decoder
            
            FileController.WriteFileContent(fileName, FileController.DECOMPRESSING_EXTENSION, fileContent);
        }
    }
}
