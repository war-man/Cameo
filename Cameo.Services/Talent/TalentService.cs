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
            TransliterateFullname(entity);

            base.Add(entity, userID);

            AssignAccountNumber(entity);
            base.Update(entity, userID);
        }

        public void TransliterateFullname(Talent talent)
        {
            string fullNameTransliterated = null;

            if (Regex.IsMatch(talent.FullName, @"\p{IsCyrillic}"))
            {
                // there is at least one cyrillic character in the string
                fullNameTransliterated = Transliteration.CyrillicToLatin(talent.FullName, Language.Russian);
            }
            else
            {
                fullNameTransliterated = Transliteration.LatinToCyrillic(talent.FullName, Language.Russian);
            }

            talent.FullNameTransliterated = fullNameTransliterated;
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

        //public IQueryable<Talent> GetFeatured(int? categoryID, int? count = null)
        //{
        //    IQueryable<Talent> talents = GetWithRelatedDataForSearchAsIQueryable();

        //    talents = talents.Where(m => m.IsFeatured);

        //    if (categoryID.HasValue && categoryID > 0)
        //    {
        //        talents = talents.Where(m =>
        //            m.TalentCategories
        //                .Select(c => c.CategoryId)
        //                .Contains(categoryID.Value));
        //    }

        //    if (count.HasValue && count > 0)
        //        talents = talents.Take(count.Value);

        //    return talents;
        //}

        //public IQueryable<Talent> GetNew(int? categoryID, int? count = null)
        //{
        //    IQueryable<Talent> talents = GetWithRelatedDataForSearchAsIQueryable();

        //    talents = talents.Where(m => m.DateCreated >= DateTime.Now.AddDays(-360)); //must be set to -7

        //    if (categoryID.HasValue && categoryID > 0)
        //    {
        //        talents = talents.Where(m =>
        //            m.TalentCategories
        //                .Select(c => c.CategoryId)
        //                .Contains(categoryID.Value));
        //    }

        //    if (count.HasValue && count > 0)
        //        talents = talents.Take(count.Value);

        //    return talents;
        //}

        //public IQueryable<Talent> GetNewInFeatured(int? count = null)
        //{
        //    IQueryable<Talent> talents = GetFeatured(null);

        //    talents = talents.Where(m => m.DateCreated >= DateTime.Now.AddDays(-360)); //must be set to -7
            
        //    if (count.HasValue && count > 0)
        //        talents = talents.Take(count.Value);

        //    return talents;
        //}

        //public IQueryable<Talent> Search(int categoryID, SortTypeEnum sort, int? count = null)
        //{
        //    IQueryable<Talent> talents = GetWithRelatedDataForSearchAsIQueryable();

        //    if (categoryID > 0)
        //    {
        //        talents = talents.Where(m =>
        //            m.TalentCategories.Select(c => c.CategoryId).Contains(categoryID));
        //    }

        //    switch (sort)
        //    {
        //        case SortTypeEnum.priceAsc:
        //            talents = talents.OrderBy(m => m.Price);
        //            break;
        //        case SortTypeEnum.priceDesc:
        //            talents = talents.OrderByDescending(m => m.Price);
        //            break;
        //        case SortTypeEnum.az:
        //            talents = talents.OrderBy(m => m.FirstName).ThenBy(m => m.LastName);
        //            break;
        //        //this case will be uncommented 
        //        //when responseTime field will be filled after giving response to request
        //        //case SortTypeEnum.responseTime:
        //        //    result = result.OrderBy(m => m.FirstName).ThenBy(m => m.LastName);
        //        //    break;
        //        default: //def
        //            talents = talents.OrderBy(m => m.ID);
        //            break;
        //    }

        //    if (count.HasValue && count > 0)
        //        talents = talents.Take(count.Value);

        //    return talents;
        //}

        //public IQueryable<Talent> SearchBySearchText(string searchText)
        //{
        //    if (string.IsNullOrWhiteSpace(searchText))
        //        return Enumerable.Empty<Talent>().AsQueryable();

        //    searchText = searchText.ToLower();

        //    IQueryable<Talent> result = GetWithRelatedDataForSearchAsIQueryable();
        //    result = result.Where(m => !string.IsNullOrWhiteSpace(m.FullName) && m.FullName.ToLower().Contains(searchText));

        //    return result;
        //}

        public void SetAvailability(Talent model, bool availability, string userID)
        {
            model.IsAvailable = availability;
            base.Update(model, userID);
        }

        //private IQueryable<Talent> GetWithRelatedDataForSearchAsIQueryable()
        //{
        //    //DEBUG
        //    return GetWithRelatedDataAsIQueryable();

        //    //RELEASE
        //    return GetWithRelatedDataAsIQueryable()
        //        .Where(m => m.IsAvailable
        //            && m.User.TalentApprovedByAdmin
        //            && m.AvatarID.HasValue && m.AvatarID > 0
        //            && !m.IsDeleted
        //            && m.Price > 0
        //            && !(m.CreditCardNumber == null || m.CreditCardNumber.Trim() == string.Empty)
        //            && m.CreditCardExpire.HasValue
        //            && m.TalentCategories.Count > 0);
        //}

        //public IQueryable<Talent> GetRelated(Talent model, int? count = null)
        //{
        //    var category = model.TalentCategories.FirstOrDefault();
        //    if (category == null)
        //        return Enumerable.Empty<Talent>().AsQueryable();

        //    var talents = Search(category.CategoryId, SortTypeEnum.def, count);
        //    talents = talents.Where(m => m.ID != model.ID);

        //    return talents;
        //}

        public void SaveDetachedIntroVideo(Talent model, string userID)
        {
            Update(model, userID);
        }

        public string GetRandomPhotoUrl()
        {
            List<string> urls = new List<string>()
            {
                "https://freepikpsd.com/wp-content/uploads/2019/09/Joker-Joaquin-Phoenix-iPhone-Wallpaper-Free.jpg",
                "https://freepikpsd.com/wp-content/uploads/2019/09/Cyberpunk-Helmet-Closed-Eyes-iPhone-Wallpaper-Free.jpg",
                "https://freepikpsd.com/wp-content/uploads/2019/09/The-Witcher-iPhone-Wallpaper-Free.jpg",
                "https://freepikpsd.com/wp-content/uploads/2019/09/Asuka-Langley-Soryu-iPhone-Wallpaper-Free.jpg",
                "https://freepikpsd.com/wp-content/uploads/2019/09/I-am-Iron-Man-Snap-iPhone-Wallpaper-Free.jpg",
                "https://freepikpsd.com/wp-content/uploads/2019/09/Logan-vs-Hulk-iPhone-Wallpaper-Free.jpg",
                "https://freepikpsd.com/wp-content/uploads/2020/04/carcajou-png-1-Images-Free-Transparent-683x1024.jpg",
                "https://freepikpsd.com/wp-content/uploads/2019/09/The-Girl-With-The-Owl-Tattoo-iPhone-Wallpaper-Free.jpg",
                "https://i.pinimg.com/originals/0d/78/34/0d7834cf970ce238f704a86f7d42a0f0.jpg",
                "https://i.pinimg.com/originals/fc/97/0f/fc970f26248d2aad2937f5bce7706a85.jpg",
                "https://i.pinimg.com/236x/84/82/3b/84823b6dceb40fca6e17b0ff1d56afb2--design-poster-poster-print.jpg",
                "https://i.pinimg.com/236x/9f/64/a6/9f64a65d90276416dd350fa0feff41ec--magazine-cover-design-magazine-covers.jpg",
                "https://i.pinimg.com/236x/37/0a/15/370a155b8c9e4bc0ca23aede15c1b6a4.jpg",
                "https://i.pinimg.com/236x/b1/8c/dd/b18cddd5ee5a3ea39c611a13488a216a.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181103_151515/1246139023_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190321_090716/1260185104_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181230_125854/1303631832_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190330_170418/1338675767_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181104_121932/1420110925_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190704_214009/1446278688_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191120_082123/1465143706_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181104_121932/1420110925_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191022_141609/1489216552_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20170415_163011/1498801167_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190928_125853/1506653934_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181230_125854/1656540307_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190903_231857/1663342103_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191022_122027/1751005141_medium.jpg"
            };

            Random random = new Random();
            int randomIndex = random.Next(0, urls.Count);
            return urls[randomIndex];
        }
    }
}