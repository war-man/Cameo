using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class TalentPriceController : BaseController
    {
        private readonly ITalentService TalentService;
        //private readonly ITalentBalanceService TalentBalanceService;

        public TalentPriceController(
            ITalentService talentService,
            ITalentBalanceService talentBalanceService)
        {
            TalentService = talentService;
            //TalentBalanceService = talentBalanceService;
        }

        [HttpGet]
        public ActionResult<TalentPriceEditVM> Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return CustomBadRequest("Талант не найден");

            TalentPriceEditVM modelVM = new TalentPriceEditVM(model);

            return Ok(modelVM);
        }

        [HttpPost]
        public ActionResult Index([FromBody] TalentPriceEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.id);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return CustomBadRequest("Талант не найден");

            if (ModelState.IsValid && IsCorrectPriceProvided(modelVM.price))
            {
                try
                {
                    model.Price = modelVM.price;

                    TalentService.Update(model, curUser.ID);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return CustomBadRequest(ex);
                }
            }
            else
                return CustomBadRequest("Указаны некорректные данные");
        }

        private bool IsCorrectPriceProvided(int price)
        {
            int priceMin = AppData.Configuration.PriceMin;
            int priceMax = AppData.Configuration.PriceMax;
            int priceStep = AppData.Configuration.PriceStep;

            return (price >= priceMin && price <= priceMax && price % priceStep == 0);
        }
    }
}