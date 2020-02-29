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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Exception exception = pathFeature?.Error; // Here will be the exception details

            var curUser = accountUtil.GetCurrentUser(User);
            _logger.LogError(exception, "UserID = " + curUser?.ID ?? "unauthorized");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Error/Status/{code:int}")]
        public IActionResult Status(int? code)
        {
            var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Exception exception = pathFeature?.Error; // Here will be the exception details

            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var curUser = accountUtil.GetCurrentUser(User);

            _logger.LogError("StatusCode = " + code + "; OriginalPath = " + statusCodeData.OriginalPath + "; UserID = " + curUser?.ID ?? "unauthorized");

            // handle different codes or just return the default error view

            ViewBag.code = code;

            //return View("Error");
            return View();
        }
    }
}