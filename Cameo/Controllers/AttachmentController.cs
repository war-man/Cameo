using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class AttachmentController : Controller
    {
        //TO-DO
        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null)
            {
                int id = 0;

                Attachment attachment = new Attachment();
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }


                return Ok(id);
            }

            

            return BadRequest();
        }

        //uncomment when needed
        //[HttpPost]
        //public JsonResult UploadMultiple(List<IFormFile> files)
        //{
        //    return Json(new { });
        //}
    }
}