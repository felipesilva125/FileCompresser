using System;
using System.Collections.Generic;
using System.Text;

namespace FileCompresser
{
    public interface IEncoder
    {
        string Encode(string content);
        string Decode(string content);
    }
}
