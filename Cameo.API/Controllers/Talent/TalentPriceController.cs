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
using Microsoft.Extensions.Logging;

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
            ITalentBalanceService talentBalanceService,
            ILogger<TalentPriceController> logger)
        {
            TalentService = talentService;
            //TalentBalanceService = talentBalanceService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<TalentPriceEditVM> Index()
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Talent model = TalentService.GetByUserID(curUser.ID);
                if (model == null)
                    throw new Exception("Талант не найден");

                TalentPriceEditVM modelVM = new TalentPriceEditVM(model);

                return Ok(modelVM);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult Index([FromBody] TalentPriceEditVM modelVM)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Talent model = TalentService.GetByID(modelVM.id);
                if (model == null || !model.UserID.Equals(curUser.ID))
                    throw new Exception("Талант не найден");

                if (ModelState.IsValid && IsCorrectPriceProvided(modelVM.price))
                {
                    model.Price = modelVM.price;

                    TalentService.Update(model, curUser.ID);

                    return Ok();
                }
                else
                    throw new Exception("Указаны некорректные данные");
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
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