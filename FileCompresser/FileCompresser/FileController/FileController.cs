using System;
using System.IO;

namespace FileCompresser
{
    public static class FileController
    {
        public const string FILE_PATH = @"C:\Temp";
        public const string READING_EXTENSION = "txt";
        public const string COMPRESSING_EXTENSION = "cod";
        public const string DECOMPRESSING_EXTENSION = "dec";

        public static byte[] ReadFileContent(string fileName, string fileExtension)
        {            
            var path = getPath(fileName, fileExtension);            
            return File.ReadAllBytes(path);
        }

        public static string ReadFileContentString(string fileName, string fileExtension)
        {
            string path = getPath(fileName, fileExtension);
            return File.ReadAllText(path);
        }

        private static string getPath(string fileName, string fileExtension)
        {
            ValidateFile(fileName, fileExtension);
            var path = getFullPath(fileName, fileExtension);
            return path;
        }

        private static string getFullPath(string fileName, string extension)
        {
            fileName = Path.ChangeExtension(fileName, extension);
            var path = Path.Combine(FILE_PATH, fileName);

            return path;
        }

        internal static void ValidateFile(string fileName, string fileExtension)
        {
            var path = getFullPath(fileName, fileExtension);

            if (!File.Exists(path))
                throw new FileNotFoundException($"File {fileName} not found!");
        }
    }        
}
