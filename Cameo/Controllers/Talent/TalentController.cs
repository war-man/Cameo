using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class TalentController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}