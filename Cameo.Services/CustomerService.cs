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

        public override void Add(Customer entity, string userID)
        {
            base.Add(entity, userID);

            AssignAccountNumber(entity);
            base.Update(entity, userID);
        }

        public Customer GetByUserID(string userID)
        {
            return GetAsIQueryable().FirstOrDefault(m => m.UserID == userID && !m.IsDeleted);
        }

        public Customer GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return _repository.GetActiveSingleDetailsWithRelatedDataByID(id);
        }

        public Customer GetActiveSingleDetailsWithRelatedDataByUserID(string userID)
        {
            return _repository.GetActiveSingleDetailsWithRelatedDataByUserID(userID);
        }

        private void AssignAccountNumber(Customer model)
        {
            if (string.IsNullOrWhiteSpace(model.AccountNumber))
                model.AccountNumber = model.ID.ToString().PadLeft(8, '0');
        }

        public Customer GetByAccountNumber(string accountNumber)
        {
            return GetAsIQueryable().FirstOrDefault(m => m.AccountNumber.Equals(accountNumber));
        }
    }
}