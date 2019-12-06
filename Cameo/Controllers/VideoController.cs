using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class VideoController : BaseController
    {
        private readonly IVideoRequestService VideoRequestService;

        public VideoController(
            IVideoRequestService videoRequestService)
        {
            VideoRequestService = videoRequestService;
        }

        public IActionResult Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);

            VideoRequest model = VideoRequestService.GetSinglePublished(id, curUser.ID);
            VideoDetailsVM modelVM = new VideoDetailsVM(model);

            return View(modelVM);
        }
    }
}