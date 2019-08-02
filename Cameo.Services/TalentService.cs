using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System.Collections.Generic;
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

        override public Talent GetByID(int id)
        {
            return GetWithRelatedDataAsIQueryable().FirstOrDefault(m => m.ID == id);
        }

        public Talent GetByUserID(string userID)
        {
            return GetWithRelatedDataAsIQueryable().FirstOrDefault(m => m.UserID == userID);
        }

        public Talent GetAvailableByID(int id)
        {
            var model = GetActiveByID(id);
            return model.IsAvailable ? model : null;
        }

        public IEnumerable<Talent> Search(int categoryID, SortTypeEnum sort)
        {
            IQueryable<Talent> result = GetWithRelatedDataForSearchAsIQueryable();

            if (categoryID > 0)
            {
                result = result.Where(m =>
                    m.TalentCategories.Select(c => c.CategoryId).Contains(categoryID));
            }

            switch (sort)
            {
                case SortTypeEnum.priceAsc:
                    result = result.OrderBy(m => m.Price);
                    break;
                case SortTypeEnum.priceDesc:
                    result = result.OrderByDescending(m => m.Price);
                    break;
                case SortTypeEnum.az:
                    result = result.OrderBy(m => m.FirstName).ThenBy(m => m.LastName);
                    break;
                //this case will be uncommented 
                //when responseTime field will be filled after giving response to request
                //case SortTypeEnum.responseTime:
                //    result = result.OrderBy(m => m.FirstName).ThenBy(m => m.LastName);
                //    break;
                default: //def
                    result = result.OrderBy(m => m.ID);
                    break;
            }

            return result;
        }

        private IQueryable<Talent> GetWithRelatedDataForSearchAsIQueryable()
        {
            return GetWithRelatedDataAsIQueryable()
                .Where(m => m.IsAvailable 
                /*&& m.IsConfirmed*/ 
                && !m.IsDeleted);
        }
    }
}