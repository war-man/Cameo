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
}
