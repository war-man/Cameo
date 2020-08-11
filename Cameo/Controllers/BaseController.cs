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

        //internal IActionResult CustomBadRequest(string errorMessage, bool fromException = false, bool isAjaxAction = false)
        //{
        //    if (!fromException)
        //    {
        //        string curUserID = "0";
        //        var curUser = accountUtil.GetCurrentUser(User);
        //        if (curUser != null && !string.IsNullOrWhiteSpace(curUser.ID))
        //            curUserID = curUser.ID;
        //        string errorMessageForLogging = "UserID = " + curUserID + "; " + errorMessage;

        //        _logger.LogError(errorMessageForLogging);
        //    }

        //    if (isAjaxAction)
        //        return BadRequest(new { errorMessage });
        //    else
        //        throw new Exception(errorMessage);
        //}

        //for ajax actions only
        internal IActionResult CustomBadRequest(Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += ". " + ex.InnerException.Message;

            _logger.LogError(ex, BuildErrorMessageForLogging(null, Request.Path.ToString(), true));

            return BadRequest(new { errorMessage = errorMessage });
        }

        internal string BuildErrorMessageForLogging(int? code, string originalPath, bool isAjaxAction)
        {
            string result = "Front-end: web; ";

            if (isAjaxAction)
                result += "ActionType: ajax; ";
            else
                result += "ActionType: usual; ";

            if (code.HasValue)
                result += "StatusCode = " + code.Value + "; ";

            if (!string.IsNullOrWhiteSpace(originalPath))
                result += "OriginalPath = " + originalPath + "; ";

            var curUser = accountUtil.GetCurrentUser(User);
            result += "UserID = " + curUser?.ID ?? "unauthorized";

            return result;
        }
    }
}