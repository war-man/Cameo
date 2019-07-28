using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class TalentProjectsAndCategoriesController 
        : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ICategoryService CategoryService;

        public TalentProjectsAndCategoriesController(
            ITalentService talentService,
            ICategoryService categoryService)
        {
            TalentService = talentService;
            CategoryService = categoryService;
        }

        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();

            TalentProjectsAndCategoriesEditVM modelVM = 
                new TalentProjectsAndCategoriesEditVM(model);

            ViewData["categories"] = CategoryService.GetAsSelectList(modelVM.Categories.ToArray());

            return View();
        }

        [HttpPost]
        public IActionResult Index(TalentProjectsAndCategoriesEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.TalentID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    UpdateTalentCategories(model, modelVM.Categories);

                    TalentService.Update(model, curUser.ID);

                    ViewData["successfullySaved"] = true;
                }
                catch (Exception ex)
                {
                    throw new SystemException("Something went wrong while saving data.", ex);
                }
            }
            else
                ModelState.AddModelError("", "Неверные данные");

            //if (model.AvatarID.HasValue)
            //    model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            //modelVM.Avatar = new AttachmentDetailsVM(model.Avatar);

            ViewData["categories"] = CategoryService.GetAsSelectList();

            return View(modelVM);
        }

        private void UpdateTalentCategories(Talent model, List<int> selectedCategories)
        {
            model.TalentCategories = new List<TalentCategory>();

            List<int> talentCategories = model.TalentCategories
                .Select(m => m.CategoryId)
                .ToList();

            List<Category> allCategories = CategoryService.GetAllActive().ToList();
            foreach (var category in allCategories)
            {
                if (selectedCategories.Contains(category.ID))
                {
                    if (!talentCategories.Contains(category.ID))
                    {
                        model.TalentCategories.Add(new TalentCategory()
                        {
                            Category = category,
                            Talent = model
                        });
                    }
                }
                else
                {
                    if (talentCategories.Contains(category.ID))
                    {
                        var cat = model.TalentCategories.FirstOrDefault(m => m.CategoryId == category.ID);
                        if  (cat != null)
                            model.TalentCategories.Remove(cat);
                    }
                }
            }
        }
    }
}