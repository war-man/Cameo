using Cameo.Common;
using Cameo.Models;

namespace Cameo.AdminPanel.ViewModels
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
                //this.Url = AppData.Configuration.NoPhotoUrl;
                this.Url = "/Content/Images/nophoto.png";
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
}
