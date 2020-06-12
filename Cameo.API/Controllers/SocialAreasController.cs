using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    //[AllowAnonymous()]
    [Route("api/[controller]")]
    [ApiController]
    public class SocialAreasController : BaseController
    {
        private readonly ISocialAreaService SocialAreaService;

        public SocialAreasController(
            ISocialAreaService socialAreaService)
        {
            SocialAreaService = socialAreaService;
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
