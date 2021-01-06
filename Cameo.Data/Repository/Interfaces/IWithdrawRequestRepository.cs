using Cameo.Models;
using System.Linq;

namespace Cameo.Data.Repository.Interfaces
{
    public interface IWithdrawRequestRepository : IBaseCRUDRepository<WithdrawRequest>
    {
        IQueryable<WithdrawRequest> GetManyWithRelatedDataAsIQueryable();
        WithdrawRequest GeWithRelatedDataByID(int id);
    }
}