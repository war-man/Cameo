using System;
using System.Collections.Generic;
using System.IO;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class AttachmentController : BaseController
    {
        private readonly IAttachmentService AttachmentService;

        private readonly ICustomerService CustomerService;
        private readonly ITalentService TalentService;

        public AttachmentController(
            IAttachmentService attachmentService,
            ICustomerService customerService,
            ITalentService talentService)
        {
            AttachmentService = attachmentService;
            CustomerService = customerService;
            TalentService = talentService;
        }

        [HttpPost]
        public IActionResult Upload(List<IFormFile> files, int? id, string fileType)
        {
            var curUser = accountUtil.GetCurrentUser(User);

            IFormFile file = files[0];
            if (file != null)
            {
                Attachment attachment = new Attachment()
                {
                    GUID = Guid.NewGuid().ToString(),
                    Filename = file.FileName,
                    Path = AppData.Configuration.UploadsPath,
                    Extension = file.FileName.Split('.')[1],
                    Size = file.Length,
                    MimeType = file.ContentType
                };

                string rootPath = AppData.Configuration.ApplicationRootPath;
                string path = AppData.Configuration.UploadsPath + "/" + attachment.GUID + "." + file.FileName.Split('.')[1];
                path = path.Replace('/', '\\');

                string target = rootPath + path;

                using (var stream = new FileStream(target, FileMode.Create))
                {
                    file.CopyTo(stream);
                    AttachmentService.Add(attachment, curUser.ID);

                    if (id.HasValue && id.Value > 0)
                        AttachFile(attachment, id.Value, fileType, curUser.ID);
                }

                return Ok(attachment.ID);
            }

            return BadRequest();
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
            //else if () ...
        }

        //uncomment when needed
        //[HttpPost]
        //public JsonResult UploadMultiple(List<IFormFile> files)
        //{
        //    return Json(new { });
        //}
    }
}