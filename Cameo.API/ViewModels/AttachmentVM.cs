using Cameo.Common;
using Cameo.Models;
using System;

namespace Cameo.API.ViewModels
{
    public class AttachmentDetailsVM
    {
        public int id { get; set; }
        public string url { get; set; }

        public AttachmentDetailsVM()
        { }

        public AttachmentDetailsVM(Attachment model)
        {
            if (model == null)
            {
                //this.url = AppData.Configuration.NoPhotoUrl;
                return;
            }

            this.id = model.ID;
            if (this.id > 0)
                //this.url = model.Path + "/" + model.GUID + "." + model.Extension;
                this.url = "https://firebasestorage.googleapis.com/v0/b/cameo-uz.appspot.com/o/" + model.Path + "%2F" + model.Filename + "?" + model.UrlParameters;
            else
                this.url = AppData.Configuration.NoPhotoUrl;
        }

        public static AttachmentDetailsVM ToVM(Attachment model)
        {
            if (model == null)
                return null;

            return new AttachmentDetailsVM(model);
        }
    }

    public class UploadFileVM
    {
        public string filename { get; set; } //"file.ext"
        public string path { get; set; } //"path/to/file/file.txt"
        public long size { get; set; } //byte
        public string content_type { get; set; } //"image/jpeg"
        public string download_url { get; set; } //"https://gerlgnenr.com?param1=val1&param2=val2..."
        public string file_type { get; set; } //Constants.FileTypes.VIDEO_REQUEST_VIDEO
        public int? model_id { get; set; }

        public Attachment ToModel()
        {
            Attachment attachment = new Attachment()
            {
                GUID = Guid.NewGuid().ToString(),
                Filename = this.filename,
                Size = this.size,
                MimeType = this.content_type
            };

            string path = "";
            string[] tmp = this.path.Split('/');
            for (int i = 0; i < tmp.Length - 1; i++)
            {
                path += tmp[i];
                if (i < tmp.Length - 2)
                    path += "/";
            }
            attachment.Path = path;

            tmp = this.download_url.Split('?');
            if (tmp.Length > 1)
                attachment.UrlParameters = tmp[1];

            return attachment;
        }
    }

    public class DeleteFileVM
    {
        public int file_id { get; set; }
        public string file_type { get; set; } //Constants.FileTypes.VIDEO_REQUEST_VIDEO
        public int? model_id { get; set; }
    }
}
