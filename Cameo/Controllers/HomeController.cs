using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cameo.Models;
using Cameo.Data;
using Microsoft.AspNetCore.Diagnostics;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    public class HomeController : BaseController
    {
        readonly private IBackgroundJobClient _backgroundJobs;
        readonly private ILogger _logger;

        public HomeController(IBackgroundJobClient backgroundJobs, ILogger<HomeController> logger)
        {
            _backgroundJobs = backgroundJobs;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //_backgroundJobs.Enqueue(() => Console.WriteLine("AAAAAA!"));

            //_logger.LogInformation("Home Index page opened");
            //int k = 6;
            //int l = k / 0;

            var curUser = accountUtil.GetCurrentUser(User);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Exception exception = pathFeature?.Error; // Here will be the exception details

            var curUser = accountUtil.GetCurrentUser(User);
            _logger.LogError(exception, "UserID = " + curUser?.ID ?? "unauthorized");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
