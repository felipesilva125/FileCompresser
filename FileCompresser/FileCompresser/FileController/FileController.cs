using System.IO;

namespace FileCompresser
{
    public static class FileController
    {
        private const string FILE_PATH = @"C:\Temp";
        public const string READING_EXTENSION = "txt";
        public const string COMPRESSING_EXTENSION = "cod";
        public const string DECOMPRESSING_EXTENSION = "dec";

        public static string ReadFileContent(string fileName, string fileExtension)
        {            
            var path = getFullPath(fileName, fileExtension);

            if (!File.Exists(path))
                throw new FileNotFoundException($"File {fileName} not found!");

            return File.ReadAllText(path);
        }

        public static void WriteFileContent(string fileName, string fileExtension, string fileContent)
        {
            var path = getFullPath(fileName, fileExtension);            
            File.WriteAllText(path, fileContent);
        }

        private static string getFullPath(string fileName, string extension)
        {
            fileName = Path.ChangeExtension(fileName, extension);
            var path = Path.Combine(FILE_PATH, fileName);

            return path;
        }
    }        
}
