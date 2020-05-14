using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Cameo.API.Utils;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoRequestController : BaseController
    {
        private readonly IVideoRequestService VideoRequestService;
        private readonly IVideoRequestTypeService VideoRequestTypeService;
        private readonly ITalentService TalentService;
        private readonly ICustomerService CustomerService;
        private readonly IHangfireService HangfireService;
        private readonly ITalentBalanceService TalentBalanceService;
        private readonly ICustomerBalanceService CustomerBalanceService;

        public VideoRequestController(
            IVideoRequestService videoRequestService,
            IVideoRequestTypeService videoRequestTypeService,
            ITalentService talentService,
            ICustomerService customerService,
            IHangfireService hangfireService,
            ITalentBalanceService talentBalanceService,
            ICustomerBalanceService customerBalanceService)
        {
            VideoRequestService = videoRequestService;
            VideoRequestTypeService = videoRequestTypeService;
            TalentService = talentService;
            CustomerService = customerService;
            HangfireService = hangfireService;
            TalentBalanceService = talentBalanceService;
            CustomerBalanceService = customerBalanceService;
        }

        [HttpGet("GetTalentRequestInfo/{talent_id}")]
        public ActionResult<TalentRequestInfoVM> GetTalentRequestInfo(int talent_id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            if (!AccountUtil.IsUserCustomer(curUser))
                return NotFound();

            var customer = CustomerService.GetByUserID(curUser.ID);
            if (customer == null)
                return BadRequest("You are not a customer");

            Talent talent = TalentService.GetAvailableByID(talent_id);
            if (talent == null)
                return NotFound();

            int customerBalance = CustomerBalanceService.GetBalance(customer);
            TalentRequestInfoVM talentRequestInfoVM = new TalentRequestInfoVM(talent, customerBalance);
            talentRequestInfoVM.RequestPrice = VideoRequestService.CalculateRequestPrice(talent);
            talentRequestInfoVM.RequestPriceToStr();

            return talentRequestInfoVM;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VideoRequestCreateVM modelVM)
        {
            int statusCode = 200;
            string errorMessage = null;

            try
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
                                    var curCustomer = CustomerService.GetByUserID(curUser.ID);
                                    int customerBalance = CustomerBalanceService.GetBalance(curCustomer);
                                    int requestPrice = VideoRequestService.CalculateRequestPrice(talent);

                                    if (customerBalance >= requestPrice)
                                    {
                                        try
                                        {
                                            CustomerBalanceService.TakeOffBalance(curCustomer, 100);

                                            VideoRequest model = modelVM.ToModel(curCustomer);

                                            //1. create model and send notification
                                            VideoRequestService.Add(model, curUser.ID);

                                            ////2. create hangfire RequestAnswerJobID and save it
                                            //model.RequestAnswerJobID = HangfireService.CreateJobForVideoRequestAnswerDeadline(model, curUser.ID);
                                            //create hangfire VideoJobID
                                            model.VideoJobID = HangfireService.CreateJobForVideoRequestVideoDeadline(model, curUser.ID);

                                            VideoRequestService.Update(model, curUser.ID);

                                            return Ok();
                                        }
                                        catch (Exception ex)
                                        {
                                            errorMessage = ex.Message;
                                        }
                                    }
                                    else
                                        errorMessage = "У Вас недостаточно средств на балансе";
                                }
                                else
                                    errorMessage = "Пока вы заполняли форму, Талант успел изменить цену";
                            }
                            else
                                errorMessage = "Талант не существует либо временно недоступен";
                        }
                        else
                            errorMessage = "Укажите от кого";
                    }
                    else
                        errorMessage = "Указаны некорректные данные";
                }
                else
                    errorMessage = "Таланты не могут заказывать видео. Зайдите как клиент.";

                if (!string.IsNullOrWhiteSpace(errorMessage))
                    statusCode = 400;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                statusCode = 500;

                Response.StatusCode = statusCode;
            }

            return BadRequest(errorMessage);
        }

        //[HttpPost]
        //public IActionResult Edit(VideoRequestEditVM modelVM)
        //{
        //    VideoRequest model = VideoRequestService.GetActiveByID(modelVM.ID);
        //    if (model == null)
        //        return NotFound();

        //    if (!VideoRequestService.RequestIsAllowedToBeEdited(model))
        //        return BadRequest("Данный запрос нельзя редактировать");

        //    if (ModelState.IsValid)
        //    {
        //        if (ValidateFromProperty(modelVM.From, modelVM.TypeID))
        //        {
        //            try
        //            {
        //                var curUser = accountUtil.GetCurrentUser(User);
        //                //var curCustomer = CustomerService.GetByUserID(curUser.ID);
        //                modelVM.UpdateModel(model);

        //                VideoRequestService.Edit(model, curUser.ID);

        //                return Ok();
        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError("", ex.Message);
        //            }
        //        }
        //        else
        //            ModelState.AddModelError("From", "Укажите от кого");
        //    }
        //    else
        //        ModelState.AddModelError("", "Указаны некорректные данные");

        //    ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

        //    return PartialView("_Edit", modelVM);


        //    ////---------
        //    //ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();
        //    //return Ok();

        //    //return BadRequest();
        //    ////return PartialView("_Edit", modelVM);

        //}

        #region Remote validation while creating
        //public IActionResult ValidateFrom(string from, int typeID)
        //{
        //    return Json(ValidateFromProperty(from, typeID));
        //}

        private bool ValidateFromProperty(string from, int typeID)
        {
            bool result = true;
            if (typeID == (int)VideoRequestTypeEnum.someone && string.IsNullOrWhiteSpace(from))
                result = false;

            return result;
        }
        #endregion

        //[HttpPost]
        //public IActionResult Cancel(int id)
        //{
        //    try
        //    {
        //        var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
        //        if (model == null)
        //            return NotFound();

        //        var curUser = accountUtil.GetCurrentUser(User);

        //        //cancel hangfire jobs
        //        //HangfireService.CancelJob(model.RequestAnswerJobID);
        //        HangfireService.CancelJob(model.VideoJobID);

        //        //cancel request/video
        //        VideoRequestService.Cancel(model, curUser.ID, curUser.Type);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        ////[HttpPost]
        ////public IActionResult Accept(int id)
        ////{
        ////    try
        ////    {
        ////        var model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
        ////        if (model == null)
        ////            return NotFound();

        ////        var curUser = accountUtil.GetCurrentUser(User);
        ////        if (!curUser.Type.Equals(UserTypesEnum.talent.ToString()))
        ////            throw new Exception("Вы не являетесь талантом");

        ////        //accept request/video
        ////        VideoRequestService.Accept(model, curUser.ID);

        ////        //cancel hangfire RequestAnswerJobID
        ////        HangfireService.CancelJob(model.RequestAnswerJobID);

        ////        //create hangfire VideoJobID
        ////        model.VideoJobID = HangfireService.CreateJobForVideoRequestVideoDeadline(model, curUser.ID);
        ////        VideoRequestService.Update(model, curUser.ID);

        ////        return Ok();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return BadRequest(ex);
        ////    }
        ////}

        //#region Video actions
        ////talent can upload and delete video any times before confirming (actions are in AttachmentController)
        ////however once confirmed, DateVideoConfirmed is set to DateTime.Now
        ////and the request is considered as finished by talent when he/she confirms

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
        //        HangfireService.CreateJobForPaymentReminder(model, curUser.ID);

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
        //#endregion
    }
}