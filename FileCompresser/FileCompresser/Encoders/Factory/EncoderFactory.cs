using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public static class EncoderFactory
    {
        public static IEncoder Create(CodificationType codificationType)
        {
            switch (codificationType)
            {
                case CodificationType.Golomb:
                    return new Golomb();
                case CodificationType.EliasGamma:
                    return new EliasGamma();
                case CodificationType.Fibonacci:
                    return new Fibonacci();
                case CodificationType.Unary:
                    return new Unary();
                case CodificationType.Delta:
                    return new Delta();
            }

            return null;
        }
    }
}
