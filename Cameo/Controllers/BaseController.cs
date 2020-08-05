using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cameo.Controllers
{
    public class BaseController : Controller
    {
        protected string globalLang = "";
        public AccountUtil accountUtil = new AccountUtil();

        internal ILogger _logger;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ////language settings
            //string lang = Request.Cookies["culture"];
            //if (string.IsNullOrWhiteSpace(lang))
            //    lang = LocalisationUtil.GetDefaultLanguage();

            //new LocalisationUtil().SetLanguage(lang, HttpContext.Response);
            //globalLang = lang;
        }

        internal IActionResult CustomBadRequest(string errorMessage, bool fromException = false, bool isAjaxAction = false)
        {
            if (!fromException)
            {
                string curUserID = "0";
                var curUser = accountUtil.GetCurrentUser(User);
                if (curUser != null && !string.IsNullOrWhiteSpace(curUser.ID))
                    curUserID = curUser.ID;
                string errorMessageForLogging = "UserID = " + curUserID + "; " + errorMessage;

                _logger.LogError(errorMessageForLogging);
            }

            if (isAjaxAction)
                return BadRequest(new { errorMessage });
            else
                throw new Exception(errorMessage);
        }

        internal IActionResult CustomBadRequest(Exception ex, bool isAjaxAction = false)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += ". " + ex.InnerException.Message;

            string curUserID = "unauthorized";
            var curUser = accountUtil.GetCurrentUser(User);
            if (curUser != null && !string.IsNullOrWhiteSpace(curUser.ID))
                curUserID = curUser.ID;
            string errorMessageForLogging = "UserID = " + curUserID + "; " + ex.Message;

            _logger.LogError(ex, errorMessageForLogging);

            return CustomBadRequest(errorMessage, true, isAjaxAction);
        }
    }
}