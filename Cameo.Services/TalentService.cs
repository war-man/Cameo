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

        public string GetRandomPhotoUrl()
        {
            List<string> urls = new List<string>()
            {
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/91398121_215459056226319_8001052095025359483_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=103&_nc_ohc=QpeHWpBfxQkAX-6jU-G&oh=6f16e7b8f15f7e75995bc7fcf868d260&oe=5F102AF3",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/103153847_750012662410713_6877097469987565377_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=109&_nc_ohc=Gne7ZskJj8oAX_2MT-B&oh=d4fcd145399d4ff1b2933fbe85a657a2&oe=5F0DBCA9",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/101262175_555147018708603_1746131759704050091_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=103&_nc_ohc=6fbPSck9fAYAX9s1N8w&oh=b83dfa5c9a309cb0c953db6dae4471c3&oe=5F0FAE68",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/90092291_116938873258760_8678810733341604697_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=108&_nc_ohc=3hOC9TinfxoAX9yupXL&oh=cf2e9aca779d1e649c6021f7ec66113d&oe=5F0E87F9",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/97295700_245561270085098_7286852456392262312_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=102&_nc_ohc=3LUqEZhWjgwAX9o3nBE&oh=bf893e87e6a88fb2fb8578c4f8617b83&oe=5F0CE9B1",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/102568125_711003819648614_2158210147199228765_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=102&_nc_ohc=xKqnIRshkz0AX9Xy0dp&oh=665e78d45ec0fe7fbaaaf24140f37fbe&oe=5F0E8FD2",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/96080393_1550101548498450_8529596848555704881_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=106&_nc_ohc=GHSU1mldvesAX9shnhz&oh=d700741e51725024ddb47dfef346c66b&oe=5F0D284A",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/100973892_547336785922438_8235285155480417369_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=110&_nc_ohc=zBSJXNOymyIAX9VHG6X&oh=a66f6e444815c462df774bbe992a9aa2&oe=5F0DBD68",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/s1080x1080/102388337_277091243479776_8527135892369297103_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=100&_nc_ohc=K1bZArl32EAAX8fRqsV&oh=757ad5ec99c2c710615417412f96ba58&oe=5F0DC969",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/s1080x1080/100671865_242458866982318_952037497740762328_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=110&_nc_ohc=4R3JXX959dwAX_Q0ssm&oh=f202f830fa81c2f3615f08201b45f99d&oe=5F0DB03F",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/100900977_1968437766623570_5399434634819320545_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=102&_nc_ohc=Pwk1-c_1UjkAX_jL5e1&oh=99269a2727e0e88a803a6a0db19ee934&oe=5F0FA385",
                "https://scontent-waw1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/98458357_106739267627088_9010794419742870590_n.jpg?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=100&_nc_ohc=gWujcUdqSocAX8zIFUY&oh=c590a788a30045362099828c2119ed30&oe=5F0F5659",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190903_231857/1629236964_medium.jpg",
                //"https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190712_234617/1645652980_medium.jpg",
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