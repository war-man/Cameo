using Cameo.Models;

namespace Cameo.Services.Interfaces
{
    public interface ICustomerService : IBaseCRUDService<Customer>
    {
        Customer GetByUserID(string userID);
    }
}