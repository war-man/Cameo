﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public TalentPriceController(ITalentService talentService)
        {
            TalentService = talentService;
        }

        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();

            TalentPriceEditVM modelVM = new TalentPriceEditVM(model);

            return View(modelVM);
        }

        [HttpPost]
        public IActionResult Index(TalentPriceEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.ID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            if (ModelState.IsValid)
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
                ModelState.AddModelError("", "Неверные данные");

            return View(modelVM);
        }
    }
}