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

        public Invoice ToModel()
        {
            Invoice invoice = new Invoice()
            {
                
            };

            return invoice;
        }
    }
}
