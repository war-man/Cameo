using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class VideoRequestController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}