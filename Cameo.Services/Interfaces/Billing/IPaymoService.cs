using Cameo.Models;

namespace Cameo.Services.Interfaces
{
    public interface IPaymoService
    {
        string ApplyForHold(Invoice invoice);
        //void MarkTransactionAsPaid(ClickTransaction transaction);
        //bool IsTransactionPaid(ClickTransaction transaction);
        //bool IsTransactionCancelled(ClickTransaction transaction);
    }
}