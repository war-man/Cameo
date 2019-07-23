using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System.IO;
using System.Linq;

namespace Cameo.Services
{
    public class AttachmentService : BaseCRUDService<Attachment>, IAttachmentService
    {
        private readonly IFileManagement FileManagement;
        public AttachmentService(IAttachmentRepository repository,
                           IUnitOfWork unitOfWork,
                           IFileManagement fileManagement)
            : base(repository, unitOfWork)
        {
            FileManagement = fileManagement;
        }

        //public void AddAndSaveFile(Attachment model, FileStream stream, string creatorID)
        //{
        //    //TO-DO: save file
        //    var bytes = FileManagement.ToByteArray(stream);
        //    string path = model.Path + "/" + model.GUID + "." + model.Extension;
        //    FileManagement.SaveFile(bytes, path);

        //    base.Add(model, creatorID);
        //}
    }
}