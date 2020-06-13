using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;
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

        //public override void GetActiveByID(int id)
        //{
        //    Talent model = base.GetActiveByID(id);
        //    if (model.us)

        //    AssignAccountNumber(entity);
        //    base.Update(entity, userID);
        //}

        public Talent GetActiveByUsername(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
                username = username.ToLower();

            Talent model = _repository.GetWithRelatedDataAsIQueryable()
                .FirstOrDefault(m => m.User.TalentApprovedByAdmin 
                    && m.User.UserName.ToLower() == username);

            if (model != null)
                return model.IsDeleted ? null : model;

            return model;
        }

        public override void Add(Talent entity, string userID)
        {
            base.Add(entity, userID);

            AssignAccountNumber(entity);
            base.Update(entity, userID);
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

        public void AssignAccountNumber(Talent model)
        {
            if (string.IsNullOrWhiteSpace(model.AccountNumber))
                model.AccountNumber = model.ID.ToString().PadLeft(8, '0');
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
            result = result.Where(m => 
                !string.IsNullOrWhiteSpace(m.FirstName) && m.FirstName.ToLower().Contains(searchText) 
                || !string.IsNullOrWhiteSpace(m.LastName) && m.LastName.ToLower().Contains(searchText)
                || !string.IsNullOrWhiteSpace(m.FullName) && m.FullName.ToLower().Contains(searchText));

            return result;
        }

        public void SetAvailability(Talent model, bool availability, string userID)
        {
            model.IsAvailable = availability;
            base.Update(model, userID);
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

        public void SaveDetachedIntroVideo(Talent model, string userID)
        {
            Update(model, userID);
        }
    }
}