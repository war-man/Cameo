using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cameo.Common;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class TalentController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly IAttachmentService AttachmentService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly IVideoRequestStatisticsService VideoRequestStatisticsService;

        public TalentController(
            ITalentService talentService,
            IAttachmentService attachmentService,
            IVideoRequestService videoRequestService,
            IVideoRequestStatisticsService videoRequestStatisticsService)
        {
            TalentService = talentService;
            AttachmentService = attachmentService;
            VideoRequestService = videoRequestService;
            VideoRequestStatisticsService = videoRequestStatisticsService;
        }

        [HttpGet]
        public ActionResult<TalentShortInfoVM> Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return CustomBadRequest("Талант не найден");

            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            TalentShortInfoVM modelVM = new TalentShortInfoVM(model);

            return Ok(modelVM);
        }

        [HttpGet("DashboardInfo")]
        public ActionResult<TalentStatisticsVM> GetDashboardInfo()
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Talent talent = TalentService.GetByUserID(curUser.ID);
                if (talent == null)
                    throw new Exception("Талант не найден");

                string numberFormat = AppData.Configuration.NumberViewStringFormat;

                TalentStatisticsVM statisticsInfo = new TalentStatisticsVM()
                {
                    total = VideoRequestStatisticsService.GetAllCountByTalent(talent).ToString(numberFormat).Trim(),
                    not_completed = VideoRequestStatisticsService.GetNotCompletedCountByTalent(talent).ToString(numberFormat).Trim(),
                    waiting_for_answer = VideoRequestStatisticsService.GetWaitingForAnswerCountByTalent(talent).ToString(numberFormat).Trim(),
                    waiting_for_video = VideoRequestStatisticsService.GetWaitingForVideoCountByTalent(talent).ToString(numberFormat).Trim(),
                    waiting_for_payment = VideoRequestStatisticsService.GetWaitingForPaymentCountByTalent(talent).ToString(numberFormat).Trim(),
                    waiting_for_payment_confirmation = VideoRequestStatisticsService.GetWaitingForPaymentConfirmationCountByTalent(talent).ToString(numberFormat).Trim(),
                    payment_confirmed = VideoRequestStatisticsService.GetPaymentConfirmedCountByTalent(talent).ToString(numberFormat).Trim(),
                    earned = VideoRequestStatisticsService.GetEarnedByTalent(talent).ToString(numberFormat).Trim()
                };

                return Ok(statisticsInfo);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpGet("GetPrice")]
        public ActionResult<string> GetPrice()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return Ok("0");
            else
                return Ok(model.Price.ToString(AppData.Configuration.NumberViewStringFormat).Trim());
        }

        [HttpPost("SetAvailability")]
        public ActionResult SetAvailability(bool availability)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Talent talent = TalentService.GetByUserID(curUser.ID);
                if (talent == null)
                    throw new Exception("Талант не найден");

                TalentService.SetAvailability(talent, availability, curUser.ID);

                return Ok();
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}