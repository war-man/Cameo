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
                .Include(m => m.TalentCategories)
                .Include(m => m.Projects);
        }
    }
}