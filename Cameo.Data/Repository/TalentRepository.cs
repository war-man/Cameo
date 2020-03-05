using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cameo.Data.Repository
{
    public class TalentRepository : BaseCRUDRepository<Talent>, ITalentRepository
    {
        public TalentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        override public IQueryable<Talent> GetWithRelatedDataAsIQueryable()
        {
            return DbSet
                .Include(m => m.TalentCategories).ThenInclude(m => m.Category)
                .Include(m => m.Projects)
                .Include(m => m.User)
                .Include(m => m.Avatar);
        }

        override public Talent GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return DbSet
                .Include(m => m.TalentCategories).ThenInclude(m => m.Category)
                .Include(m => m.Projects)
                .FirstOrDefault(m => m.ID == id && !m.IsDeleted);
        }
    }
}