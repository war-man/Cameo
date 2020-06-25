using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cameo.Controllers
{
    public class BaseController : Controller
    {
        protected string globalLang = "";
        public AccountUtil accountUtil = new AccountUtil();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ////language settings
            //string lang = Request.Cookies["culture"];
            //if (string.IsNullOrWhiteSpace(lang))
            //    lang = LocalisationUtil.GetDefaultLanguage();

            //new LocalisationUtil().SetLanguage(lang, HttpContext.Response);
            //globalLang = lang;
        }

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