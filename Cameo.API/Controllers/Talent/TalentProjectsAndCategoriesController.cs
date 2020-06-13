using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet]
        public ActionResult<TalentProjectsAndCategoriesEditVM> Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return CustomBadRequest("Талант не найден");

            TalentProjectsAndCategoriesEditVM modelVM = 
                new TalentProjectsAndCategoriesEditVM(model);

            return Ok(modelVM);
        }

        [HttpPost]
        public ActionResult Index([FromBody] TalentProjectsAndCategoriesEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.talent_id);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return CustomBadRequest("Талант не найден");

            if (ModelState.IsValid)
            {
                try
                {
                    UpdateTalentCategories(model, modelVM.categories);
                    UpdateTalentProjects(model, modelVM.projects);

                    TalentService.Update(model, curUser.ID);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return CustomBadRequest(ex);
                }
            }
            else
                return CustomBadRequest("Указаны некорректные данные");
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