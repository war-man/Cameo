﻿using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cameo.Common;

namespace Cameo.Controllers
{
    [Authorize]
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

        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();

            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            TalentShortInfoVM modelVM = new TalentShortInfoVM(model);

            return View(modelVM);
        }

        public IActionResult GetDashboardInfo()
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
                    Total = VideoRequestStatisticsService.GetAllCountByTalent(talent).ToString(numberFormat).Trim(),
                    WaitingForAnswer = VideoRequestStatisticsService.GetWaitingForAnswerCountByTalent(talent).ToString(numberFormat).Trim(),
                    WaitingForVideo = VideoRequestStatisticsService.GetWaitingForVideoCountByTalent(talent).ToString(numberFormat).Trim(),
                    WaitingForPayment = VideoRequestStatisticsService.GetWaitingForPaymentCountByTalent(talent).ToString(numberFormat).Trim(),
                    WaitingForPaymentConfirmation = VideoRequestStatisticsService.GetWaitingForPaymentConfirmationCountByTalent(talent).ToString(numberFormat).Trim(),
                    PaymentConfirmed = VideoRequestStatisticsService.GetPaymentConfirmedCountByTalent(talent).ToString(numberFormat).Trim(),
                    Earned = VideoRequestStatisticsService.GetEarnedByTalent(talent).ToString(numberFormat).Trim()
                };

                //return Ok(statisticsInfo);
                return PartialView("_Statistics", statisticsInfo);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        public string GetPrice()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return "0";
            else
                return model.Price.ToString(AppData.Configuration.NumberViewStringFormat).Trim();
        }

        [HttpPost]
        public IActionResult SetAvailability(bool availability)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent talent = TalentService.GetByUserID(curUser.ID);
            if (talent == null)
                return NotFound("Talent not found");

            TalentService.SetAvailability(talent, availability, curUser.ID);

            return Ok();
        }
    }
}