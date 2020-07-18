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

        public IActionResult GetCategorized()
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

            ViewBag.isHomePage = true;

            return PartialView("_GetCategorized", talentsCategorizedVM);
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
                    return CustomBadRequest("Категория не найдена");
            }

            ViewBag.categoryID = cat;
            ViewBag.categoryName = category.Name;

            return View();
        }

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

            List<TalentsCategorizedVM> talentsCategorizedVM = new List<TalentsCategorizedVM>();

            if (categoryID == (int)CategoryEnum.neW)
            {
                var newTalents = TalentSearchService.GetNew(null);
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
                            ID = (int)CategoryEnum.neW,
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
                    return CustomBadRequest("Категория не найдена");

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
                            ID = (int)CategoryEnum.neW,
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

            return PartialView("_GetCategorized", talentsCategorizedVM);
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
                return NotFound();

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

            return View(modelVM);
        }

        public IActionResult GetLatestVideosForTalent(int id)
        {
            Talent talent = TalentService.GetActiveByID(id);
            if (talent == null)
                return NotFound();

            List<VideoRequest> videos = VideoRequestService.GetPublicByTalent(talent, 0)
                .ToList();

            List<VideoDetailsVM> videosVM = new List<VideoDetailsVM>();
            foreach (var item in videos)
            {
                videosVM.Add(new VideoDetailsVM(item));
            }

            return PartialView(videosVM);
        }

        public IActionResult GetRelated(int id, int? count = null)
        {
            Talent model = TalentService.GetActiveByID(id);
            if (model == null)
                return NotFound();

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

        public IActionResult GetBySearchText(string searchText)
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

        private void PrepareViewDataItems(int? cat)
        {
            List<SelectListItem> sortingItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = SortTypeEnum.def.ToString(),
                    Text = "Обычная сортировка",
                    Selected = true
                },
                new SelectListItem()
                {
                    Value = SortTypeEnum.priceAsc.ToString(),
                    Text = "Цена (по возрастанию)"
                },
                new SelectListItem()
                {
                    Value = SortTypeEnum.priceDesc.ToString(),
                    Text = "Цена (по возрастанию)"
                },
                new SelectListItem()
                {
                    Value = SortTypeEnum.az.ToString(),
                    Text = "А-Я"
                },
                new SelectListItem()
                {
                    Value = SortTypeEnum.responseTime.ToString(),
                    Text = "Время отклика (от быстрого к долгому)"
                },
            };

            ViewData["sortingItems"] = sortingItems;
            ViewData["cat"] = cat;
        }
    }
}