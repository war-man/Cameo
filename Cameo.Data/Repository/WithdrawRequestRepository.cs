using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cameo.Data.Repository
{
    public class WithdrawRequestRepository : BaseCRUDRepository<WithdrawRequest>, IWithdrawRequestRepository
    {
        public WithdrawRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IQueryable<WithdrawRequest> GetManyWithRelatedDataAsIQueryable()
        {
            return DbSet
                .Include(m => m.Creator)
                .Include(m => m.Status);
        }

        public WithdrawRequest GeWithRelatedDataByID(int id)
        {
            return GetManyWithRelatedDataAsIQueryable()
                .FirstOrDefault(m => m.ID == id);
        }
    }
}