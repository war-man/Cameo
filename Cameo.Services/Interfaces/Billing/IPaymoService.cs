using Cameo.Models;
using System.Threading.Tasks;

namespace Cameo.Services.Interfaces
{
    public interface IPaymoService
    {
        Task<string> ApplyForHold(Invoice invoice);
        Task ConfirmHold(Invoice invoice, string sms);
        Task PerformHold(Invoice invoice);
        Task CancelHold(Invoice invoice);
        //void MarkTransactionAsPaid(ClickTransaction transaction);
        //bool IsTransactionPaid(ClickTransaction transaction);
        //bool IsTransactionCancelled(ClickTransaction transaction);
    }
}