using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    public class ErrorController : BaseController
    {
        readonly private ILogger _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        //[Route("error/404")]
        //public IActionResult Error404()
        //{
        //    return View();
        //}

        [Route("Error/{code:int}")]
        public IActionResult Error(int? code)
        {
            var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Exception exception = pathFeature?.Error; // Here will be the exception details

            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var curUser = accountUtil.GetCurrentUser(User);

            _logger.LogError("StatusCode = " + code + "; OriginalPath = " + statusCodeData.OriginalPath + "; UserID = " + curUser?.ID ?? "unauthorized");

            // handle different codes or just return the default error view

            ViewBag.code = code;

            return View("Error");
        }
    }
}