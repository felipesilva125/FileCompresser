using System.IO;

namespace FileCompresser
{
    public static class FileController
    {
        private const string FILE_PATH = @"C:\Temp";
        private const string READING_EXTENSION = "txt";
        private const string COMPRESSING_EXTENSION = "cod";
        private const string DECOMPRESSING_EXTENSION = "dec";

        public static string ReadFileContent(string fileName)
        {            
            var path = getFullPath(fileName, READING_EXTENSION);

            if (!File.Exists(path))
                throw new FileNotFoundException($"File {fileName} not found!");

            return File.ReadAllText(path);
        }

        public static void WriteFileContent(string fileName, string fileContent)
        {
            var path = getFullPath(fileName, COMPRESSING_EXTENSION);

            if (File.Exists(path))
                throw new FileNotFoundException($"File {fileName} already exists!");

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
