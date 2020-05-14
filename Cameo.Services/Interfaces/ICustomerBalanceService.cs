using Cameo.Models;
using Cameo.Models.Enums;
using System.Collections.Generic;

namespace Cameo.Services.Interfaces
{
    public interface ICustomerBalanceService// : IBaseCRUDService<Talent>
    {
        int GetBalance(Customer customer);
        void ReplenishBalance(Customer customer, int amount);
        void TakeOffBalance(Customer customer, int amount);
    }
}