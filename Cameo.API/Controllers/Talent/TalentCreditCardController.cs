using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class TalentCreditCardController : BaseController
    {
        private readonly ITalentService TalentService;

        public TalentCreditCardController(ITalentService talentService)
        {
            TalentService = talentService;
        }

        [HttpGet]
        public ActionResult<TalentCreditCardEditVM> Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return CustomBadRequest("Талант не найден");

            TalentCreditCardEditVM modelVM = new TalentCreditCardEditVM(model);

            return Ok(modelVM);
        }

        [HttpPost]
        public ActionResult Index([FromBody] TalentCreditCardEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.id);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return CustomBadRequest("Талант не найден");

            if (ModelState.IsValid)
            {
                try
                {
                    model.CreditCardNumber = modelVM.credit_card_number.Replace(" ", "");
                    model.CreditCardHolder = modelVM.credit_card_holder;

                    if (!string.IsNullOrWhiteSpace(modelVM.credit_card_expire))
                    {
                        string[] tmp = modelVM.credit_card_expire.Split('/');

                        string monthString = tmp[0];
                        string yearString = tmp[1];

                        int month = int.Parse(monthString);
                        int year = int.Parse(yearString) + 2000;

                        DateTime creditCardExpireTmp = new DateTime(year, month, 1); //day does not play any role

                        if (!ExpiresIn3Months(creditCardExpireTmp))
                        {
                            model.CreditCardExpire = creditCardExpireTmp;

                            TalentService.Update(model, curUser.ID);

                            return Ok();
                        }
                        else
                            return CustomBadRequest("Срок годности карты истекает менее чем через 3 месяца");
                        //ModelState.AddModelError("", "Срок годности карты истекает менее чем через 3 месяца");
                    }
                    else
                        return CustomBadRequest("Некорректный срок годности");
                    //ModelState.AddModelError("", "Некорректный срок годности");
                }
                catch (Exception ex)
                {
                    return CustomBadRequest(ex);
                }
            }
            else
                return CustomBadRequest("Указаны некорректные данные");
        }

        private bool ExpiresIn3Months(DateTime creditCardExpire)
        {
            DateTime now = DateTime.Now;

            int diffInYears = creditCardExpire.Year - now.Year;
            if (diffInYears < 0)
                return true;
            else if (diffInYears == 0 || diffInYears == 1)
            {
                int diffInMonths = creditCardExpire.Month + (diffInYears * 12) - now.Month;
                if (diffInMonths >= 3)
                    return false;
                else
                    return true;
            }
            else if (diffInYears > 1)
                return false;

            return true;
        }
    }
}