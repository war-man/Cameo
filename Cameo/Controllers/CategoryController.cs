﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cameo.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService CategoryService;

        public CategoryController(ICategoryService categoryService)
        {
            CategoryService = categoryService;
        }

        public List<SelectListItem> GetAsSelectList(int selected = 0)
        {
            return CategoryService.GetAsSelectList(new int[1] { selected });
        }

        public IActionResult GetAll(int selected = 0)
        {
            List<CategoryVM> categories = CategoryService.GetAsIQueryable()
                .Select(m => new CategoryVM()
                {
                    ID = m.ID,
                    Name = m.Name,
                    NumberOfItems = m.TalentCategories.Count
                })
                .ToList();

            return PartialView(categories);
        }
    }
}