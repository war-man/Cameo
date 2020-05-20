using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITalentService TalentService;

        Random random = new Random();

        public UserController(
            UserManager<ApplicationUser> userManager,
            ITalentService talentService)
        {
            _userManager = userManager;
            TalentService = talentService;
        }

        //For Customer
        public IActionResult ThanksForRegistrationCustomer()
        {
            return View();
        }

        //For Talent
        public IActionResult ThanksForRegistrationTalent()
        {
            return View();
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

        public async Task<IActionResult> UpdateTalentsScript()
        {
            string errorMessage = "";
            string exceptionMessage = "";

            try
            {
                var talents = TalentService.GetAll().ToList();
                foreach (var talent in talents)
                {
                    if (!string.IsNullOrWhiteSpace(talent.FullName)
                        && !string.IsNullOrWhiteSpace(talent.UserID))
                        continue;

                    talent.FullName = talent.FirstName + " " + talent.LastName;

                    var user = new ApplicationUser
                    {
                        UserName = talent.FirstName.ToLower()[0] + talent.LastName.ToLower(),
                        PhoneNumber = GenerateRandomPhoneNumber(),
                        UserType = UserTypesEnum.talent.ToString()
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        talent.User = user;
                    }
                    else
                    {
                        errorMessage += "Unable to create user for talent " + talent.ID + "; \n";
                    }
                }

                TalentService.UpdateCollection(talents, accountUtil.GetCurrentUser(User).ID);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                if (ex.InnerException != null)
                    exceptionMessage += ". Inner exception: " + ex.InnerException.Message;
            }

            return Content(errorMessage + "\n\n" + exceptionMessage);
        }

        private string GenerateRandomPhoneNumber()
        {
            string phoneNumber = "+9989";
            for (int i = 0; i < 8; i++)
            {
                phoneNumber += random.Next(0, 10);
            }

            return phoneNumber;
        }
    }

    public class UserInput
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}