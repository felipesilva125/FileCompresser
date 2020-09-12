using System;

namespace FileCompresser
{
    public class Program
    {
        public static void Main()
        {            
            try
            {
                compressFile("alice29");
                //decompressFile("alice29");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");                
            }
        }        

        private static void compressFile(string fileName)
        {
            var fileContent = FileController.ReadFileContent(fileName);

            //passa pelo encoder

            FileController.WriteFileContent(fileName, fileContent);
        }

        private static void decompressFile(string fileName)
        {
            //lê arquivo
            var fileContent = FileController.ReadFileContent(fileName);

            //passa pelo decoder

            //grava arquivo
            FileController.WriteFileContent(fileName, fileContent);
        }
    }
}
