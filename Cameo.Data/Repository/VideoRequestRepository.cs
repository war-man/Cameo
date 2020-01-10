using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Data.Repository
{
    public class VideoRequestRepository : BaseCRUDRepository<VideoRequest>, IVideoRequestRepository
    {
        public VideoRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IQueryable<VideoRequest> GetRequestsByTalent(Talent talent)
        {
            return GetAsIQueryable().Where(m => m.TalentID == talent.ID);
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
                .Include(m => m.Talent).ThenInclude(m => m.Projects)
                .Include(m => m.Talent).ThenInclude(m => m.User)
                .Include(m => m.Video)
                .FirstOrDefault(m => m.ID == id && !m.IsDeleted);
        }
    }
}