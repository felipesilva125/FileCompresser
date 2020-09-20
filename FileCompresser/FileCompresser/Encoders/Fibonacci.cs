using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public class Fibonacci : IEncoder
    {
        public void Encode(string content)
        {
            string path = Path.Combine(FileController.FILE_PATH, "teste");
            path = Path.ChangeExtension(path, FileController.COMPRESSING_EXTENSION);
            
            byte[] bytes = Encoding.ASCII.GetBytes(content);
            int[] bytesAsInts = Array.ConvertAll(bytes, c => Convert.ToInt32(c));

            int[] fib = new int[20];
            fib[0] = 0;
            fib[1] = 1;

            for (int i = 2; i < fib.Length; i++)
                fib[i] = fib[i - 1] + fib[i - 2];

            int count = 0;
            int number;
            List<int> fibLocations = new List<int>();
            List<string> codewords = new List<string>();

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
            int totalLength = 0;

            // generate the codewords
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
                    totalLength += codewordAux.Length;
                    codewordAux = "";
                }
            }

            BitArray bits = new BitArray(totalLength);
            BitArray bits8 = new BitArray(8);

            // fill bits array with codewords
            count = 0;
            for (int i = 0; i < codewords.Count; i++)
            {
                var res = new BitArray(codewords[i].Select(c => c == '1').ToArray());
                for (int j = 0; j < res.Count; j++)
                {
                    bits[count++] = res[j];
                }
            }

            int tam = bits.Count / 8;
            int resto = bits.Count % 8;
            byte[] bitToByte = new byte[tam];

            count = 0;
            int bitCount = 0;
            for (int i = 0; i < bits.Count; i++) {
                if (i % 8 == 0 && i != 0) {
                    bits8.CopyTo(bitToByte, bitCount++);
                    count = 0;
                    bits8[count++] = bits[i];
                }
                else {
                    bits8[count++] = bits[i];
                }
            }
            count = 0;
            tam = bits.Count - 1;
            for (int i = 0; i < 8; i++) {
                if (resto > 0) {
                    bits8[count++] = bits[tam--];
                    resto--;
                }
                else {
                    bits8[count++] = false;
                }
            }
            bits8.CopyTo(bitToByte, bitCount);

            File.WriteAllBytes(path, bitToByte);
        }

        public void Decode(byte[] bytes)
        {
            string path = Path.Combine(FileController.FILE_PATH, "teste");
            path = Path.ChangeExtension(path, FileController.DECOMPRESSING_EXTENSION);
            
            BitArray bits = new BitArray(bytes);
            List<string> codewords = new List<string>();
            List<int> intCodes = new List<int>();
            List<char> charCodes = new List<char>();

            int[] fib = new int[20];
            fib[0] = 0;
            fib[1] = 1;
            for (int i = 2; i < fib.Length; i++)
                fib[i] = fib[i - 1] + fib[i - 2];

            string codesAux = "";
            bool a;
            bool b;
            for (int i = 0; i < bits.Count; i++) {
                if (i < bits.Count - 1) {
                    a = bits[i];
                    b = bits[i + 1];
                    if (a == true && b == true)
                    {
                        codesAux += "1";              // codeword with no stop bit
                        codewords.Add(codesAux);
                        codesAux = "";
                        ++i;
                    }
                    else
                    {
                        if (bits[i] == false)
                            codesAux += "0";
                        else
                            codesAux += "1";
                    }
                }
                else
                {
                    if (bits[i] == false)
                            codesAux += "0";
                        else
                            codesAux += "1";
                }
            }

            int sum = 0;
            char numberAux;
            for (int i = 0; i < codewords.Count; i++)
            {
                for (int j = 0; j < codewords[i].Length; j++)
                {
                    numberAux = codewords[i][j];
                    if (numberAux == '1')
                    {
                        sum += fib[j + 2];
                    }
                }
                intCodes.Add(sum);
                sum = 0;
            }

            string result = "";
            for (int i = 0; i < intCodes.Count; i++)
            {
                charCodes.Add(Convert.ToChar(intCodes[i]));
                result += charCodes[i];
            }
            
            File.WriteAllText(path, result);
        }
    }
}
