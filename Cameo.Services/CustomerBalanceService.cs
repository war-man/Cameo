using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cameo.Services
{
    public class CustomerBalanceService : /*BaseCRUDService<Talent>,*/ ICustomerBalanceService
    {
        private readonly ICustomerService CustomerService;

        public CustomerBalanceService(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        //public int GetBalance(Customer customer)
        //{
        //    return customer?.Balance ?? 0;
        //}

        //public void ReplenishBalance(Customer customer, int amount)
        //{
        //    customer.Balance += amount;
        //}

        //public void TakeOffBalance(Customer customer, int amount)
        //{
        //    customer.Balance -= amount;
        //}

        //public string GenerateClickPaymentButtonUrl(string accountNumber, int amount, string returnUrl)
        //{
        //    if (string.IsNullOrWhiteSpace(accountNumber))
        //        throw new Exception("accountNumber is not provided");

        //    if (amount <= 0)
        //        throw new Exception("amount must be positive integer");

        //    string url = @"https://my.click.uz/services/pay";
        //    url += "?service_id=" + Constants.CLICK.SETTINGS.SERVICE_id;
        //    url += "&merchant_id=" + Constants.CLICK.SETTINGS.MERCHANT_ID;
        //    url += "&amount=" + amount + ".00";
        //    url += "&transaction_param=" + accountNumber;

        //    if (!string.IsNullOrWhiteSpace(returnUrl))
        //        url += "&return_url=" + returnUrl;

        //    return url;
        //}
    }
}
