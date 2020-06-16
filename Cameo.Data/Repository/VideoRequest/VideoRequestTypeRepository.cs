using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class VideoRequestTypeRepository : BaseCRUDRepository<VideoRequestType>, IVideoRequestTypeRepository
    {
        public VideoRequestTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}