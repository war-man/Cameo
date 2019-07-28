using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System.Linq;

namespace Cameo.Services
{
    public class TalentService : BaseCRUDService<Talent>, ITalentService
    {
        public TalentService(ITalentRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public Talent GetByUserID(string userID)
        {
            return GetAsIQueryable().FirstOrDefault(m => m.UserID == userID);
        }

        public Talent GetAvailableByID(int id)
        {
            var model = GetActiveByID(id);
            return model.IsAvailable ? model : null;
        }
    }
}