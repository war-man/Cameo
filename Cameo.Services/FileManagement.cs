using Cameo.Common;
using Cameo.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cameo.Services
{
    public class FileManagement : IFileManagement
    {
        //private readonly IOptions<AppConfiguration> AppSettings;
        private AppConfiguration AppSettings;

        public FileManagement(IOptions<AppConfiguration> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public bool SaveFile(byte[] fileByte, string path)
        {
            bool result = false;

            if (fileByte != null)
            {
                try
                {
                    string rootPath = AppSettings.ApplicationRootPath;
                    path = path.Replace('/', '\\');

                    string target = rootPath + path;

                    FileStream file = File.Create(target);
                    file.Write(fileByte, 0, fileByte.Length);
                    file.Close();
                    result = true;
                }
                catch (Exception ex)
                {
                    string exceptionMessage = ex.Message;
                    result = false;
                }
            }

            return result;
        }


        public byte[] ToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
