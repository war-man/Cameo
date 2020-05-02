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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoPath">/Uploads/asdf/df</param>
        /// <param name="videoName">video.mp4</param>
        /// <returns>new video name: newName.mp4</returns>
        //string ConvertVideoToMp4(string videoPath, string videoName);
        bool DeleteFile(string fileUrl);
        //bool CopyFile(string sourcePath, string sourceFilename, string destinationPath, string destinationFilename);
        string GetFileAbsolutePath(string path, string filename);
    }
}