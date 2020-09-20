using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public interface IEncoder
    {
        void Encode(string content);
        void Decode(byte[] content);
    }
}
