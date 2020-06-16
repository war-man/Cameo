using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class ClickTransactionRepository : BaseCRUDRepository<ClickTransaction>, IClickTransactionRepository
    {
        public ClickTransactionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}