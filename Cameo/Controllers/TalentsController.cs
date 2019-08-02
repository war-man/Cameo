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

        public TalentsController(
            ITalentService talentService,
            ICategoryService categoryService)
        {
            TalentService = talentService;
            CategoryService = categoryService;
        }

        public IActionResult Index()
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

            return View(new FilterVM());
        }

        //[HttpPost]
        public IActionResult Get(int categoryID, SortTypeEnum sort)
        {
            var talents = TalentService.Search(categoryID, sort)
                .Select(m => new TalentGridViewItem(m))
                .ToList();

            ViewData["categories"] = CategoryService.GetAllActive()
                .ToDictionary(m => m.ID, m => m.Name);

            return PartialView(talents);
        }
    }
}