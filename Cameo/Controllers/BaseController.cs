using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cameo.Controllers
{
    public class BaseController : Controller
    {
        public AccountUtil accountUtil = new AccountUtil();

        internal IActionResult CustomBadRequest(string errorMessage)
        {
            return BadRequest(new { errorMessage });
        }

        internal IActionResult CustomBadRequest(Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += ". " + ex.InnerException.Message;

            return CustomBadRequest(errorMessage);
        }
    }
}