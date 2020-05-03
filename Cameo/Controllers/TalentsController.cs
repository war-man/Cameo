﻿using System;
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

namespace Cameo.Controllers
{
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

        public IActionResult Index(int? cat)
        {
            PrepareViewDataItems(cat);
            return View(new FilterVM());
        }

        public IActionResult GetCategorized()
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
                var categoryTalentsVM = new TalentsCategorizedVM(category, talentsCategorized.ToList());
                
                talentsCategorizedVM.Add(categoryTalentsVM);
            }

            foreach (var categoryTalentsVM in talentsCategorizedVM)
            {
                foreach (var talent in categoryTalentsVM.Talents)
                {
                    if (talent.Avatar.ID == 0)
                        talent.Avatar.Url = GetRandomPhotoUrl();
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
            Talent model = TalentService.GetActiveByUsername(username);
            if (model == null)
                return NotFound();

            TalentDetailsVM modelVM = new TalentDetailsVM(model);

            VideoRequest videoRequest = VideoRequestService.GetRandomSinglePublishedByTalent(model, curUser.ID);
            if (videoRequest != null)
            {
                modelVM.Video = new AttachmentDetailsVM(videoRequest.Video);
                modelVM.RequestID = videoRequest.ID;
            }

            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();
            ViewData["isUserCustomer"] = AccountUtil.IsUserCustomer(curUser);

            return View(modelVM);
        }

        public IActionResult GetLatestForTalent(int talentID, int requestIDToBeExcluded)
        {
            Talent talent = TalentService.GetActiveByID(talentID);
            if (talent == null)
                return NotFound();

            List<VideoRequest> videos = VideoRequestService.GetPublicForTalent(talent, requestIDToBeExcluded)
                .ToList();

            List<VideoDetailsVM> videosVM = new List<VideoDetailsVM>();
            foreach (var item in videos)
            {
                videosVM.Add(new VideoDetailsVM(item));
            }

            return PartialView(videosVM);
        }

        public IActionResult GetRelated(int id)
        {
            Talent model = TalentService.GetActiveByID(id);
            if (model == null)
                return NotFound();

            List<TalentGridViewItem> relatedTalents = TalentService.GetRelated(model)
                .Select(m => new TalentGridViewItem(m))
                .ToList();

            return PartialView(relatedTalents);
        }

        public IActionResult Get(int categoryID, SortTypeEnum sort)
        {
            var talents = TalentService.Search(categoryID, sort)
                .Select(m => new TalentGridViewItem(m))
                .ToList();

            foreach (var item in talents)
            {
                if (item.Avatar.ID == 0)
                    item.Avatar.Url = GetRandomPhotoUrl();
            }

            //ViewData["categories"] = CategoryService.GetAllActive()
            //    .ToDictionary(m => m.ID, m => m.Name);

            return PartialView(talents);
        }

        public IActionResult GetBySearchText(string searchText)
        {
            var talents = TalentService.SearchBySearchText(searchText)
                .Select(m => new TalentShortInfoVM(m))
                .ToList();

            foreach (var item in talents)
            {
                if (item.Avatar.ID == 0)
                    item.Avatar.Url = GetRandomPhotoUrl();
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