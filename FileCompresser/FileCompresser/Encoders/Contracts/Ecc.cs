using System;
using System.Collections.Generic;
using System.Collections;

namespace FileCompresser
{
    public class Ecc
    {
        // takes an array of 2 bytes (ERROR if array size is wrong) and return a crc coded byte
        public byte Crc(byte[] bytes)
        {
            if (bytes.Length != 2) {
                throw new Exception("CRC ERROR: byte array size is icorrect!");
            }

            BitArray bits = new BitArray(bytes);            // bits from byte array
            BitArray bits_9 = new BitArray(9);              // array for 9 bits
            List<bool> rest = new List<bool>();             // crc rest will be stored here

            bool[] g = new bool[] { true, false, false, false, false, false, true, true, true };
            BitArray div = new BitArray(g);                 // div array (G)

            int bitsCount = 9;
            int onePosition = 0;                            // store the number of bits we need to shift
            bool first = true;                              // to find the first/next 1
            for (int i = 0; i < bits.Length; i++)
            {                                               // loop to fill bits_9 and find the first 1
                if (bits[i] == true && first)
                {
                    onePosition = i;                        // fisrt significant bit
                    first = false;
                }
                if (i < 9)
                {
                    bits_9[i] = bits[i];                    // fill bits_9 array with first 9 bits
                }
            }

            int count = 0;                                  // count the number of bits we shifted the bits_9
            while (bitsCount + onePosition <= bits.Length)  // check if we still have more bits from bits array
            {
                bits_9.RightShift(onePosition);             // shift right until first 1 bit

                if (onePosition != 0)
                {
                    int aux = onePosition - 1;
                    for (int i = 8 - aux; i < bits_9.Length; i++)
                    {
                        bits_9[i] = bits[bitsCount++];      // because of shift, the lasts bits must be
                    }                                       // changed to match the original bits array
                }

                bits_9.Xor(div);                            // xor
                count = 0;                                  // store the shift number
                first = true;
                for (int i = 0; i < bits_9.Length; i++)
                {
                    if (bits_9[i] == true && first)         // check the first 1 again
                    {
                        onePosition = count;
                        first = false;
                        break;
                    }
                    else
                        count++;
                }
            }

            for (int i = 0; i < bits_9.Length; i++)         // remaining bits from bits_9 are put
            {                                               // in the rest list
                rest.Add(bits_9[i]);
            }

            while (bitsCount < bits.Length)
            {
                rest.Add(bits[bitsCount++]);                // remaining bits from data that were not
            }                                               // divided(xor) are put in the rest list

            int tamanho = rest.Count - 8;
            BitArray codewords = new BitArray(8);

            for (int i = 0; i < codewords.Length; i++)      // fill bitArray from bool array rest that
            {                                               // contains the crc rest
                codewords[i] = rest[tamanho++];
            }
            byte[] crcAux = new byte[1];
            codewords.CopyTo(crcAux, 0);                   // generate byte from codewords bitArray

            byte crcByte = crcAux[0];

            return crcByte;
        }

        // take a byte array(without header) and apply hamming(7, 4)
        public BitArray Hamming(byte[] bytes)
        {
            BitArray bits = new BitArray(bytes);           // bytes to bitArray

            int tam = (int)Math.Ceiling(bits.Count / 4d);   // number of hamming we need to apply
            BitArray hamming = new BitArray(tam * 7);       // multiple tam by 7 and we have the total number of bits
            BitArray s = new BitArray(4);                   // auxiliar bitArray for every 4 bits from bits bitArray

            int index = 0;
            int count4 = 0;
            for (int i = 0; i <= bits.Count; i++)           // loop through all bits from bits bitArray and;
            {                                               // every 4 bits store them in s bitArray and hamming bitArray and;
                if (count4 == 4)                            // apply hamming.
                {
                    BitArray parityBits = HammingTable(s, true);
                    hamming[index++] = parityBits[0];
                    hamming[index++] = parityBits[1];
                    hamming[index++] = parityBits[2];

                    count4 = 0;
                    if (i != bits.Count)
                    {
                        s[count4++] = bits[i];
                        hamming[index++] = bits[i];
                    }
                }
                else
                {
                    s[count4++] = bits[i];
                    hamming[index++] = bits[i];
                }
            }

            return hamming;                             // return bitArray with hamming coding
        }

        // take BitArray and return decoded BitArray
        public BitArray HammingDec(BitArray hamming)
        {
            int hamNumber = hamming.Count / 7;          // number of hamming codes
            int size = hamNumber / 2;                   // number of bytes

            BitArray hamBits = hamming;
            BitArray hammingDec = new BitArray(size * 8);

            BitArray aux = new BitArray(7);             // auxiliary BitArray to store every 7 bits from input
            int indexAux = 0;
            int indexDec = 0;
            for (int i = 0; i <= hamBits.Count; i++)    // loop through input BitArray and;
            {                                           // take every 7 bits and check their parity bits (last 3);
                if (i % 7 == 0 && i != 0)               // check for errors and take the first 4 bits from that 7.
                {
                    BitArray checkParity = HammingTable(aux, false);
                    hammingDec[indexDec++] = checkParity[0];
                    hammingDec[indexDec++] = checkParity[1];
                    hammingDec[indexDec++] = checkParity[2];
                    hammingDec[indexDec++] = checkParity[3];

                    indexAux = 0;
                    if (i != hamBits.Count)
                    {
                        aux[indexAux++] = hamBits[i];
                    }
                }
                else
                {
                    aux[indexAux++] = hamBits[i];
                }
            }

            return hammingDec;
        }
        
        // check parity bits and return: BitArray with 3 bits - if is encoding
        //                               BitArray with 4 bits - if is decoding
        public BitArray HammingTable(BitArray s, bool coding)
        {
            int data1 = Convert.ToInt32(s[0]);
            int data2 = Convert.ToInt32(s[1]);
            int data3 = Convert.ToInt32(s[2]);
            int data4 = Convert.ToInt32(s[3]);

            bool p1 = Convert.ToBoolean((data1 + data2 + data3) % 2);
            bool p2 = Convert.ToBoolean((data2 + data3 + data4) % 2);
            bool p3 = Convert.ToBoolean((data1 + data3 + data4) % 2);

            if (coding)                             // check if is coding or decoding
            {
                BitArray p = new BitArray(3);

                p[0] = p1;
                p[1] = p2;
                p[2] = p3;

                return p;
            }
            else
            {
                BitArray ham = new BitArray(4);
                ham[0] = s[0];
                ham[1] = s[1];
                ham[2] = s[2];
                ham[3] = s[3];

                bool par1 = false;
                bool par2 = false;
                bool par3 = false;

                if (p1 == s[4])                         // check if parity bits are ok
                    par1 = true;
                if (p2 == s[5])
                    par2 = true;
                if (p3 == s[6])
                    par3 = true;

                if (par1 && par2 && par3)               // all are ok
                {
                    // no fixing needed
                }
                else if ((par1 == false) && par2 && (par3 == false))    // data1 error
                {
                    if (s[0])                                           // s[0] is wrong, change bit
                        ham[0] = false;
                    else
                        ham[0] = true;
                }
                else if ((par1 == false) && (par2 == false) && par3)    // data2 error
                {
                    if (s[1])                                           // s[1] is wrong, change bit
                        ham[1] = false;
                    else
                        ham[1] = true;
                }
                else if ((par1 == false) && (par2 == false) && (par3 == false)) // data3 error
                {
                    if (s[2])                                           // s[2] is wrong, change bit
                        ham[2] = false;
                    else
                        ham[2] = true;
                }
                else if (par1 && (par2 == false) && (par3 == false))    // data4 error
                {
                    if (s[3])                                           // s[3] is wrong, change bit
                        ham[3] = false;
                    else
                        ham[3] = true;
                }
                else
                {
                    Console.WriteLine("Hamming Decoder ERROR: more than one bit are wrong, no fixing available!");
                }

                return ham;
            }
        }
    }
}
