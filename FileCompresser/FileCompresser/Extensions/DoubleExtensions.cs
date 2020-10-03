using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public static class DoubleExtensions
    {
        public static double IgnoreNegativeResult(this double input)
        {
            return input < 0 ? 0 : input;
        }
    }
}
