﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cameo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : BaseController
    {
        private readonly IFirebaseRegistrationTokenService FirebaseRegistrationTokenService;

        public FirebaseController(IFirebaseRegistrationTokenService firebaseRegistrationTokenService)
        {
            FirebaseRegistrationTokenService = firebaseRegistrationTokenService;
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
    }
}