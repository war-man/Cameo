using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class AttachmentRepository : BaseCRUDRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}