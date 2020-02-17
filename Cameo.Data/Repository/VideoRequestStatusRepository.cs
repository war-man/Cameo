using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class VideoRequestStatusRepository : BaseCRUDRepository<VideoRequestStatus>, IVideoRequestStatusRepository
    {
        public VideoRequestStatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}