using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using NickBuhro.Translit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cameo.Services
{
    public class TalentSearchService : BaseCRUDService<Talent>, ITalentSearchService
    {
        public TalentSearchService(ITalentRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public IQueryable<Talent> GetFeatured(int? categoryID, int? count = null)
        {
            IQueryable<Talent> talents = GetWithRelatedDataForSearchAsIQueryable();

            talents = talents.Where(m => m.IsFeatured);

            if (categoryID.HasValue && categoryID > 0)
            {
                talents = talents.Where(m =>
                    m.TalentCategories
                        .Select(c => c.CategoryId)
                        .Contains(categoryID.Value));
            }

            if (count.HasValue && count > 0)
                talents = talents.Take(count.Value);

            return talents;
        }

        public IQueryable<Talent> GetNew(int? categoryID, int? count = null)
        {
            IQueryable<Talent> talents = GetWithRelatedDataForSearchAsIQueryable();

            talents = talents.Where(m => m.DateCreated >= DateTime.Now.AddDays(-360)); //must be set to -7

            if (categoryID.HasValue && categoryID > 0)
            {
                talents = talents.Where(m =>
                    m.TalentCategories
                        .Select(c => c.CategoryId)
                        .Contains(categoryID.Value));
            }

            if (count.HasValue && count > 0)
                talents = talents.Take(count.Value);

            return talents;
        }

        public IQueryable<Talent> GetNewInFeatured(int? count = null)
        {
            IQueryable<Talent> talents = GetFeatured(null);

            talents = talents.Where(m => m.DateCreated >= DateTime.Now.AddDays(-360)); //must be set to -7
            
            if (count.HasValue && count > 0)
                talents = talents.Take(count.Value);

            return talents;
        }

        public IQueryable<Talent> Search(int categoryID, SortTypeEnum sort, int? count = null)
        {
            IQueryable<Talent> talents = GetWithRelatedDataForSearchAsIQueryable();

            if (categoryID > 0)
            {
                talents = talents.Where(m =>
                    m.TalentCategories.Select(c => c.CategoryId).Contains(categoryID));
            }

            switch (sort)
            {
                case SortTypeEnum.priceAsc:
                    talents = talents.OrderBy(m => m.Price);
                    break;
                case SortTypeEnum.priceDesc:
                    talents = talents.OrderByDescending(m => m.Price);
                    break;
                case SortTypeEnum.az:
                    talents = talents.OrderBy(m => m.FirstName).ThenBy(m => m.LastName);
                    break;
                //this case will be uncommented 
                //when responseTime field will be filled after giving response to request
                //case SortTypeEnum.responseTime:
                //    result = result.OrderBy(m => m.FirstName).ThenBy(m => m.LastName);
                //    break;
                default: //def
                    talents = talents.OrderBy(m => m.ID);
                    break;
            }

            if (count.HasValue && count > 0)
                talents = talents.Take(count.Value);

            return talents;
        }

        public IQueryable<Talent> SearchBySearchText(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return Enumerable.Empty<Talent>().AsQueryable();

            searchText = searchText.ToLower();

            IQueryable<Talent> result = GetWithRelatedDataForSearchAsIQueryable();
            result = result
                .Where(m => !string.IsNullOrWhiteSpace(m.FullName) && m.FullName.ToLower().Contains(searchText)
                    || !string.IsNullOrWhiteSpace(m.FullNameTransliterated) && m.FullNameTransliterated.ToLower().Contains(searchText));

            return result;
        }

        private IQueryable<Talent> GetWithRelatedDataForSearchAsIQueryable()
        {
            //DEBUG
            return GetWithRelatedDataAsIQueryable();

            //RELEASE
            return GetWithRelatedDataAsIQueryable()
                .Where(m => m.IsAvailable
                    && m.User.TalentApprovedByAdmin
                    && m.AvatarID.HasValue && m.AvatarID > 0
                    && !m.IsDeleted
                    && m.Price > 0
                    && !(m.CreditCardNumber == null || m.CreditCardNumber.Trim() == string.Empty)
                    && m.CreditCardExpire.HasValue
                    && m.TalentCategories.Count > 0);
        }

        public IQueryable<Talent> GetRelated(Talent model, int? count = null)
        {
            var category = model.TalentCategories.FirstOrDefault();
            if (category == null)
                return Enumerable.Empty<Talent>().AsQueryable();

            var talents = Search(category.CategoryId, SortTypeEnum.def, count);
            talents = talents.Where(m => m.ID != model.ID);

            return talents;
        }
    }
}