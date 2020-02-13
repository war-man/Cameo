using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class VideoController : BaseController
    {
        private readonly IVideoRequestService VideoRequestService;
        private readonly IFileManagement FileManagement;

        public VideoController(
            IVideoRequestService videoRequestService,
            IFileManagement fileManagement)
        {
            VideoRequestService = videoRequestService;
            FileManagement = fileManagement;
        }

        public IActionResult Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetSinglePublished(id, curUser.ID);
            VideoDetailsVM modelVM = new VideoDetailsVM(model);

            return View(modelVM);
        }

        public IActionResult GetVideo(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetSinglePublished(id, curUser.ID);

            if (model == null)
                return null;

            string fileAbsolutePath = FileManagement.GetFileAbsolutePath(
                model.Video.Path, 
                model.Video.GUID + "." + model.Video.Extension);

            return PhysicalFile(fileAbsolutePath, "video/mp4");
        }

        public IActionResult GetIncompletedVideo(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetIncompletedVideo(id, curUser.ID);

            if (model == null)
                return null;

            string fileAbsolutePath = FileManagement.GetFileAbsolutePath(
                model.Video.Path,
                model.Video.GUID + "." + model.Video.Extension);

            return PhysicalFile(fileAbsolutePath, "video/mp4");
        }
    }
}