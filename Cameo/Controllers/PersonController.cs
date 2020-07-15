using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class PersonController : BaseController
    {
        public PersonController()
        {
        }

        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            if (curUser.Type == UserTypesEnum.talent.ToString())
                return RedirectToAction("Index", "Talent");
            else
                return RedirectToAction("Index", "Customer");
        }

        //public IActionResult Book(int id)
        //{
        //    Talent model = TalentService.GetActiveByID(id);



        //    return View();
        //}
    }
}