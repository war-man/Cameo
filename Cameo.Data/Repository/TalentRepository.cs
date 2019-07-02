using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class TalentRepository : BaseCRUDRepository<Talent>, ITalentRepository
    {
        public TalentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}