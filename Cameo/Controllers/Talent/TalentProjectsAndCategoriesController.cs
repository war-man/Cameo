using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    public class TalentProjectsAndCategoriesController 
        : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ICategoryService CategoryService;

        public TalentProjectsAndCategoriesController(
            ITalentService talentService,
            ICategoryService categoryService,
            ILogger<TalentProjectsAndCategoriesController> logger)
        {
            TalentService = talentService;
            CategoryService = categoryService;
            _logger = logger;
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

            return View(modelVM);
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
                    UpdateTalentProjects(model, modelVM.Projects);

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
            if (selectedCategories == null || selectedCategories.Count == 0)
            {
                model.TalentCategories = new List<TalentCategory>();
                return;
            }

            HashSet<int> selectedCategoriesHS = new HashSet<int>(selectedCategories);
            HashSet<int> talentCategories = new HashSet<int>(model.TalentCategories
                .Select(m => m.CategoryId));

            List<Category> allCategories = CategoryService.GetAllActive().ToList();
            foreach (var category in allCategories)
            {
                if (selectedCategoriesHS.Contains(category.ID))
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

        private void UpdateTalentProjects(Talent model, List<string> projectNames)
        {
            if (projectNames == null || projectNames.Count == 0)
            {
                model.Projects = new List<TalentProject>();
                return;
            }

            model.Projects.Clear();

            foreach (var name in projectNames)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    model.Projects.Add(new TalentProject()
                    {
                        Name = name
                    });
                }
            }
        }
    }
}