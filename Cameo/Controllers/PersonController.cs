using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class PersonController : BaseController
    {
        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            if (curUser.Type == UserTypesEnum.customer.ToString())
                return RedirectToAction("PersonalData", "Customer");
            else
                return RedirectToAction("PersonalData", "Talent");
        }
    }
}