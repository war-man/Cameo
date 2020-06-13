using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class TalentStatisticsVM
    {
        public string Total { get; set; }
        public string NotCompleted { get; set; }
        public string WaitingForAnswer { get; set; }
        public string WaitingForVideo { get; set; }
        public string WaitingForPayment { get; set; }
        public string WaitingForPaymentConfirmation { get; set; }
        public string PaymentConfirmed { get; set; }
        public string Earned { get; set; }
    }
}
