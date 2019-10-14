using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System.Linq;

namespace Cameo.Services
{
    public class CustomerService : BaseCRUDService<Customer>, ICustomerService
    {
        public CustomerService(ICustomerRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public Customer GetByUserID(string userID)
        {
            return GetAsIQueryable().FirstOrDefault(m => m.UserID == userID && !m.IsDeleted);
        }
    }
}