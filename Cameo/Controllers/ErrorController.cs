using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class ErrorController : Controller
    {
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

            // handle different codes or just return the default error view

            ViewBag.code = code;

            return View("Error");
        }
    }
}