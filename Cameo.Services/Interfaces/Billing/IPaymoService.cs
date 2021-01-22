using Cameo.Models;
using System.Threading.Tasks;

namespace Cameo.Services.Interfaces
{
    public interface IPaymoService
    {
        string ApplyForHold(Invoice invoice);
        void ConfirmHold(Invoice invoice, string sms);
        void PerformHold(Invoice invoice);
        void CancelHold(Invoice invoice);
        //void MarkTransactionAsPaid(ClickTransaction transaction);
        //bool IsTransactionPaid(ClickTransaction transaction);
        //bool IsTransactionCancelled(ClickTransaction transaction);
    }
}