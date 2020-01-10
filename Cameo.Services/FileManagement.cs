using Cameo.Common;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NReco.VideoConverter;
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
        private readonly string projectPath = @"C:\Users\MIRAZAM\Documents\Visual Studio 2017\Projects\GitHub\Cameo\Cameo";

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

        public string ConvertVideoToMp4(string videoPath, string videoName)
        {
            videoPath = videoPath.Replace('/', '\\');

            var sourceFile = projectPath + videoPath + @"\" + videoName;

            string GUID = Guid.NewGuid().ToString();
            string newVideoName = GUID + "." + Format.mp4;
            var destinationFile = projectPath + videoPath + @"\" + newVideoName;

            //NRecoService.ConvertMedia(sourceFile, destinationFile, Format.mp4);
            CopyFile(videoPath, videoName, videoPath, newVideoName);

            return newVideoName;
        }

        public bool DeleteFile(string fileUrl)
        {
            try
            {
                string rootPath = AppSettings.ApplicationRootPath;
                fileUrl = fileUrl.Replace('/', '\\');
                string target = rootPath + fileUrl;

                if (System.IO.File.Exists(target))
                    System.IO.File.Delete(target);

                //if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~" + fileUrl)))
                //    System.IO.File.Delete(HttpContext.Current.Server.MapPath("~" + fileUrl));
                return true;
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath">/Uploads</param>
        /// <param name="sourceFilename">videoName.avi</param>
        /// <param name="destinationPath">/Uploads</param>
        /// <param name="destinationFilename">newVideoName.mp4</param>
        /// <returns></returns>
        public bool CopyFile(string sourcePath, string sourceFilename, string destinationPath, string destinationFilename)
        {
            sourcePath = sourcePath.Replace('/', '\\');
            sourcePath = projectPath + sourcePath + @"\";
            sourceFilename = sourcePath + sourceFilename;

            destinationPath = destinationPath.Replace('/', '\\');
            destinationPath = projectPath + destinationPath + @"\";
            destinationFilename = destinationPath + destinationFilename;

            try
            {
                if (Directory.Exists(destinationPath))
                {
                    if (File.Exists(sourceFilename))
                        File.Copy(sourceFilename, destinationFilename);
                    else
                        throw new Exception("Source file not found");
                }
                else
                    throw new Exception("Target Path not found");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetFileAbsolutePath(string path, string filename)
        {
            string curDir = Directory.GetCurrentDirectory();
            string absolutePath = curDir + path + "/" + filename;
            absolutePath = absolutePath.Replace('/', '\\');

            return absolutePath;
        }
    }
}
