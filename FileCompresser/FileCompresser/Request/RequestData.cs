using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileCompresser
{
    public static class RequestData
    {
        public static string RequestFileName()
        {
            string fileName;

            do
            {
                try
                {
                    Console.WriteLine("Enter the file name:");
                    fileName = Console.ReadLine();

                    FileController.ValidateFile(fileName, FileController.READING_EXTENSION);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    continue;
                }

                break;

            } while (true);

            return fileName;
        }

        public static CodificationType RequestCodificationType()
        {
            CodificationType selectedType;

            do
            {
                Console.WriteLine("Enter the codification type:");
                Console.WriteLine("0 - Golomb");
                Console.WriteLine("1 - Elias Gamma");
                Console.WriteLine("2 - Fibonacci");
                Console.WriteLine("3 - Unary");
                Console.WriteLine("4 - Delta");

                var option = Console.ReadLine();                

                if (!getCodificationOptions().ToArray().Contains(option))
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    continue;
                }

                selectedType = Enum.Parse<CodificationType>(option);

                break;

            } while (true);

            return selectedType;
        }

        private static IEnumerable<string> getCodificationOptions()
        {
            foreach (int value in Enum.GetValues(typeof(CodificationType)))
                yield return value.ToString();
        }

        public static byte RequestDivisor()
        {
            do
            {
                Console.WriteLine("Enter the divisor:");
                var divisor = Console.ReadLine();

                if (!byte.TryParse(divisor, out var result))
                {
                    Console.WriteLine("Invalid value. Please try again.");
                    continue;
                }

                return result;

            } while (true);            
        }
    }
}
