using Cameo.Common;
using Cameo.Models;
using System;

namespace Cameo.ViewModels
{
    public class AttachmentDetailsVM
    {
        public int ID { get; set; }
        public string Url { get; set; }

        public AttachmentDetailsVM()
        { }

        public AttachmentDetailsVM(Attachment model)
        {
            if (model == null)
            {
                this.Url = AppData.Configuration.NoPhotoUrl;
                return;
            }

            this.ID = model.ID;
            if (this.ID > 0)
                //this.Url = model.Path + "/" + model.GUID + "." + model.Extension;
                this.Url = "https://firebasestorage.googleapis.com/v0/b/cameo-uz.appspot.com/o/" + model.Path.Replace("/", "%2F") + "%2F" + model.Filename + "?" + model.UrlParameters;
            else
                this.Url = AppData.Configuration.NoPhotoUrl;
        }
    }

    public class UploadFileVM
    {
        public string Filename { get; set; } //"file.ext"
        public string Path { get; set; } //"path/to/file/file.txt"
        public long Size { get; set; } //byte
        public string ContentType { get; set; } //"image/jpeg"
        public string DownloadUrl { get; set; } //"https://gerlgnenr.com?param1=val1&param2=val2..."
        public string FileType { get; set; } //Constants.FileTypes.VIDEO_REQUEST_VIDEO
        public int? ModelID { get; set; }

        public Attachment ToModel()
        {
            Attachment attachment = new Attachment()
            {
                GUID = Guid.NewGuid().ToString(),
                Filename = this.Filename,
                Size = this.Size,
                MimeType = this.ContentType
            };

            string path = "";
            string[] tmp = this.Path.Split('/');
            for (int i = 0; i < tmp.Length - 1; i++)
            {
                path += tmp[i];
                if (i < tmp.Length - 2)
                    path += "/";
            }
            attachment.Path = path;

            tmp = this.DownloadUrl.Split('?');
            if (tmp.Length > 1)
                attachment.UrlParameters = tmp[1];

            return attachment;
        }
    }
}
