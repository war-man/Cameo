using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public AccountUtil accountUtil = new AccountUtil();

        internal ILogger _logger;

        //internal ActionResult CustomBadRequest(string errorMessage, bool fromException = false)
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

        //    return BadRequest(new { error_message = errorMessage });
        //}

        internal ActionResult CustomBadRequest(Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += ". " + ex.InnerException.Message;

            _logger.LogError(ex, BuildErrorMessageForLogging(null, Request.Path.ToString()));

            return BadRequest(new { errorMessage = new string[1] { errorMessage } });
        }

        internal string BuildErrorMessageForLogging(int? code, string originalPath)
        {
            string result = "Front-end: mobile; ";

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