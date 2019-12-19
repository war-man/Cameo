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
        private readonly ILogTalentPriceService LogTalentPriceService;

        public TalentService(ITalentRepository repository,
                           IUnitOfWork unitOfWork,
                           ILogTalentPriceService logTalentPriceService)
            : base(repository, unitOfWork)
        {
            LogTalentPriceService = logTalentPriceService;
        }

        public Talent GetActiveByUsername(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
                username = username.ToLower();

            Talent model = _repository.GetWithRelatedDataAsIQueryable()
                .FirstOrDefault(m => m.User.UserName.ToLower() == username);
            if (model != null)
                return model.IsDeleted ? null : model;

            return model;
        }

        public override void Update(Talent entity, string userID)
        {
            base.Update(entity, userID);

            LogPrice(entity, userID);
        }

        private void LogPrice(Talent model, string userID)
        {
            LogTalentPrice log = new LogTalentPrice();
            log.TalentID = model.ID;
            log.Price = model.Price;
            LogTalentPriceService.Add(log, userID);
        }

        override public Talent GetByID(int id)
        {
            return GetWithRelatedDataAsIQueryable().FirstOrDefault(m => m.ID == id);
        }

        public Talent GetByUserID(string userID)
        {
            return GetWithRelatedDataAsIQueryable().FirstOrDefault(m => m.UserID == userID && !m.IsDeleted);
        }

        public Talent GetAvailableByID(int id)
        {
            var model = GetActiveByID(id);
            return model.IsAvailable ? model : null;
        }

        public Talent GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return _repository.GetActiveSingleDetailsWithRelatedDataByID(id);
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

        public IEnumerable<Talent> SearchBySearchText(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return Enumerable.Empty<Talent>();

            searchText = searchText.ToLower();

            IQueryable<Talent> result = GetWithRelatedDataForSearchAsIQueryable();
            result = result.Where(m => 
                !string.IsNullOrWhiteSpace(m.FirstName) && m.FirstName.ToLower().Contains(searchText) 
                || !string.IsNullOrWhiteSpace(m.LastName) && m.LastName.ToLower().Contains(searchText));

            return result;
        }

        private IQueryable<Talent> GetWithRelatedDataForSearchAsIQueryable()
        {
            return GetWithRelatedDataAsIQueryable()
                .Where(m => m.IsAvailable
                    /*&& m.IsConfirmed*/
                    && !m.IsDeleted);
                    //&& m.CreditCardExpire);
        }

        public IEnumerable<Talent> GetRelated(Talent model)
        {
            List<int> categories = model.TalentCategories
                .Select(m => m.CategoryId)
                .ToList();

            List<List<int>> variants = GenerateVariants(categories);

            return null;
        }

        private List<List<int>> GenerateVariants(List<int> ints)
        {
            for (int i = 1; i < ints.Count; i++)
            {
                while (NextSet(ints, ints.Count, i))
                {

                }
            }

            return null;
        }

        bool NextSet(List<int> a, int n, int m)
        {
            int k = m;
            for (int i = k - 1; i >= 0; --i)
                if (a[i] < n - k + i + 1)
                {
                    ++a[i];
                    for (int j = i + 1; j < k; ++j)
                        a[j] = a[j - 1] + 1;
                    return true;
                }
            return false;
        }
    }
}