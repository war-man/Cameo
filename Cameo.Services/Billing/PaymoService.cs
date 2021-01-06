using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;

namespace Cameo.Services
{
    public class PaymoService : IPaymoService
    {
        private readonly IInvoiceService InvoiceService;

        public PaymoService(IInvoiceService invoiceService)
        {
            InvoiceService = invoiceService;
        }

        //POST: https://api.paymo.uz/hold/
        public string ApplyForHold(Invoice invoice)
        {
            //make https request
            return "123456";
        }

        //PUT: https://api.paymo.uz/hold/{hold_id}
        public void ConfirmHold(Invoice invoice, string sms)
        {
            if (string.IsNullOrWhiteSpace(invoice?.hold_id))
                throw new Exception("hold_id не присвоен");

            //make https request
            //make checkings
            //fields in response must be assigned to invoice
        }

        //POST: https://api.pays.uz/hold/{hold_id}
        public void PerformHold(Invoice invoice)
        {
            if (string.IsNullOrWhiteSpace(invoice?.hold_id))
                throw new Exception("hold_id не присвоен");

            //make https request
            //make checkings

            InvoiceService.MarkAsSuccess(invoice);
        }

        //DELETE: https://api.pays.uz/hold/{hold_id}
        public void CancelHold(Invoice invoice)
        {
            if (string.IsNullOrWhiteSpace(invoice?.hold_id))
                throw new Exception("hold_id не присвоен");

            //make https request
            //make checkings

            InvoiceService.MarkAsCanceled(invoice);
        }
    }
}