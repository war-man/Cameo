﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class VideoRequestController : BaseController
    {
        private readonly IVideoRequestService VideoRequestService;
        private readonly IVideoRequestTypeService VideoRequestTypeService;
        private readonly ITalentService TalentService;
        private readonly ICustomerService CustomerService;

        public VideoRequestController(
            IVideoRequestService videoRequestService,
            IVideoRequestTypeService videoRequestTypeService,
            ITalentService talentService,
            ICustomerService customerService)
        {
            VideoRequestService = videoRequestService;
            VideoRequestTypeService = videoRequestTypeService;
            TalentService = talentService;
            CustomerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VideoRequestCreateVM modelVM)
        {
            var talent = TalentService.GetAvailableByID(modelVM.TalentID);

            if (ModelState.IsValid)
            {
                if (ValidateFromProperty(modelVM.From, modelVM.TypeID))
                {
                    
                    if (talent != null)
                    {
                        if (talent.Price == modelVM.Price)
                        {
                            try
                            {
                                var curUser = accountUtil.GetCurrentUser(User);
                                var curCustomer = CustomerService.GetByUserID(curUser.ID);

                                VideoRequest model = modelVM.ToModel(curCustomer);
                                VideoRequestService.Add(model, curUser.ID);

                                ViewBag.success = true;
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", ex.Message);
                            }
                        }
                        else
                            ModelState.AddModelError("", "Пока вы заполняли форму, Талант успел изменить цену");
                    }
                    else
                        ModelState.AddModelError("", "Талант не существует либо временно недоступен");
                }
                else
                    ModelState.AddModelError("From", "Укажите от кого");
            }
            else
                ModelState.AddModelError("", "Указаны некорректные данные");

            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();
            ViewData["talent"] = new TalentDetailsVM();

            return PartialView("_Create", modelVM);
        }

        public IActionResult ValidateFrom(string from, int typeID)
        {
            return Json(ValidateFromProperty(from, typeID));
        }

        private bool ValidateFromProperty(string from, int typeID)
        {
            bool result = true;
            if (typeID == (int)VideoRequestTypeEnum.someone && string.IsNullOrWhiteSpace(from))
                result = false;

            return result;
        }
    }
}