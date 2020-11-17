using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Cameo.Utils;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly ITalentBalanceService TalentBalanceService;
        private readonly ICustomerBalanceService CustomerBalanceService;
        private readonly IVideoRequestPriceCalculationsService VideoRequestPriceCalculationsService;

        public VideoRequestController(
            IVideoRequestService videoRequestService,
            IVideoRequestTypeService videoRequestTypeService,
            ITalentService talentService,
            ICustomerService customerService,
            IHangfireService hangfireService,
            ITalentBalanceService talentBalanceService,
            ICustomerBalanceService customerBalanceService,
            IVideoRequestPriceCalculationsService videoRequestPriceCalculationsService,
            ILogger<VideoRequestController> logger)
        {
            VideoRequestService = videoRequestService;
            VideoRequestTypeService = videoRequestTypeService;
            TalentService = talentService;
            CustomerService = customerService;
            HangfireService = hangfireService;
            TalentBalanceService = talentBalanceService;
            CustomerBalanceService = customerBalanceService;
            VideoRequestPriceCalculationsService = videoRequestPriceCalculationsService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "CustomerOnly")]
        public IActionResult Create(string username)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            if (!AccountUtil.IsUserCustomer(curUser))
                throw new Exception("Вы не являетесь клиентом");

            var customer = CustomerService.GetByUserID(curUser.ID);
            if (customer == null)
                throw new Exception("Вы не являетесь клиентом");

            Talent talent = TalentService.GetActiveByUsername(username);
            if (talent == null)
                throw new Exception("Талант не найден");

            TalentDetailsVM talentVM = new TalentDetailsVM(talent);
            ViewData["talent"] = talentVM;

            var createVM = new VideoRequestCreateVM()
            {
                TypeID = (int)VideoRequestTypeEnum.someone
            };

            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

            //int customerBalance = CustomerBalanceService.GetBalance(customer);
            //ViewData["customerBalance"] = customerBalance;
            //string numberFormat = AppData.Configuration.NumberViewStringFormat;
            //ViewData["customerBalanceStr"] = customerBalance
            //    .ToString(numberFormat)
            //    .Trim();

            //if (customerBalance < talentVM.Price)
            //{
            //    int paymentAmount = talentVM.RequestPrice - customerBalance;
            //    string returnUrl = "https://helloo.uz/VideoRequest/Create?username=" + username;

            //    ViewData["PaymentAmount"] = paymentAmount;
            //    ViewData["ClickPaymentButtonUrl"] = CustomerBalanceService
            //        .GenerateClickPaymentButtonUrl(customer.AccountNumber, paymentAmount, returnUrl);
            //}

            return View(createVM);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost]
        public IActionResult Create(VideoRequestCreateVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            var talent = TalentService.GetAvailableByID(modelVM.TalentID);

            if (AccountUtil.IsUserCustomer(curUser))
            {
                if (ModelState.IsValid)
                {
                    if (ValidateFromProperty(modelVM.From, modelVM.TypeID))
                    {
                        if (talent != null)
                        {
                            if (talent.Price == modelVM.Price)
                            {
                                //var curCustomer = CustomerService.GetByUserID(curUser.ID);
                                //int customerBalance = CustomerBalanceService.GetBalance(curCustomer);
                                //int requestPrice = VideoRequestPriceCalculationsService.CalculateRequestPrice(talent);

                                //if (customerBalance >= requestPrice)
                                //{
                                //    try
                                //    {
                                //        CustomerBalanceService.TakeOffBalance(curCustomer, requestPrice);

                                //        VideoRequest model = modelVM.ToModel(curCustomer);

                                //        //1. create model and send notification
                                //        VideoRequestService.Add(model, curUser.ID);

                                //        //2. create hangfire RequestAnswerJobID and save it
                                //        model.RequestAnswerJobID = HangfireService.CreateJobForVideoRequestAnswerDeadline(model, curUser.ID);
                                //        ////create hangfire VideoJobID
                                //        //model.VideoJobID = HangfireService.CreateJobForVideoRequestVideoDeadline(model, curUser.ID);

                                //        VideoRequestService.Update(model, curUser.ID);

                                //        ViewBag.success = true;
                                //        ViewBag.requestID = model.ID;
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        ModelState.AddModelError("", ex.Message);
                                //    }
                                //}
                                //else
                                //    ModelState.AddModelError("", "У Вас недостаточно средств на балансе");
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
            }
            else
                ModelState.AddModelError("", "Таланты не могут заказывать видео. Зайдите как клиент.");

            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

            TalentDetailsVM talentVM = new TalentDetailsVM(talent);
            ViewData["talent"] = talentVM;

            return PartialView("_Create", modelVM);
        }

        [Authorize(Policy = "CustomerOnly")]
        public IActionResult Edit(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);

            VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
            if (request == null || !VideoRequestService.BelongsToCustomer(request, curUser.ID))
                throw new Exception("Заказ не найден");

            if (!VideoRequestService.IsEditable(request))
                throw new Exception("Данный заказ нельзя редактировать");

            VideoRequestEditVM editVM = new VideoRequestEditVM(request);

            Talent talent = TalentService.GetActiveByID(request.TalentID);
            if (talent == null)
                return NotFound();

            TalentDetailsVM talentVM = new TalentDetailsVM(talent);
            //talentVM.RequestPrice = VideoRequestService.CalculateRequestPrice(talent);
            //talentVM.RequestPriceToStr();
            ViewData["talent"] = talentVM;
            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();
            //ViewData["customerBalance"] = CustomerBalanceService.GetBalance(customer);

            return View(editVM);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost]
        public IActionResult Edit(VideoRequestEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);

            VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(modelVM.ID);
            if (request == null || !VideoRequestService.BelongsToCustomer(request, curUser.ID))
                throw new Exception("Заказ не найден");

            if (!VideoRequestService.IsEditable(request))
                throw new Exception("Данный запрос нельзя редактировать");

            if (ModelState.IsValid)
            {
                if (ValidateFromProperty(modelVM.From, modelVM.TypeID))
                {
                    try
                    {
                        modelVM.UpdateModel(request);

                        VideoRequestService.Edit(request, curUser.ID);

                        ViewBag.success = true;
                        ViewBag.requestID = request.ID;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                else
                    ModelState.AddModelError("From", "Укажите от кого");
            }
            else
                ModelState.AddModelError("", "Указаны некорректные данные");

            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

            return PartialView("_Edit", modelVM);


            ////---------
            //ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();
            //return Ok();

            //return BadRequest();
            ////return PartialView("_Edit", modelVM);

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

        //ajax
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            try
            {
                var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (model == null)
                    throw new Exception("Заказ не найден");

                var curUser = accountUtil.GetCurrentUser(User);

                //cancel request/video
                VideoRequestService.Cancel(model, curUser.ID, curUser.Type);

                //cancel hangfire jobs
                HangfireService.CancelJob(model.RequestAnswerJobID);
                HangfireService.CancelJob(model.VideoJobID);

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        //[HttpPost]
        //public IActionResult Accept(int id)
        //{
        //    try
        //    {
        //        var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
        //        if (model == null)
        //            return NotFound();

        //        var curUser = accountUtil.GetCurrentUser(User);
        //        if (!curUser.Type.Equals(UserTypesEnum.talent.ToString()))
        //            throw new Exception("Вы не являетесь талантом");

        //        //accept request/video
        //        VideoRequestService.Accept(model, curUser.ID);

        //        //cancel hangfire RequestAnswerJobID
        //        HangfireService.CancelJob(model.RequestAnswerJobID);

        //        //create hangfire VideoJobID
        //        model.VideoJobID = HangfireService.CreateJobForVideoRequestVideoDeadline(model, curUser.ID);
        //        VideoRequestService.Update(model, curUser.ID);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        #region Video actions
        //talent can upload and delete video any times before confirming (actions are in AttachmentController)
        //however once confirmed, DateVideoConfirmed is set to DateTime.Now
        //and the request is considered as finished by talent when he/she confirms

        //[Authorize(Policy = "TalentOnly")]
        //[HttpPost]
        //public IActionResult ConfirmVideo(int id)
        //{
        //    try
        //    {
        //        var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
        //        if (model == null)
        //            return NotFound();

        //        var curUser = accountUtil.GetCurrentUser(User);
        //        if (!curUser.Type.Equals(UserTypesEnum.talent.ToString()))
        //            throw new Exception("Вы не являетесь талантом");

        //        int balance = TalentBalanceService.GetBalance(model.Talent);
        //        if (balance <= 0)
        //            throw new Exception("У Вас недостаточно средств, чтобы подтвердить запрос");

        //        //cancel hangfire RequestAnswerJobID
        //        //HangfireService.CancelJob(model.RequestAnswerJobID);
        //        HangfireService.CancelJob(model.VideoJobID);

        //        //confirm request/video
        //        VideoRequestService.ConfirmVideo(model, curUser.ID);

        //        //create hangfire PaymentReminderJobID
        //        //HangfireService.CreateJobForPaymentReminder(model, curUser.ID);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        //[HttpPost]
        //public IActionResult MakePayment(int id)
        //{
        //    try
        //    {
        //        var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
        //        if (model == null)
        //            return NotFound();

        //        var curUser = accountUtil.GetCurrentUser(User);
        //        if (!curUser.Type.Equals(UserTypesEnum.customer.ToString()))
        //            throw new Exception("Вы не являетесь клиентом");

        //        //cancel hangfire RequestAnswerJobID, VideoJobID, PaymentJobID
        //        //HangfireService.CancelJob(model.RequestAnswerJobID);
        //        //HangfireService.CancelJob(model.VideoJobID);
        //        //HangfireService.CancelJob(model.PaymentJobID);
        //        HangfireService.CancelJob(model.PaymentReminderJobID);

        //        //confirm request/video
        //        VideoRequestService.MakePayment(model, curUser.ID);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        #endregion
    }
}