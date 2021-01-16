using Cameo.Models;

namespace Cameo.Services.Interfaces
{
    public interface IInvoiceService : IBaseCRUDService<Invoice>
    {
        void AssignHoldID(Invoice entity, string hold_id, string creatorID);
        void MarkAsSuccess(Invoice entity);
        void MarkAsCanceled(Invoice entity);
        void MarkAsFailedToWithdrawMoney(Invoice entity);
        //void MarkTransactionAsCanceled(ClickTransaction transaction);
        //void MarkTransactionAsPaid(ClickTransaction transaction);
        //bool IsTransactionPaid(ClickTransaction transaction);
        //bool IsTransactionCancelled(ClickTransaction transaction);
    }
}