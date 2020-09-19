using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public class Fibonacci : IEncoder
    {
        public void Encode(string content)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(content);
            int[] bytesAsInts = Array.ConvertAll(bytes, c => Convert.ToInt32(c));

            int[] fib = new int[20];
            fib[0] = 0;
            fib[1] = 1;

            for (int i = 2; i < fib.Length; i++)   // fib series array
                fib[i] = fib[i - 1] + fib[i - 2];

            int count = 0;
            int number;
            List<int> fibLocations = new List<int>();
            List<string> codewords = new List<string>();

            // find the locations of the number sum in fib array
            while (count < bytesAsInts.Length)
            {
                number = bytesAsInts[count];
                while (number > 0)
                {
                    int aux = 0;
                    while (fib[aux] <= number)
                    {
                        aux++;
                    }
                    number = number - fib[aux - 1];
                    fibLocations.Add(aux - 1);
                }
                fibLocations.Add(0);    // Separates the locations for each number
                count++;
            }

            int[] codesAux = new int[20];
            string codewordAux = "";
            int positionAux = fibLocations[0];

            // generate codewords
            for (int i = 0; i < fibLocations.Count; i++)
            {
                if (i > 0)
                {
                    if (fibLocations[i - 1] == 0)
                    {
                        positionAux = fibLocations[i];
                    }
                }
                if (fibLocations[i] != 0)
                {
                    codesAux[fibLocations[i]] = 1;
                }
                else
                {
                    codesAux[positionAux + 1] = 1;  // stop bit
                    for (int j = 2; j <= positionAux + 1; j++)
                    {
                        codewordAux += codesAux[j];
                    }
                    Array.Clear(codesAux, 0, codesAux.Length);
                    codewords.Add(codewordAux);
                    codewordAux = "";
                }
            }

            // returning a normal string with the codewords - .cod will be larger
            string result = "";
            for (int i = 0; i < codewords.Count; i++)
            {
                result += codewords[i];
            }

            //return result;
        }

        // NOT WORKING
        public void Decode(string content)
        {
            List<string> codewords = new List<string>();
            List<int> intCodes = new List<int>();
            List<char> charCodes = new List<char>();
            string codewordAux = "";

            int[] fib = new int[20];
            fib[0] = 0;
            fib[1] = 1;
            for (int i = 2; i < fib.Length; i++)
                fib[i] = fib[i - 1] + fib[i - 2];

            // error here to split the codewords to the array
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == 1 && content[i + 1] == 1)
                {
                    codewordAux += content[i];     // dont need the stop bit (content[i + 1])
                    codewords.Add(codewordAux);
                    codewordAux = "";
                    i++;
                }
                else
                {
                    codewordAux += content[i];
                }
            }

            int sum = 0;
            int numberAux = 0;
            // sum the fib number to get the decoded number
            for (int i = 0; i < codewords.Count; i++) {
                for (int j = 0; j < codewords[i].Length; j++) {
                    numberAux = Convert.ToInt32(codewords[i][j]);
                    if (numberAux == 1) {
                        sum += fib[j+2];
                    }
                }
                intCodes.Add(sum);
                sum = 0;
            }

            // return numbers as ASCII chars
            string result = "";
            for (int i = 0; i < intCodes.Count; i++) {
                charCodes.Add(Convert.ToChar(intCodes[i]));
                result += charCodes[i];
            }

            //return result;
        }
    }
}
