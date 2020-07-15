using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost("SaveToken/{token}")]
        public IActionResult SaveToken(string token)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);

                FirebaseRegistrationTokenService.SaveToken(token, curUser.ID, "mob");

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpPost("RefreshToken/{oldToken}/{newToken}")]
        public IActionResult RefreshToken(string oldToken, string newToken)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);

                FirebaseRegistrationTokenService.RefreshToken(curUser.ID, oldToken, newToken, "mob");

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}