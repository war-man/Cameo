//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Cameo.Models;
//using Cameo.Services.Interfaces;
//using Cameo.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace Cameo.Controllers
//{
//    [Authorize(Policy = "TalentOnly")]
//    public class TalentCreditCardController : BaseController
//    {
//        private readonly ITalentService TalentService;

//        public TalentCreditCardController(
//            ITalentService talentService,
//            ILogger<TalentCreditCardController> logger)
//        {
//            TalentService = talentService;
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            var curUser = accountUtil.GetCurrentUser(User);
//            Talent model = TalentService.GetByUserID(curUser.ID);
//            if (model == null)
//                return NotFound();

//            TalentCreditCardEditVM modelVM = new TalentCreditCardEditVM(model);

//            return View(modelVM);
//        }

//        [HttpPost]
//        public IActionResult Index(TalentCreditCardEditVM modelVM)
//        {
//            var curUser = accountUtil.GetCurrentUser(User);
//            Talent model = TalentService.GetByID(modelVM.ID);
//            if (model == null || !model.UserID.Equals(curUser.ID))
//                return NotFound();

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    model.CreditCardNumber = modelVM.CreditCardNumber.Replace(" ", "");
//                    if (!string.IsNullOrWhiteSpace(modelVM.CreditCardExpire))
//                    {
//                        string[] tmp = modelVM.CreditCardExpire.Split('/');

//                        string monthString = tmp[0];
//                        string yearString = tmp[1];

//                        int month = int.Parse(monthString);
//                        int year = int.Parse(yearString) + 2000;

//                        DateTime creditCardExpireTmp = new DateTime(year, month, 1); //day does not play any role

//                        if (!ExpiresIn3Months(creditCardExpireTmp))
//                        {
//                            model.CreditCardExpire = creditCardExpireTmp;

//                            TalentService.Update(model, curUser.ID);

//                            ViewData["successfullySaved"] = true;
//                        }
//                        else
//                            ModelState.AddModelError("", "Срок годности карты истекает менее чем через 3 месяца");
//                    }
//                    else
//                        ModelState.AddModelError("", "Некорректный срок годности");
//                }
//                catch (Exception ex)
//                {
//                    throw new SystemException("Something went wrong while saving data.", ex);
//                }
//            }
//            else
//                ModelState.AddModelError("", "Неверные данные");

//            return View(modelVM);
//        }

//        private bool ExpiresIn3Months(DateTime creditCardExpire)
//        {
//            DateTime now = DateTime.Now;

//            int diffInYears = creditCardExpire.Year - now.Year;
//            if (diffInYears < 0)
//                return true;
//            else if (diffInYears == 0 || diffInYears == 1)
//            {
//                int diffInMonths = creditCardExpire.Month + (diffInYears * 12) - now.Month;
//                if (diffInMonths >= 3)
//                    return false;
//                else
//                    return true;
//            }
//            else if (diffInYears > 1)
//                return false;

//            return true;
//        }
//    }
//}