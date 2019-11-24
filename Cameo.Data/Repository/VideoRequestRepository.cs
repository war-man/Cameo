using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cameo.Data.Repository
{
    public class VideoRequestRepository : BaseCRUDRepository<VideoRequest>, IVideoRequestRepository
    {
        public VideoRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        override public IQueryable<VideoRequest> GetWithRelatedDataAsIQueryable()
        {
            return DbSet
                .Include(m => m.Customer).ThenInclude(m => m.Avatar)
                .Include(m => m.Talent).ThenInclude(m => m.Avatar)
                .Include(m => m.Type)
                .Include(m => m.Video)
                .Include(m => m.RequestStatus);
        }

        override public VideoRequest GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return DbSet
                .Include(m => m.Customer).ThenInclude(m => m.Avatar)
                .Include(m => m.Talent).ThenInclude(m => m.Avatar)
                .Include(m => m.Video)
                .FirstOrDefault(m => m.ID == id && !m.IsDeleted);
        }
    }
}