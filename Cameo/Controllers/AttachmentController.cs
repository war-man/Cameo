﻿using System;
using System.Collections.Generic;
using System.IO;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.Utils;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NReco;
using NReco.VideoConverter;

namespace Cameo.Controllers
{
    [Authorize]
    public class AttachmentController : BaseController
    {
        private readonly IAttachmentService AttachmentService;
        private readonly ICustomerService CustomerService;
        private readonly ITalentService TalentService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly IHangfireService HangfireService;

        private readonly IHostingEnvironment _env;

        public AttachmentController(
            IAttachmentService attachmentService,
            ICustomerService customerService,
            ITalentService talentService,
            IVideoRequestService videoRequestService,
            IHangfireService hangfireService,
            IHostingEnvironment env,
            ILogger<AttachmentController> logger)
        {
            AttachmentService = attachmentService;
            CustomerService = customerService;
            TalentService = talentService;
            VideoRequestService = videoRequestService;
            HangfireService = hangfireService;
            _env = env;
            _logger = logger;
        }

        //ajax
        [HttpPost]
        public IActionResult Save([FromBody] UploadFileVM uploadFileVM)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);

                Attachment attachment = uploadFileVM.ToModel();
                AttachmentService.Add(attachment, curUser.ID);

                if (uploadFileVM.ModelID.HasValue && uploadFileVM.ModelID.Value > 0)
                    AttachFile(attachment, uploadFileVM.ModelID.Value, uploadFileVM.FileType, curUser.ID);

                return Ok(new { id = attachment.ID });
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        private void AttachFile(Attachment attachment, int id, string fileType, string curUserID)
        {
            if (fileType.Equals(Constants.FileTypes.CUSTOMER_AVATAR))
            {
                var model = CustomerService.GetByID(id);
                if (model != null)
                {
                    model.Avatar = attachment;
                    CustomerService.Update(model, curUserID);
                }
            }
            else if (fileType.Equals(Constants.FileTypes.TALENT_AVATAR))
            {
                var model = TalentService.GetByID(id);
                if (model != null)
                {
                    model.Avatar = attachment;
                    TalentService.Update(model, curUserID);
                }
            }
            //else if (fileType.Equals(Constants.FileTypes.VIDEO_REQUEST_VIDEO))
            //{
            //    var model = VideoRequestService.GetByID(id);
            //    if (model != null)
            //    {
            //        if (VideoRequestService.IsVideoUploadable(model))
            //        {
            //            model.Video = attachment;
            //            VideoRequestService.SaveUploadedVideo(model, curUserID);

            //            //HangfireService.CreateTaskForConvertingVideo(attachment.ID, curUserID);
            //        }
            //    }
            //}
            //else if (fileType.Equals(Constants.FileTypes.VIDEO_REQUEST_PAYMENT_SCREENSHOT))
            //{
            //    var request = VideoRequestService.GetByID(id);
            //    if (request != null)
            //    {
            //        if (!request.PaymentScreenshotID.HasValue)
            //        {
            //            request.PaymentScreenshot = attachment;
            //            VideoRequestService.SaveUploadedPaymentScreenshot(request, curUserID);

            //            request.PaymentConfirmationJobID = HangfireService
            //                .CreateJobForVideoRequestPaymentConfirmationDeadline(request, curUserID);
            //        }
            //        else
            //            request.PaymentScreenshot = attachment;

            //        VideoRequestService.Update(request, curUserID);
            //    }
            //}
            //else if () ...
        }

        //[HttpPost]
        //[RequestSizeLimit(200000000)]
        //public IActionResult Upload(List<IFormFile> files, int? id, string fileType)
        //{
        //    var curUser = accountUtil.GetCurrentUser(User);

        //    IFormFile file = files[0];
        //    if (file != null)
        //    {
        //        Attachment attachment = new Attachment()
        //        {
        //            GUID = Guid.NewGuid().ToString(),
        //            Filename = file.FileName,
        //            Path = AppData.Configuration.UploadsPath,
        //            Extension = file.FileName.Split('.')[1],
        //            Size = file.Length,
        //            MimeType = file.ContentType
        //        };

        //        string rootPath = _env.ContentRootPath;

        //        string path = AppData.Configuration.UploadsPath;
        //        if (fileType.Equals(Constants.FileTypes.VIDEO_REQUEST_VIDEO))
        //        {
        //            string videosRelativePath = "/Videos";
        //            path += videosRelativePath;
        //            attachment.Path += videosRelativePath;
        //        }

        //        path += "/" + attachment.GUID + "." + file.FileName.Split('.')[1];
        //        path = path.Replace('/', '\\');

        //        string target = rootPath + path;

        //        using (var stream = new FileStream(target, FileMode.Create))
        //        {
        //            file.CopyTo(stream);

        //            AttachmentService.Add(attachment, curUser.ID);

        //            if (id.HasValue && id.Value > 0)
        //                AttachFile(attachment, id.Value, fileType, curUser.ID);
        //        }

        //        return Ok(new AttachmentDetailsVM(attachment));
        //    }

        //    return BadRequest();
        //}

        //ajax
        [HttpPost]
        public IActionResult Delete(int fileID, int? objID, string fileType)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);

                var model = AttachmentService.GetActiveByID(fileID);
                if (model == null)
                    return NotFound();

                string error = "";

                if (objID.HasValue && objID > 0
                    && !string.IsNullOrWhiteSpace(fileType))
                {
                    error = DetachFromObj(model, objID.Value, fileType, curUser.ID);
                    if (string.IsNullOrWhiteSpace(error))
                        AttachmentService.Delete(model, curUser.ID);
                }
                else
                    error = "Некоторые входящие данные неверные";

                if (!string.IsNullOrWhiteSpace(error))
                    throw new Exception(error);

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        private string DetachFromObj(Attachment attachment, int objID, string fileType, string curUserID)
        {
            if (fileType.Equals(Constants.FileTypes.VIDEO_REQUEST_VIDEO))
            {
                var obj = VideoRequestService.GetActiveByID(objID);
                if (obj == null)
                    return "Объект не найден";

                if (!VideoRequestService.VideoIsAllowedToBeDeleted(obj))
                    return "У Вас недостаточно прав для удаления этого видео";

                obj.Video = null;
                VideoRequestService.SaveDetachedVideo(obj, curUserID);
            }
            //else if () ...

            return null;
        }

        //uncomment when needed
        //[HttpPost]
        //public JsonResult UploadMultiple(List<IFormFile> files)
        //{
        //    return Json(new { });
        //}
    }
}