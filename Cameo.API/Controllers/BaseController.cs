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

        internal ActionResult CustomBadRequest(string errorMessage, bool fromException = false)
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

            return BadRequest(new { error_message = errorMessage });
        }

        internal ActionResult CustomBadRequest(Exception ex)
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

            return CustomBadRequest(errorMessage, true);
        }
    }
}