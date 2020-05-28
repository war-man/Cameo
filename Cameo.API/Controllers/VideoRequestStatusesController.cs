using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    //[AllowAnonymous()]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoRequestStatusesController : BaseController
    {
        private readonly IVideoRequestStatusService VideoRequestStatusService;

        public VideoRequestStatusesController(
            IVideoRequestStatusService videoRequestStatusService)
        {
            VideoRequestStatusService = videoRequestStatusService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<BaseDropdownableDetailsVM>> Get()
        {
            var statusesForFilter = VideoRequestStatusService.GetAsSelectListForFilter();
            var statusesVM = new List<BaseDropdownableDetailsVM>();
            
            foreach (var item in statusesForFilter)
            {
                statusesVM.Add(new BaseDropdownableDetailsVM(item));
            }

            return statusesVM;
        }
    }
}
