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

        public int GetBalance(Customer customer)
        {
            return customer?.Balance ?? 0;
        }

        public void ReplenishBalance(Customer customer, int amount)
        {
            customer.Balance += amount;
        }

        public void TakeOffBalance(Customer customer, int amount)
        {
            customer.Balance -= amount;
        }
    }
}
