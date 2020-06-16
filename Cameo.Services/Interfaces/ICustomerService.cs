using Cameo.Models;

namespace Cameo.Services.Interfaces
{
    public interface ICustomerService : IBaseCRUDService<Customer>
    {
        Customer GetByUserID(string userID);
        Customer GetActiveSingleDetailsWithRelatedDataByID(int id);
        Customer GetActiveSingleDetailsWithRelatedDataByUserID(string userID);
        Customer GetByAccountNumber(string accountNumber);
    }
}