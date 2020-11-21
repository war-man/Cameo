using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class InvoiceGenerateVM
    {
        public int TalentID { get; set; }
        public string card_number { get; set; }
        public string card_expiry { get; set; }

        public Invoice ToModel(int price)
        {
            Invoice invoice = new Invoice()
            {
                Amount = price,
                card_number = card_number,
            };

            if (!string.IsNullOrWhiteSpace(card_expiry))
            {
                string[] tmp = card_expiry.Split('/');

                string monthString = tmp[0];
                string yearString = tmp[1];

                int month = int.Parse(monthString);
                int year = int.Parse(yearString) + 2000;

                DateTime creditCardExpireTmp = new DateTime(year, month, 1); //day does not play any role
                invoice.card_expiry = creditCardExpireTmp;
            }

            return invoice;
        }
    }
}
