using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System.Linq;

namespace Cameo.Services
{
    public class AttachmentService : BaseCRUDService<Attachment>, IAttachmentService
    {
        public AttachmentService(IAttachmentRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            
        }
    }
}