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
    [Authorize]
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

            ViewData["balance"] = TalentBalanceService.GetBalanceIncludingReservations(model);
            ViewData["maxAvailablePrice"] = TalentBalanceService.CalculateMaxAvailablePriceForCameo(model)
                .ToString(AppData.Configuration.NumberStringFormat);

            return View(modelVM);
        }

        [HttpPost]
        public IActionResult Index(TalentPriceEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.ID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            int maxAvailablePrice = TalentBalanceService.CalculateMaxAvailablePriceForCameo(model);

            if (ModelState.IsValid)
            {
                if (modelVM.Price <= maxAvailablePrice)
                {
                    try
                    {
                        model.Price = modelVM.Price;

                        TalentService.Update(model, curUser.ID);

                        ViewData["successfullySaved"] = true;
                    }
                    catch (Exception ex)
                    {
                        throw new SystemException("Something went wrong while saving data.", ex);
                    }
                }
                else
                    ModelState.AddModelError("", "Ваш баланс не позволяет установить указанную цену");
            }
            else
                ModelState.AddModelError("", "Неверные данные");

            ViewData["balance"] = TalentBalanceService.GetBalanceIncludingReservations(model);
            ViewData["maxAvailablePrice"] = maxAvailablePrice.ToString(AppData.Configuration.NumberStringFormat);

            return View(modelVM);
        }

        public IActionResult CalculateMaxNumberOfPossibleRequests(int balance, int price)
        {
            return Ok(TalentBalanceService.CalculateMaxNumberOfPossibleRequests(balance, price));
        }
    }
}