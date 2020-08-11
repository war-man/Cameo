using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    [Authorize]
    public class FirebaseController : BaseController
    {
        private readonly IFirebaseRegistrationTokenService FirebaseRegistrationTokenService;

        public FirebaseController(
            IFirebaseRegistrationTokenService firebaseRegistrationTokenService,
            ILogger<FirebaseController> logger)
        {
            FirebaseRegistrationTokenService = firebaseRegistrationTokenService;
            _logger = logger;
        }

        //ajax
        [HttpPost]
        public IActionResult SaveToken(string token)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);

                FirebaseRegistrationTokenService.SaveToken(token, curUser.ID, "web");

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}