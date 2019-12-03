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
        private readonly IHangfireService HangfireService;

        public VideoRequestController(
            IVideoRequestService videoRequestService,
            IVideoRequestTypeService videoRequestTypeService,
            ITalentService talentService,
            ICustomerService customerService,
            IHangfireService hangfireService)
        {
            VideoRequestService = videoRequestService;
            VideoRequestTypeService = videoRequestTypeService;
            TalentService = talentService;
            CustomerService = customerService;
            HangfireService = hangfireService;
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

                                //1. create model and send email
                                VideoRequestService.Add(model, curUser.ID);

                                //2. create hangfire RequestAnswerJobID and save it
                                model.RequestAnswerJobID = HangfireService.CreateJobForVideoRequestAnswerDeadline(model, curUser.ID);
                                VideoRequestService.Update(model, curUser.ID);

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

        #region Remote validation while creating
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
        #endregion

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            try
            {
                var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (model == null)
                    return NotFound();

                var curUser = accountUtil.GetCurrentUser(User);

                //cancel hangfire jobs
                HangfireService.CancelJob(model.RequestAnswerJobID);
                HangfireService.CancelJob(model.VideoJobID);

                //cancel request/video
                VideoRequestService.Cancel(model, curUser.ID, curUser.Type);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult Accept(int id)
        {
            try
            {
                var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (model == null)
                    return NotFound();

                var curUser = accountUtil.GetCurrentUser(User);
                if (!curUser.Type.Equals(UserTypesEnum.talent.ToString()))
                    throw new Exception("Вы не являетесь талантом");

                //accept request/video
                VideoRequestService.Accept(model, curUser.ID);

                //cancel hangfire RequestAnswerJobID
                HangfireService.CancelJob(model.RequestAnswerJobID);

                //create hangfire RequestAnswerJobID
                model.VideoJobID = HangfireService.CreateJobForVideoRequestVideoDeadline(model, curUser.ID);
                VideoRequestService.Update(model, curUser.ID);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region Video actions
        //talent can upload and delete video any times before confirming (actions are in AttachmentController)
        //however once confirmed, DateVideoConfirmed is set to DateTime.Now
        //and the request is considered as finished by talent when he/she confirms

        [HttpPost]
        public IActionResult ConfirmVideo(int id)
        {
            try
            {
                var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (model == null)
                    return NotFound();

                var curUser = accountUtil.GetCurrentUser(User);
                if (!curUser.Type.Equals(UserTypesEnum.talent.ToString()))
                    throw new Exception("Вы не являетесь талантом");

                //cancel hangfire RequestAnswerJobID
                HangfireService.CancelJob(model.RequestAnswerJobID);
                HangfireService.CancelJob(model.VideoJobID);

                //confirm request/video
                VideoRequestService.ConfirmVideo(model, curUser.ID);

                //create hangfire PaymentJobID
                model.PaymentJobID = HangfireService.CreateJobForVideoRequestPaymentDeadline(model, curUser.ID);
                VideoRequestService.Update(model, curUser.ID);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult MakePayment(int id)
        {
            try
            {
                var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (model == null)
                    return NotFound();

                var curUser = accountUtil.GetCurrentUser(User);
                if (!curUser.Type.Equals(UserTypesEnum.customer.ToString()))
                    throw new Exception("Вы не являетесь клиентом");

                //cancel hangfire RequestAnswerJobID, VideoJobID, PaymentJobID
                HangfireService.CancelJob(model.RequestAnswerJobID);
                HangfireService.CancelJob(model.VideoJobID);
                HangfireService.CancelJob(model.PaymentJobID);

                //confirm request/video
                VideoRequestService.MakePayment(model, curUser.ID);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}