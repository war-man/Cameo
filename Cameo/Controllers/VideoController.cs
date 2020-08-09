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
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    public class VideoController : BaseController
    {
        private readonly IVideoRequestService VideoRequestService;
        private readonly IFileManagement FileManagement;

        public VideoController(
            IVideoRequestService videoRequestService,
            IFileManagement fileManagement,
            ILogger<VideoController> logger)
        {
            VideoRequestService = videoRequestService;
            FileManagement = fileManagement;
            _logger = logger;
        }

        public IActionResult Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetSinglePublished(id, curUser.ID);
            if (model == null)
                return CustomBadRequest("Видео не найдено");

            VideoDetailsVM videoVM = new VideoDetailsVM(model);
            videoVM.Video.Url = "/videos/hz2.mp4";

            return View(videoVM);
        }

        public IActionResult GetVideo(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest request = VideoRequestService.GetSinglePublished(id, curUser.ID);

            if (request == null)
                return CustomBadRequest("Видео не найдено");

            AttachmentDetailsVM video = new AttachmentDetailsVM(request.Video);

            return Content(video.Url);

            //string fileAbsolutePath = FileManagement.GetFileAbsolutePath(
            //    request.Video.Path,
            //    request.Video.GUID + "." + request.Video.Extension);

            //return PhysicalFile(fileAbsolutePath, "video/mp4");
        }

        //public IActionResult GetVideo(int id)
        //{
        //    var curUser = accountUtil.GetCurrentUser(User);
        //    VideoRequest model = VideoRequestService.GetSinglePublished(id, curUser.ID);

        //    if (model == null)
        //        return NotFound();

        //    string fileAbsolutePath = FileManagement.GetFileAbsolutePath(
        //        model.Video.Path, 
        //        model.Video.GUID + "." + model.Video.Extension);

        //    return PhysicalFile(fileAbsolutePath, "video/mp4");
        //}

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