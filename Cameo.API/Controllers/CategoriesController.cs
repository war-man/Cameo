using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    //[AllowAnonymous()]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService CategoryService;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger<CategoriesController> logger)
        {
            CategoryService = categoryService;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<BaseDropdownableDetailsVM>> Get()
        {
            var categoriesVM = new List<BaseDropdownableDetailsVM>();

            var categories = CategoryService.GetAllActive();
            foreach (var item in categories)
            {
                categoriesVM.Add(new BaseDropdownableDetailsVM(item));
            }

            return categoriesVM;
        }
    }
}
