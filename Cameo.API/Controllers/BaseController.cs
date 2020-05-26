using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cameo.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public AccountUtil accountUtil = new AccountUtil();

        internal ActionResult CustomBadRequest(string errorMessage)
        {
            return BadRequest(new { error_message = errorMessage });
        }

        internal ActionResult CustomBadRequest(Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += ". " + ex.InnerException.Message;

            return CustomBadRequest(errorMessage);
        }
    }
}