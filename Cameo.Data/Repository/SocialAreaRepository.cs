using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class SocialAreaRepository : BaseCRUDRepository<SocialArea>, ISocialAreaRepository
    {
        public SocialAreaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}