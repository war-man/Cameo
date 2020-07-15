using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IFirebaseService FirebaseService;

        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService CustomerService;
        private readonly ITalentService TalentService;
        private readonly ISocialAreaService SocialAreaService;

        public AccountController(
            IFirebaseService firebaseService,
            SignInManager<ApplicationUser> signInManager,
            //ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            ICustomerService customerService,
            ITalentService talentService,
            ISocialAreaService socialAreaService,
            ILogger<AccountController> logger)
        {
            FirebaseService = firebaseService;
            _signInManager = signInManager;
            //_logger = logger;
            _userManager = userManager;
            CustomerService = customerService;
            TalentService = talentService;
            SocialAreaService = socialAreaService;
            _logger = logger;
        }

        public IActionResult Login(string ReturnUrl)
        {
            ViewBag.returnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string firebaseUid, string returnUrl)
        {
            try
            {
                bool registration_is_required = true;

                string phoneNumber = await FirebaseService.GetPhoneNumberByUID(firebaseUid);
                var existingUser = GetUserByPhoneNumber(phoneNumber);
                if (existingUser != null)
                {
                    if (existingUser.UserType == UserTypesEnum.talent.ToString()
                        && !existingUser.TalentApprovedByAdmin)
                    {
                        throw new Exception("Ваша заявка как Таланта еще не одобрена. Мы с Вами свяжемся.");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(existingUser, true);
                    }

                    registration_is_required = false;
                }

                return Json(new { registration_is_required, returnUrl = CorrectifyReturnUrl(returnUrl) });
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        private string CorrectifyReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return returnUrl;
            else
                return "/";
        }

        private ApplicationUser GetUserByPhoneNumber(string phoneNumber)
        {
            return _userManager.Users.FirstOrDefault(m => m.PhoneNumber.Equals(phoneNumber));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string phoneNumber = await FirebaseService.GetPhoneNumberByUID(registerVM.FirebaseUid);
                    var existingUser = GetUserByPhoneNumber(phoneNumber);
                    if (existingUser == null)
                    {
                        existingUser = await _userManager.FindByNameAsync(registerVM.UserName);
                        if (existingUser == null)
                        {
                            var user = new ApplicationUser
                            {
                                UserName = registerVM.UserName,
                                PhoneNumber = phoneNumber,
                                FirebaseUid = registerVM.FirebaseUid,
                                UserType = UserTypesEnum.customer.ToString()
                            };

                            var result = await _userManager.CreateAsync(user);
                            if (result.Succeeded)
                            {
                                Customer customer = new Customer()
                                {
                                    FullName = registerVM.FullName,
                                    UserID = user.Id
                                };
                                CustomerService.Add(customer, user.Id);
                            }

                            await _signInManager.SignInAsync(user, true);

                            return Ok();
                        }
                        else
                            ModelState.AddModelError("", "Пользователь с таким именем уже зарегистрирован");
                    }
                    else
                        ModelState.AddModelError("", "Пользователь с таким телефоном уже зарегистрирован");
                }
                else
                    ModelState.AddModelError("", "Введены неверные данные");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            
            return BadRequest(ModelState);
        }

        public IActionResult AccessDenied()
        {
            throw new UnauthorizedAccessException("У Вас недостаточно прав для перехода по данной ссылке");
        }

        //Enrollment actions
        public IActionResult EnrollAsTalent()
        {
            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyIfUserCanEnroll(string firebaseUid)
        {
            try
            {
                string phoneNumber = await FirebaseService.GetPhoneNumberByUID(firebaseUid);
                var existingUser = GetUserByPhoneNumber(phoneNumber);
                if (existingUser != null)
                    throw new Exception("Пользователь с таким телефоном уже зарегистрирован");

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnrollAsTalent(EnrollAsTalentVM enrollAsTalentVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string phoneNumber = await FirebaseService.GetPhoneNumberByUID(enrollAsTalentVM.FirebaseUid);
                    var existingUser = GetUserByPhoneNumber(phoneNumber);
                    if (existingUser == null)
                    {
                        existingUser = await _userManager.FindByNameAsync(enrollAsTalentVM.UserName);
                        if (existingUser == null)
                        {
                            var user = new ApplicationUser
                            {
                                UserName = enrollAsTalentVM.UserName,
                                PhoneNumber = phoneNumber,
                                FirebaseUid = enrollAsTalentVM.FirebaseUid,
                                UserType = UserTypesEnum.talent.ToString(),
                                TalentConfirmationCode = CommonService.GenerateRandomNumerics(8)
                            };

                            var result = await _userManager.CreateAsync(user);
                            if (result.Succeeded)
                            {
                                Talent talent = new Talent()
                                {
                                    FullName = enrollAsTalentVM.FullName,
                                    UserID = user.Id,
                                    SocialAreaID = enrollAsTalentVM.SocialAreaID,
                                    SocialAreaHandle = enrollAsTalentVM.SocialAreaHandle,
                                    FollowersCount = enrollAsTalentVM.FollowersCount,
                                    IsAvailable = true
                                };
                                TalentService.Add(talent, user.Id);

                                return Ok(new {confirmationCode = user.TalentConfirmationCode });
                            }
                            else
                                ModelState.AddModelError("", "Не удалось зарегистрировать пользователя");
                        }
                        else
                            ModelState.AddModelError("", "Пользователь с таким именем уже зарегистрирован");
                    }
                    else
                        ModelState.AddModelError("", "Пользователь с таким телефоном уже зарегистрирован");
                }
                else
                    ModelState.AddModelError("", "Введены неверные данные");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest(ModelState);
        }
    }
}