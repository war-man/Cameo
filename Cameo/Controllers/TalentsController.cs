using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Cameo.Utils;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    public class TalentsController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ITalentSearchService TalentSearchService;
        private readonly ICategoryService CategoryService;
        private readonly IVideoRequestTypeService VideoRequestTypeService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly IVideoRequestPriceCalculationsService VideoRequestPriceCalculationsService;

        public TalentsController(
            ITalentService talentService,
            ITalentSearchService talentSearchService,
            ICategoryService categoryService,
            IVideoRequestTypeService videoRequestTypeService,
            IVideoRequestService videoRequestService,
            IVideoRequestPriceCalculationsService videoRequestPriceCalculationsService,
            ILogger<TalentsController> logger)
        {
            TalentService = talentService;
            TalentSearchService = talentSearchService;
            CategoryService = categoryService;
            VideoRequestTypeService = videoRequestTypeService;
            VideoRequestService = videoRequestService;
            VideoRequestPriceCalculationsService = videoRequestPriceCalculationsService;
            _logger = logger;
        }

        public IActionResult Index(int? cat)
        {
            var curUser = accountUtil.GetCurrentUser(User);

            PrepareViewDataItems(cat);
            return View(new FilterVM());
        }

        //ajax
        public IActionResult GetCategorized()
        {
            try
            {
                List<TalentsCategorizedVM> talentsCategorizedVM = new List<TalentsCategorizedVM>();
                List<Category> categories = new List<Category>();

                var featuredTalents = TalentSearchService.GetFeatured(null, 6);
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

                var newTalents = TalentSearchService.GetNew(null, 6);
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
                    var talentsCategorized = TalentSearchService.Search(category.ID, SortTypeEnum.def, 6);
                    if (talentsCategorized.Count() > 0)
                    {
                        var categoryTalentsVM = new TalentsCategorizedVM(category, talentsCategorized.ToList());

                        talentsCategorizedVM.Add(categoryTalentsVM);
                    }
                }

                foreach (var categoryTalentsVM in talentsCategorizedVM)
                {
                    foreach (var talent in categoryTalentsVM.Talents)
                    {
                        if (talent.Avatar.ID == 0)
                            talent.Avatar.Url = TalentService.GetRandomPhotoUrl();
                    }
                }

                //ViewBag.isHomePage = true;

                return PartialView("_GetCategorizedHomePage", talentsCategorizedVM);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }   
        }

        public IActionResult ByCategory(int cat)
        {
            var category = CategoryService.GetByID(cat);
            if (category == null)
            {
                if (cat == (int)CategoryEnum.neW)
                    category = new Category() { Name = "Новые" };
                else if (cat == (int)CategoryEnum.featured)
                    category = new Category() { Name = "Популярные" };
                else
                    throw new Exception("Категория не найдена");
            }

            ViewBag.categoryID = cat;
            ViewBag.categoryName = category.Name;

            return View();
        }

        //ajax
        public IActionResult GetByCategory(int categoryID)
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

            try
            {
                List<TalentsCategorizedVM> talentsCategorizedVM = new List<TalentsCategorizedVM>();

                if (categoryID == (int)CategoryEnum.neW)
                {
                    var newTalents = TalentSearchService.GetNew(null);
                    if (newTalents.Count() > 0)
                    {
                        var newTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.all,
                                Name = "Все новые"
                            },
                            newTalents.ToList());
                        talentsCategorizedVM.Add(newTalentsVM);
                    }
                }
                else if (categoryID == (int)CategoryEnum.featured)
                {
                    var featuredTalents = TalentSearchService.GetFeatured(null, 6);
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

                    var newTalents = TalentSearchService.GetNewInFeatured(6);
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

                    var allTalents = TalentSearchService.GetFeatured(null);
                    if (allTalents.Count() > 0)
                    {
                        var allTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.all,
                                Name = "Все Популярные"
                            },
                            allTalents.ToList());
                        talentsCategorizedVM.Add(allTalentsVM);
                    }
                }
                else
                {
                    var categoryDB = CategoryService.GetActiveByID(categoryID);
                    if (categoryDB == null)
                        throw new Exception("Категория не найдена");

                    var featuredTalents = TalentSearchService.GetFeatured(categoryID, 6);
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

                    var newTalents = TalentSearchService.GetNew(categoryID, 6);
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

                    var allTalents = TalentSearchService.Search(categoryID, SortTypeEnum.def);
                    if (allTalents.Count() > 0)
                    {
                        var allTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.all,
                                Name = "Все"
                            },
                            allTalents.ToList());
                        talentsCategorizedVM.Add(allTalentsVM);
                    }

                    ViewBag.categoryName = categoryDB.Name;
                }

                foreach (var categoryTalentsVM in talentsCategorizedVM)
                {
                    foreach (var talent in categoryTalentsVM.Talents)
                    {
                        if (talent.Avatar.ID == 0)
                            talent.Avatar.Url = TalentService.GetRandomPhotoUrl();
                    }
                }

                ViewData["sortingItems"] = TalentSearchService.GetSortOptions();

                return PartialView("_GetCategorized", talentsCategorizedVM);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        //ajax
        public IActionResult LoadAllSortedTalentsByCategory(int categoryID, SortTypeEnum sort)
        {
            try
            {
                TalentsCategorizedVM talentsCategorizedVM = new TalentsCategorizedVM();

                if (categoryID == (int)CategoryEnum.neW)
                {
                    var newTalents = TalentSearchService.GetNew(null, sort: sort);
                    if (newTalents.Count() > 0)
                    {
                        var newTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.all,
                                Name = "Все новые"
                            },
                            newTalents.ToList());
                        talentsCategorizedVM = newTalentsVM;
                    }
                }
                else if (categoryID == (int)CategoryEnum.featured)
                {
                    var allTalents = TalentSearchService.GetFeatured(null, sort: sort);
                    if (allTalents.Count() > 0)
                    {
                        var allTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.all,
                                Name = "Все Популярные"
                            },
                            allTalents.ToList());
                        talentsCategorizedVM = allTalentsVM;
                    }
                }
                else
                {
                    var categoryDB = CategoryService.GetActiveByID(categoryID);
                    if (categoryDB == null)
                        throw new Exception("Категория не найдена");

                    var allTalents = TalentSearchService.Search(categoryID, sort);
                    if (allTalents.Count() > 0)
                    {
                        var allTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.all,
                                Name = "Все"
                            },
                            allTalents.ToList());
                        talentsCategorizedVM = allTalentsVM;
                    }

                    ViewBag.categoryName = categoryDB.Name;
                }

                foreach (var talent in talentsCategorizedVM.Talents)
                {
                    if (talent.Avatar.ID == 0)
                        talent.Avatar.Url = TalentService.GetRandomPhotoUrl();
                }

                return PartialView("_Get", talentsCategorizedVM.Talents);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        //public IActionResult Details(int id)
        //{
        //    Talent model = TalentService.GetActiveByID(id);
        //    if (model == null)
        //        return NotFound();

        //    TalentDetailsVM modelVM = new TalentDetailsVM(model);

        //    ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

        //    return View(modelVM);
        //}

        public IActionResult Details(string username)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent talent = TalentService.GetActiveByUsername(username);
            if (talent == null)
                throw new Exception("Талант не найден");
                //return NotFound();

            TalentDetailsVM modelVM = new TalentDetailsVM(talent);
            modelVM.RequestPrice = VideoRequestPriceCalculationsService.CalculateRequestPrice(talent);
            modelVM.RequestPriceToStr();

            if (modelVM.Avatar.ID == 0)
                modelVM.Avatar.Url = TalentService.GetRandomPhotoUrl();

            //ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();
            ViewData["isUserCustomer"] = AccountUtil.IsUserCustomer(curUser);

            //ViewData["VideoRequestCreateVM"] = new VideoRequestCreateVM()
            //{
            //    TypeID = (int)VideoRequestTypeEnum.someone
            //};

#if DEBUG
            if (modelVM.IntroVideo.ID > 0)
                modelVM.IntroVideo.Url = "/videos/hz2.mp4";
#endif

            return View(modelVM);
        }

        //ajax
        public IActionResult GetLatestVideosForTalent(int id)
        {
            try
            {
                Talent talent = TalentService.GetActiveByID(id);
                if (talent == null)
                    throw new Exception("Талант не найден");
                    //return NotFound();

                List<VideoRequest> videos = VideoRequestService.GetPublicByTalent(talent, 0)
                    .ToList();

                List<VideoDetailsVM> videosVM = new List<VideoDetailsVM>();
                foreach (var item in videos)
                {
                    VideoDetailsVM videoVM = new VideoDetailsVM(item);
#if DEBUG
                    videoVM.Video.Url = "/videos/hz2.mp4";
#endif
                    videosVM.Add(videoVM);
                }

                return PartialView(videosVM);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        //ajax
        public IActionResult GetRelated(int id, int? count = null)
        {
            try
            {
                Talent model = TalentService.GetActiveByID(id);
                if (model == null)
                    throw new Exception("Талант не найден");
                    //return NotFound();

                List<TalentGridViewItem> relatedTalents = TalentSearchService.GetRelated(model, count)
                    .Select(m => new TalentGridViewItem(m))
                    .ToList();

                foreach (var talent in relatedTalents)
                {
                    if (talent.Avatar.ID == 0)
                        talent.Avatar.Url = TalentService.GetRandomPhotoUrl();
                }

                return PartialView(relatedTalents);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        public IActionResult Get(int categoryID, SortTypeEnum sort)
        {
            var talents = TalentSearchService.Search(categoryID, sort)
                .Select(m => new TalentGridViewItem(m))
                .ToList();

            foreach (var item in talents)
            {
                if (item.Avatar.ID == 0)
                    item.Avatar.Url = TalentService.GetRandomPhotoUrl();
            }

            //ViewData["categories"] = CategoryService.GetAllActive()
            //    .ToDictionary(m => m.ID, m => m.Name);

            return PartialView(talents);
        }

        //ajax
        public IActionResult GetBySearchText(string searchText)
        {
            try
            {
                var talents = TalentSearchService.SearchBySearchText(searchText)
                .Select(m => new TalentShortInfoVM(m))
                .ToList();

                foreach (var item in talents)
                {
                    if (item.Avatar.ID == 0)
                        item.Avatar.Url = TalentService.GetRandomPhotoUrl();
                }

                return PartialView("_SearchBoxResult", talents);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        private void PrepareViewDataItems(int? cat)
        {
            ViewData["sortingItems"] = TalentSearchService.GetSortOptions();
            ViewData["cat"] = cat;
        }
    }
}