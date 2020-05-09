﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cameo.API.Utils;
using Cameo.API.ViewModels;
using Cameo.Common;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Cameo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IFirebaseService FirebaseService;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService CustomerService;

        public AccountController(
            IFirebaseService firebaseService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ICustomerService customerService)
        {
            FirebaseService = firebaseService;
            _signInManager = signInManager;
            _userManager = userManager;
            CustomerService = customerService;
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticateResponseVM>> Authenticate([FromBody] LoginVM login)
        {
            int statusCode = 200;
            string errorMessage = null;

            bool registrationIsRequired = true;
            string authToken = null;
            string userType = null;

            try
            {
                string phoneNumber = await FirebaseService.GetPhoneNumberByUID(login.firebase_uid);
                var existingUser = GetUserByPhoneNumber(phoneNumber);
                if (existingUser != null)
                {
                    if (existingUser.UserType == UserTypesEnum.talent.ToString()
                        && !existingUser.TalentApprovedByAdmin)
                    {
                        errorMessage = "Ваша заявка как Таланта еще не одобрена. Мы с Вами свяжемся.";
                        statusCode = 400;
                    }
                    else
                    {
                        authToken = GenerateAuthToken(existingUser);
                        userType = existingUser.UserType;
                    }

                    registrationIsRequired = false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                statusCode = 500;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage)
                && statusCode != 200)
            {
                Response.StatusCode = statusCode;
            }

            return new AuthenticateResponseVM(
                errorMessage,
                registrationIsRequired,
                authToken,
                userType);
        }

        private ApplicationUser GetUserByPhoneNumber(string phoneNumber)
        {
            return _userManager.Users.FirstOrDefault(m => m.PhoneNumber.Equals(phoneNumber));
        }

        private string GenerateAuthToken(ApplicationUser user)
        {
            string tokenSecurityKey = AppData.Configuration.TokenSecurityKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                //new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.Role, "Administrator"),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(CustomClaimTypes.UserType, user.UserType)
            };
#if DEBUG
            string validIssuer = AppData.Configuration.TokenValidIssuer;
            string validAudience = AppData.Configuration.TokenValidAudience;
#else
            string validIssuer = AppData.Configuration.TokenValidIssuerServer;
            string validAudience = AppData.Configuration.TokenValidAudienceServer;
#endif
            int tokenExpirationPeriodInDays = AppData.Configuration.TokenExpirationPeriodInDays;
            var token = new JwtSecurityToken(validIssuer, validAudience, claims, expires: DateTime.Now.AddDays(tokenExpirationPeriodInDays), signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseVM>> Register([FromBody] RegisterVM registerVM)
        {
            int statusCode = 200;
            string errorMessage = null;

            try
            {
                if (ModelState.IsValid)
                {
                    string phoneNumber = await FirebaseService.GetPhoneNumberByUID(registerVM.firebase_uid);
                    var existingUser = GetUserByPhoneNumber(phoneNumber);
                    if (existingUser == null)
                    {
                        existingUser = await _userManager.FindByNameAsync(registerVM.username);
                        if (existingUser == null)
                        {
                            var user = new ApplicationUser
                            {
                                UserName = registerVM.username,
                                PhoneNumber = phoneNumber,
                                FirebaseUid = registerVM.firebase_uid,
                                UserType = UserTypesEnum.customer.ToString()
                            };

                            var result = await _userManager.CreateAsync(user);
                            if (result.Succeeded)
                            {
                                Customer customer = new Customer()
                                {
                                    FullName = registerVM.full_name,
                                    UserID = user.Id
                                };
                                CustomerService.Add(customer, user.Id);
                            }

                            string authToken = GenerateAuthToken(user);
                            string userType = user.UserType;

                            return Ok(new RegisterResponseVM(authToken, userType));
                        }
                        else
                            errorMessage = "Пользователь с таким именем уже зарегистрирован";
                    }
                    else
                        errorMessage = "Пользователь с таким телефоном уже зарегистрирован";
                }
                else
                    errorMessage = "Введены неверные данные";

                if (!string.IsNullOrWhiteSpace(errorMessage))
                    statusCode = 400;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                statusCode = 500;

                Response.StatusCode = statusCode;
            }

            return BadRequest(errorMessage);
        }

        [HttpPost("Authenticate0")]
        public async Task<IActionResult> Authenticate0([FromBody] LoginVM0 login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
                return NotFound("User not found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, true);
            //var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                string tokenSecurityKey = AppData.Configuration.TokenSecurityKey;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };
#if DEBUG
                string validIssuer = AppData.Configuration.TokenValidIssuer;
                string validAudience = AppData.Configuration.TokenValidAudience;
#else
                string validIssuer = AppData.Configuration.TokenValidIssuerServer;
                string validAudience = AppData.Configuration.TokenValidAudienceServer;
#endif
                int tokenExpirationPeriodInDays = AppData.Configuration.TokenExpirationPeriodInDays;
                var token = new JwtSecurityToken(validIssuer, validAudience, claims, expires: DateTime.Now.AddDays(tokenExpirationPeriodInDays), signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return new JsonResult(new { token = tokenString });
            }
            else
                return BadRequest("Bad password");
        }
    }

    //public class LoginVM
    //{
    //    public string uid { get; set; }
    //}
    public class LoginVM0
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}