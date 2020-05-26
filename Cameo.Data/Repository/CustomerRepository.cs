using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cameo.Data.Repository
{
    public class CustomerRepository : BaseCRUDRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        override public Customer GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return DbSet
                .Include(m => m.User)
                .FirstOrDefault(m => m.ID == id && !m.IsDeleted);
        }

        override public Customer GetActiveSingleDetailsWithRelatedDataByUserID(string userID)
        {
            return DbSet
                .Include(m => m.User)
                .FirstOrDefault(m => m.UserID == userID && !m.IsDeleted);
        }
    }
}