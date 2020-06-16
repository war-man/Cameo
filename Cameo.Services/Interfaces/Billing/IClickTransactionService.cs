using Cameo.Models;

namespace Cameo.Services.Interfaces
{
    public interface IClickTransactionService : IBaseCRUDService<ClickTransaction>
    {
        void MarkTransactionAsCanceled(ClickTransaction transaction);
        void MarkTransactionAsPaid(ClickTransaction transaction);
        bool IsTransactionPaid(ClickTransaction transaction);
        bool IsTransactionCancelled(ClickTransaction transaction);
    }
}