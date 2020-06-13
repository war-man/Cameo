using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    public class TalentPriceController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ITalentBalanceService TalentBalanceService;

        public TalentPriceController(
            ITalentService talentService,
            ITalentBalanceService talentBalanceService)
        {
            TalentService = talentService;
            TalentBalanceService = talentBalanceService;
        }

        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();

            TalentPriceEditVM modelVM = new TalentPriceEditVM(model);

            //ViewData["balance"] = TalentBalanceService.GetBalance(model);
            //ViewData["maxAvailablePrice"] = TalentBalanceService.CalculateMaxAvailablePriceForCameo(model)
            //    .ToString(AppData.Configuration.NumberViewStringFormat);

            return View(modelVM);
        }

        [HttpPost]
        public IActionResult Index(TalentPriceEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.ID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            //int maxAvailablePrice = TalentBalanceService.CalculateMaxAvailablePriceForCameo(model);

            if (ModelState.IsValid)
            {
                //if (modelVM.Price <= maxAvailablePrice)
                //{
                    try
                    {
                        model.Price = modelVM.Price;

                        TalentService.Update(model, curUser.ID);

                        ViewData["successfullySaved"] = true;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Something went wrong while saving data: " + ex.Message);
                    }
                //}
                //else
                //    ModelState.AddModelError("", "Ваш баланс не позволяет установить указанную цену");
            }
            else
                ModelState.AddModelError("", "Неверные данные");

            //ViewData["balance"] = TalentBalanceService.GetBalance(model);
            //ViewData["maxAvailablePrice"] = maxAvailablePrice.ToString(AppData.Configuration.NumberViewStringFormat);

            return View(modelVM);
        }

        //public IActionResult CalculateMaxNumberOfPossibleRequests(int balance, int price)
        //{
        //    double commission = 0;
        //    double.TryParse(AppData.Configuration.WebsiteCommission.ToString(), out commission);
        //    if (commission <= 0)
        //        commission = 25;

        //    return Ok(TalentBalanceService.CalculateMaxNumberOfPossibleRequests(balance, price, commission));
        //}

        //public string GetPrice()
        //{
        //    var curUser = accountUtil.GetCurrentUser(User);
        //    Talent model = TalentService.GetByUserID(curUser.ID);
        //    if (model == null)
        //        return "0";
        //    else
        //        return model.Price.ToString(AppData.Configuration.NumberViewStringFormat);
        //}

        public string GetBalance()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return "0";
            else
                return TalentBalanceService.GetBalance(model)
                    .ToString(AppData.Configuration.NumberViewStringFormat);
        }
    }
}