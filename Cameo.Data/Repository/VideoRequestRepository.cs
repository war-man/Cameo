using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class VideoRequestRepository : BaseCRUDRepository<VideoRequest>, IVideoRequestRepository
    {
        public VideoRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}