using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ValidateIfUserWithUsernameExists(UserInput Input)
        {
            var existingUser = await _userManager.FindByNameAsync(Input.UserName);
            return Json(existingUser == null);
        }

        public async Task<IActionResult> ValidateIfUserWithEmailExists(UserInput Input)
        {
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            return Json(existingUser == null);
        }
    }

    public class UserInput
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}