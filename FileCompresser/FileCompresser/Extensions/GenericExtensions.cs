using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public static class GenericExtensions
    {
        public static T[] SelfArray<T>(this T input) => new T[] { input };
    }
}
