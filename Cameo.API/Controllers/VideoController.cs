using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    //[AllowAnonymous()]
    [Route("api/[controller]")]
    [ApiController]
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

        // GET api/values/5
        [HttpGet("{request_id}")]
        public ActionResult<VideoDetailsVM> Get(int request_id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetSinglePublished(request_id, curUser.ID);
            if (model == null)
                return NotFound();

            VideoDetailsVM modelVM = new VideoDetailsVM(model);
            return modelVM;
        }

        [HttpGet("GetVideo/{request_id}")]
        public IActionResult GetVideo(int request_id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetSinglePublished(request_id, curUser.ID);

            if (model == null)
                return NotFound();

            string fileAbsolutePath = FileManagement.GetFileAbsolutePath(
                model.Video.Path,
                model.Video.GUID + "." + model.Video.Extension);

            return PhysicalFile(fileAbsolutePath, "video/mp4");
        }
    }
}
