using Cameo.Common;
using Cameo.Models;

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
                this.url = AppData.Configuration.NoPhotoUrl;
                return;
            }

            this.id = model.ID;
            if (this.id > 0)
                this.url = model.Path + "/" + model.GUID + "." + model.Extension;
            else
                this.url = AppData.Configuration.NoPhotoUrl;
        }
    }

    public class UploadFileVM
    {
        public string Filename { get; set; } //"file.ext"
        public string Path { get; set; } //"path/to/file"
        public long Size { get; set; } //byte
        public string ContentType { get; set; } //"image/jpeg"
        public string DownloadUrl { get; set; } //"https://gerlgnenr.com?param1=val1&param2=val2..."

        public Attachment ToModel()
        {
            Attachment newAttachment = new Attachment();
            //newAttachment


            return newAttachment;
        }
    }
}
