using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    public class VideoRequestStatusController : BaseController
    {
        private readonly IVideoRequestStatusService VideoRequestStatusService;

        public VideoRequestStatusController(
            IVideoRequestStatusService videoRequestStatusService,
            ILogger<VideoRequestStatusController> logger)
        {
            VideoRequestStatusService = videoRequestStatusService;
            _logger = logger;
        }

        public List<SelectListItem> GetAsSelectList(int selected = 0)
        {
            return VideoRequestStatusService.GetAsSelectList(new int[1] { selected });
        }
    }
}