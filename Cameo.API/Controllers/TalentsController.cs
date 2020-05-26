using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalentsController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ICategoryService CategoryService;
        private readonly IVideoRequestTypeService VideoRequestTypeService;
        private readonly IVideoRequestService VideoRequestService;

        public TalentsController(
            ITalentService talentService,
            ICategoryService categoryService,
            IVideoRequestTypeService videoRequestTypeService,
            IVideoRequestService videoRequestService)
        {
            TalentService = talentService;
            CategoryService = categoryService;
            VideoRequestTypeService = videoRequestTypeService;
            VideoRequestService = videoRequestService;
        }

        [HttpGet("GetCategorized")]
        public ActionResult<IEnumerable<TalentsCategorizedVM>> GetCategorized()
        {
            List<TalentsCategorizedVM> talentsCategorizedVM = new List<TalentsCategorizedVM>();
            List<Category> categories = new List<Category>();

            var featuredTalents = TalentService.GetFeatured(null, 6);
            if (featuredTalents.Count() > 0)
            {
                var featuredTalentsVM = new TalentsCategorizedVM(
                    new Category()
                    {
                        ID = (int)CategoryEnum.featured,
                        Name = "Популярные"
                    },
                    featuredTalents.ToList());
                talentsCategorizedVM.Add(featuredTalentsVM);
            }

            var newTalents = TalentService.GetNew(null, 6);
            if (newTalents.Count() > 0)
            {
                var newTalentsVM = new TalentsCategorizedVM(
                    new Category()
                    {
                        ID = (int)CategoryEnum.neW,
                        Name = "Новые"
                    },
                    newTalents.ToList());
                talentsCategorizedVM.Add(newTalentsVM);
            }

            categories.AddRange(CategoryService.GetAllActive());
            foreach (var category in categories)
            {
                var talentsCategorized = TalentService.Search(category.ID, SortTypeEnum.def, 6);
                if (talentsCategorized.Count() > 0)
                {
                    var categoryTalentsVM = new TalentsCategorizedVM(category, talentsCategorized.ToList());

                    talentsCategorizedVM.Add(categoryTalentsVM);
                }
            }

            foreach (var categoryTalentsVM in talentsCategorizedVM)
            {
                foreach (var talent in categoryTalentsVM.talents)
                {
                    if (talent.avatar.id == 0)
                        talent.avatar.url = GetRandomPhotoUrl();
                }
            }

            return talentsCategorizedVM;
        }

        [HttpGet("GetByCategory")]
        public ActionResult<IEnumerable<TalentsCategorizedVM>> GetByCategory(int category_id)
        {
            //1. if category = new then
            //  - all in new
            //2. if category = featured then:
            //  - featured
            //  - new in featured
            //  - all in featured
            //3. else (if normal category)
            //  - featured in category
            //  - new in category
            //  - all in category

            List<TalentsCategorizedVM> talentsCategorizedVM = new List<TalentsCategorizedVM>();

            if (category_id == (int)CategoryEnum.neW)
            {
                var newTalents = TalentService.GetNew(null);
                if (newTalents.Count() > 0)
                {
                    var newTalentsVM = new TalentsCategorizedVM(
                        new Category()
                        {
                            ID = (int)CategoryEnum.neW,
                            Name = "Все новые"
                        },
                        newTalents.ToList());
                    talentsCategorizedVM.Add(newTalentsVM);
                }
            }
            else if (category_id == (int)CategoryEnum.featured)
            {
                //DO NOT DELETE THIS CODE!!!
                //must be uncommented later when number of telants will be large

                //var featuredTalents = TalentService.GetFeatured(null, 6);
                //if (featuredTalents.Count() > 0)
                //{
                //    var featuredTalentsVM = new TalentsCategorizedVM(
                //        new Category()
                //        {
                //            ID = (int)CategoryEnum.featured,
                //            Name = "Популярные"
                //        },
                //        featuredTalents.ToList());
                //    talentsCategorizedVM.Add(featuredTalentsVM);
                //}

                //var newTalents = TalentService.GetNewInFeatured(6);
                //if (newTalents.Count() > 0)
                //{
                //    var newTalentsVM = new TalentsCategorizedVM(
                //        new Category()
                //        {
                //            ID = (int)CategoryEnum.neW,
                //            Name = "Новые"
                //        },
                //        newTalents.ToList());
                //    talentsCategorizedVM.Add(newTalentsVM);
                //}

                var allTalents = TalentService.GetFeatured(null);
                if (allTalents.Count() > 0)
                {
                    var allTalentsVM = new TalentsCategorizedVM(
                        new Category()
                        {
                            ID = (int)CategoryEnum.neW,
                            Name = "Все Популярные"
                        },
                        allTalents.ToList());
                    talentsCategorizedVM.Add(allTalentsVM);
                }
            }
            else
            {
                var categoryDB = CategoryService.GetActiveByID(category_id);
                if (categoryDB == null)
                    return NotFound("Category not found");

                //DO NOT DELETE THIS CODE!!!
                //must be uncommented later when number of telants will be large

                //var featuredTalents = TalentService.GetFeatured(categoryID, 6);
                //if (featuredTalents.Count() > 0)
                //{
                //    var featuredTalentsVM = new TalentsCategorizedVM(
                //        new Category()
                //        {
                //            ID = (int)CategoryEnum.featured,
                //            Name = "Популярные"
                //        },
                //        featuredTalents.ToList());
                //    talentsCategorizedVM.Add(featuredTalentsVM);
                //}

                //var newTalents = TalentService.GetNew(categoryID, 6);
                //if (newTalents.Count() > 0)
                //{
                //    var newTalentsVM = new TalentsCategorizedVM(
                //        new Category()
                //        {
                //            ID = (int)CategoryEnum.neW,
                //            Name = "Новые"
                //        },
                //        newTalents.ToList());
                //    talentsCategorizedVM.Add(newTalentsVM);
                //}

                var allTalents = TalentService.Search(category_id, SortTypeEnum.def);
                if (allTalents.Count() > 0)
                {
                    var allTalentsVM = new TalentsCategorizedVM(
                        new Category()
                        {
                            ID = (int)CategoryEnum.neW,
                            Name = "Все"
                        },
                        allTalents.ToList());
                    talentsCategorizedVM.Add(allTalentsVM);
                }
            }

            foreach (var categoryTalentsVM in talentsCategorizedVM)
            {
                foreach (var talent in categoryTalentsVM.talents)
                {
                    if (talent.avatar.id == 0)
                        talent.avatar.url = GetRandomPhotoUrl();
                }
            }

            return talentsCategorizedVM;
        }

        [HttpGet("GetBySearchText")]
        public ActionResult<IEnumerable<TalentShortInfoVM>> GetBySearchText(string search_text)
        {
            var talents = TalentService.SearchBySearchText(search_text)
                .Select(m => new TalentShortInfoVM(m))
                .ToList();

            foreach (var item in talents)
            {
                if (item.avatar.id == 0)
                    item.avatar.url = GetRandomPhotoUrl();
            }

            return talents;
        }

        [HttpGet("{id}")]
        public ActionResult<TalentDetailsVM> Get(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetActiveByID(id);
            if (model == null)
                return NotFound();

            TalentDetailsVM modelVM = new TalentDetailsVM(model);

            //VideoRequest videoRequest = VideoRequestService.GetRandomSinglePublishedByTalent(model, curUser.ID);
            //if (videoRequest != null)
            //{
            //    modelVM.Video = new AttachmentDetailsVM(videoRequest.Video);
            //    modelVM.RequestID = videoRequest.ID;
            //}

            return modelVM;
        }

        [HttpGet("GetLatestVideosForTalent/{id}")]
        public ActionResult<IEnumerable<VideoDetailsVM>> GetLatestVideosForTalent(int id)
        {
            Talent talent = TalentService.GetActiveByID(id);
            if (talent == null)
                return CustomBadRequest("Талант не найден");

            List<VideoRequest> videos = VideoRequestService.GetPublicForTalent(talent, 0)
                .ToList();

            List<VideoDetailsVM> videosVM = new List<VideoDetailsVM>();
            foreach (var item in videos)
            {
                videosVM.Add(new VideoDetailsVM(item));
            }

            return videosVM;
        }

        [HttpGet("GetRelated/{id}")]
        public ActionResult<IEnumerable<TalentGridViewItem>> GetRelated(int id, int? count = null)
        {
            Talent talent = TalentService.GetActiveByID(id);
            if (talent == null)
                return CustomBadRequest("Талант не найден");

            List<TalentGridViewItem> relatedTalents = TalentService.GetRelated(talent, count)
                .Select(m => new TalentGridViewItem(m))
                .ToList();

            foreach (var talentItem in relatedTalents)
            {
                if (talentItem.avatar.id == 0)
                    talentItem.avatar.url = GetRandomPhotoUrl();
            }

            return relatedTalents;
        }

        private string GetRandomPhotoUrl()
        {
            List<string> urls = new List<string>()
            {
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190322_175932/1076641576_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190323_144715/1131788095_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181230_125854/1151881448_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191022_141609/1244742035_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191022_161444/1273827994_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190512_221439/1302255751_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190903_231857/1345976596_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190512_221439/1392034683_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190112_230823/1412651936_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20160119_083125/1422806958_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20161218_000047/1530027694_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190112_221337/1598193086_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190903_231857/1629236964_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190712_234617/1645652980_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181103_151515/1246139023_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190321_090716/1260185104_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181230_125854/1303631832_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190330_170418/1338675767_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181104_121932/1420110925_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190704_214009/1446278688_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191120_082123/1465143706_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181104_121932/1420110925_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191022_141609/1489216552_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20170415_163011/1498801167_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190928_125853/1506653934_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20181230_125854/1656540307_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20190903_231857/1663342103_medium.jpg",
                "https://imagefreeblob.blob.core.windows.net/imagepreviews/hakon/20191022_122027/1751005141_medium.jpg"
            };

            Random random = new Random();
            int randomIndex = random.Next(0, urls.Count);
            return urls[randomIndex];
        }
    }
}
