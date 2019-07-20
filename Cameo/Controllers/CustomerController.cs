using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class CustomerController : BaseController
    {
        public IActionResult PersonalData()
        {
            var curUser = accountUtil.GetCurrentUser(User);

            return View();
        }
    }
}