using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class LogTalentPriceRepository : BaseCRUDRepository<LogTalentPrice>, ILogTalentPriceRepository
    {
        public LogTalentPriceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}