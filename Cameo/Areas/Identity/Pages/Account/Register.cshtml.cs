using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cameo.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;
        private readonly ICustomerService CustomerService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            //IEmailSender emailSender,
            ICustomerService customerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            CustomerService = customerService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            //[Required]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            [Remote("ValidateIfUserWithEmailExists", "User", ErrorMessage = "Пользователь с такой почтой уже существует")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "UserName")]
            [RegularExpression("^[a-z0-9]*$", ErrorMessage = "Only lowercase Alphabets and Numbers allowed.")]
            [Remote("ValidateIfUserWithUsernameExists", "User", ErrorMessage = "Пользователь с таким именем уже существует")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByNameAsync(Input.UserName);
                if (existingUser == null)
                {
                    existingUser = await _userManager.FindByEmailAsync(Input.UserName);
                    if (existingUser == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = Input.UserName,
                            Email = Input.Email,
                            UserType = UserTypesEnum.customer.ToString()
                        };
                        var result = await _userManager.CreateAsync(user, Input.Password);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User created a new Customer account with password.");

                            Customer customer = new Customer()
                            {
                                FirstName = Input.FirstName,
                                LastName = Input.LastName,
                                UserID = user.Id
                            };
                            CustomerService.Add(customer, user.Id);

                            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            //var callbackUrl = Url.Page(
                            //    "/Account/ConfirmEmail",
                            //    pageHandler: null,
                            //    values: new { userId = user.Id, code = code },
                            //    protocol: Request.Scheme);

                            //string body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                            //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", body);

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                            //return RedirectToAction("ThanksForRegistrationCustomer", "User");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Пользователь с такой почтой уже существует");
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
