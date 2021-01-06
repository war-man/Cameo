using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class WithdrawRequestStatusRepository : BaseCRUDRepository<WithdrawRequestStatus>, IWithdrawRequestStatusRepository
    {
        public WithdrawRequestStatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}