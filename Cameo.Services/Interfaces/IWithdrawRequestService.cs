using Cameo.Models;
using Cameo.Models.Enums;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface IWithdrawRequestService : IBaseCRUDService<WithdrawRequest>
    {
        void Add(WithdrawRequest entity, Talent talent);
        IQueryable<WithdrawRequest> Search(
            int? start,
            int? length,

            out int recordsTotal,
            out int recordsFiltered,
            out string error,

            int? statusID,
            int? talentID);
        bool UserHasNotEnoughtMoneyForWithdrawal(int balance);
        bool AmountIsLessThanMinimum(int amount);
        int GetMinimalAmountInBalanceForWithdrawal();
        bool IsCompleted(WithdrawRequest model);
        void MarkAsCompleted(WithdrawRequest model, string adminID);
        IQueryable<WithdrawRequest> GetAllByTalent(int talentID);
    }
}