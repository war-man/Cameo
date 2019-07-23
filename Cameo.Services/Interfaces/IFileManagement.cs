using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IFileManagement
    {
        bool SaveFile(byte[] fileByte, string path);
        byte[] ToByteArray(Stream input);
    }
}