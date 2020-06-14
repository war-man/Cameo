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
        private readonly IVideoRequestPriceCalculationsService VideoRequestPriceCalculationsService;

        public VideoRequestController(
            IVideoRequestService videoRequestService,
            IVideoRequestTypeService videoRequestTypeService,
            ITalentService talentService,
            ICustomerService customerService,
            IHangfireService hangfireService,
            ITalentBalanceService talentBalanceService,
            ICustomerBalanceService customerBalanceService,
            IVideoRequestPriceCalculationsService videoRequestPriceCalculationsService)
        {
            VideoRequestService = videoRequestService;
            VideoRequestTypeService = videoRequestTypeService;
            TalentService = talentService;
            CustomerService = customerService;
            HangfireService = hangfireService;
            TalentBalanceService = talentBalanceService;
            CustomerBalanceService = customerBalanceService;
            VideoRequestPriceCalculationsService = videoRequestPriceCalculationsService;
        }

        [HttpGet("GetTalentRequestInfo/{talent_id}")]
        public ActionResult<TalentRequestInfoVM> GetTalentRequestInfo(int talent_id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            if (!AccountUtil.IsUserCustomer(curUser))
                return CustomBadRequest("Только клиенты могут смотреть эту информацию");

            var customer = CustomerService.GetByUserID(curUser.ID);
            if (customer == null)
                return CustomBadRequest("Вы не являетесь клиентом");

            Talent talent = TalentService.GetAvailableByID(talent_id);
            if (talent == null)
                return CustomBadRequest("Талант не найден");

            int customerBalance = CustomerBalanceService.GetBalance(customer);
            TalentRequestInfoVM talentRequestInfoVM = new TalentRequestInfoVM(talent, customerBalance);
            talentRequestInfoVM.request_price = VideoRequestPriceCalculationsService.CalculateRequestPrice(talent);
            talentRequestInfoVM.RequestPriceToStr();

            return talentRequestInfoVM;
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] VideoRequestCreateVM modelVM)
        {
            string errorMessage = null;

            var curUser = accountUtil.GetCurrentUser(User);
            var talent = TalentService.GetAvailableByID(modelVM.talent_id);

            if (AccountUtil.IsUserCustomer(curUser))
            {
                if (ModelState.IsValid)
                {
                    if (ValidateFromProperty(modelVM.from, modelVM.type_id))
                    {
                        if (talent != null)
                        {
                            if (talent.Price == modelVM.price)
                            {
                                var curCustomer = CustomerService.GetByUserID(curUser.ID);
                                int customerBalance = CustomerBalanceService.GetBalance(curCustomer);
                                int requestPrice = VideoRequestPriceCalculationsService.CalculateRequestPrice(talent);

                                if (customerBalance >= requestPrice)
                                {
                                    try
                                    {
                                        CustomerBalanceService.TakeOffBalance(curCustomer, requestPrice);

                                        VideoRequest newRequest = modelVM.ToModel(curCustomer);

                                        //1. create model and send notification
                                        VideoRequestService.Add(newRequest, curUser.ID);

                                        ////2. create hangfire RequestAnswerJobID and save it
                                        newRequest.RequestAnswerJobID = HangfireService.CreateJobForVideoRequestAnswerDeadline(newRequest, curUser.ID);
                                        //create hangfire VideoJobID
                                        //model.VideoJobID = HangfireService.CreateJobForVideoRequestVideoDeadline(model, curUser.ID);

                                        VideoRequestService.Update(newRequest, curUser.ID);

                                        return Ok(new { id = newRequest.ID });
                                    }
                                    catch (Exception ex)
                                    {
                                        return CustomBadRequest(ex);
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
                return CustomBadRequest(errorMessage);

            errorMessage = "Что-то пошло не так";
            return CustomBadRequest(errorMessage);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost("Edit/{id}")]
        public IActionResult Edit(int id, [FromBody] VideoRequestEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);

            VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
            if (request == null || !VideoRequestService.BelongsToCustomer(request, curUser.ID))
                //return NotFound();
                return CustomBadRequest("Заказ не найден");

            if (!VideoRequestService.IsEditable(request))
                return CustomBadRequest("Данный запрос нельзя редактировать");

            if (ModelState.IsValid)
            {
                if (ValidateFromProperty(modelVM.from, modelVM.type_id))
                {
                    try
                    {
                        modelVM.UpdateModel(request);

                        VideoRequestService.Edit(request, curUser.ID);

                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        //ModelState.AddModelError("", ex.Message);
                        return CustomBadRequest(ex);
                    }
                }
                else
                    //ModelState.AddModelError("From", "Укажите от кого");
                    return CustomBadRequest("Укажите от кого");
            }
            else
                //ModelState.AddModelError("", "Указаны некорректные данные");
                return CustomBadRequest("Указаны некорректные данные");
        }

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

        [HttpPost("Cancel/{id}")]
        public IActionResult Cancel(int id)
        {
            try
            {
                var request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (request == null)
                    //return NotFound();
                    return CustomBadRequest("Заказ не найден");

                var curUser = accountUtil.GetCurrentUser(User);

                //cancel request/video
                VideoRequestService.Cancel(request, curUser.ID, curUser.Type);

                //cancel hangfire jobs
                HangfireService.CancelJob(request.RequestAnswerJobID);
                HangfireService.CancelJob(request.VideoJobID);

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [Authorize(Policy = "TalentOnly")]
        [HttpPost("Accept/{id}")]
        public IActionResult Accept(int id)
        {
            try
            {
                var request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (request == null)
                    throw new Exception("Заказ не найден");

                var curUser = accountUtil.GetCurrentUser(User);
                if (!AccountUtil.IsUserTalent(curUser))
                    throw new Exception("Вы не являетесь талантом");

                //accept request/video
                VideoRequestService.Accept(request, curUser.ID);

                //cancel hangfire RequestAnswerJobID
                HangfireService.CancelJob(request.RequestAnswerJobID);

                //create hangfire VideoJobID
                request.VideoJobID = HangfireService.CreateJobForVideoRequestVideoDeadline(request, curUser.ID);
                VideoRequestService.Update(request, curUser.ID);

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        #region Video actions
        //talent can upload and delete video any times before confirming (actions are in AttachmentController)
        //however once confirmed, DateVideoConfirmed is set to DateTime.Now
        //and the request is considered as finished by talent when he/she confirms

        [Authorize(Policy = "TalentOnly")]
        [HttpPost("ConfirmVideo/{request_id}")]
        public IActionResult ConfirmVideo(int request_id)
        {
            try
            {
                var request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(request_id);
                if (request == null)
                    throw new Exception("Заказ не найден");

                var curUser = accountUtil.GetCurrentUser(User);
                if (!curUser.Type.Equals(UserTypesEnum.talent.ToString()))
                    throw new Exception("Вы не являетесь талантом");

                //int balance = TalentBalanceService.GetBalance(request.Talent);
                //if (balance <= 0)
                //    throw new Exception("У Вас недостаточно средств, чтобы подтвердить запрос");

                //confirm request/video
                VideoRequestService.ConfirmVideo(request, curUser.ID);
                
                //cancel hangfire jobs
                HangfireService.CancelJob(request.RequestAnswerJobID);
                HangfireService.CancelJob(request.VideoJobID);

                ////create hangfire PaymentReminderJobID
                //HangfireService.CreateJobForPaymentReminder(model, curUser.ID);

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [Authorize(Policy = "TalentOnly")]
        [HttpPost("ConfirmPayment/{request_id}")]
        public IActionResult ConfirmPayment(int request_id)
        {
            try
            {
                var request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(request_id);
                if (request == null)
                    throw new Exception("Заказ не найден");

                var curUser = accountUtil.GetCurrentUser(User);
                if (!curUser.Type.Equals(UserTypesEnum.talent.ToString()))
                    throw new Exception("Вы не являетесь талантом");

                //int balance = TalentBalanceService.GetBalance(request.Talent);
                //if (balance <= 0)
                //    throw new Exception("У Вас недостаточно средств, чтобы подтвердить запрос");

                //confirm request/video
                VideoRequestService.ConfirmPayment(request, curUser.ID);

                //cancel hangfire jobs
                HangfireService.CancelJob(request.RequestAnswerJobID);
                HangfireService.CancelJob(request.VideoJobID);
                HangfireService.CancelJob(request.PaymentConfirmationJobID);

                ////create hangfire PaymentReminderJobID
                //HangfireService.CreateJobForPaymentReminder(model, curUser.ID);

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

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