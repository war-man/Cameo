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

            invoice.StatusID = 2; //success; must be refactored to enum
        }

        //DELETE: https://api.pays.uz/hold/{hold_id}
        public void CancelHold(Invoice invoice)
        {
            if (string.IsNullOrWhiteSpace(invoice?.hold_id))
                throw new Exception("hold_id не присвоен");

            //make https request
            //make checkings

            invoice.StatusID = 3; //cancelled; must be refactored to enum
        }
    }
}