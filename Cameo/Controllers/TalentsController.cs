using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
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

        public TalentsController(
            ITalentService talentService,
            ICategoryService categoryService,
            IVideoRequestTypeService videoRequestTypeService)
        {
            TalentService = talentService;
            CategoryService = categoryService;
            VideoRequestTypeService = videoRequestTypeService;
        }

        public IActionResult Index(int? cat)
        {
            PrepareViewDataItems(cat);
            return View(new FilterVM());
        }

        public IActionResult Details(int id)
        {
            Talent model = TalentService.GetActiveByID(id);
            if (model == null)
                return NotFound();

            TalentDetailsVM modelVM = new TalentDetailsVM(model);

            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

            return View(modelVM);
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

        //[HttpPost]
        public IActionResult Get(int categoryID, SortTypeEnum sort)
        {
            var talents = TalentService.Search(categoryID, sort)
                .Select(m => new TalentGridViewItem(m))
                .ToList();

            foreach (var item in talents)
            {
                item.Avatar.Url = GetRandomPhotoUrl();
            }

            ViewData["categories"] = CategoryService.GetAllActive()
                .ToDictionary(m => m.ID, m => m.Name);

            return PartialView(talents);
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