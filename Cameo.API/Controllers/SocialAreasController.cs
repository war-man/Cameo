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
    public class SocialAreasController : BaseController
    {
        private readonly ISocialAreaService SocialAreaService;

        public SocialAreasController(
            ISocialAreaService socialAreaService,
            ILogger<SocialAreasController> logger)
        {
            SocialAreaService = socialAreaService;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<BaseDropdownableDetailsVM>> Get()
        {
            var socialAreasVM = new List<BaseDropdownableDetailsVM>();

            var socialAreas = SocialAreaService.GetAllActive();
            foreach (var item in socialAreas)
            {
                socialAreasVM.Add(new BaseDropdownableDetailsVM(item));
            }

            return socialAreasVM;
        }
    }
}
