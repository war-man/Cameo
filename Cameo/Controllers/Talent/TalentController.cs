using System;
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

        public TalentController(
            ITalentService talentService,
            IAttachmentService attachmentService,
            IVideoRequestService videoRequestService)
        {
            TalentService = talentService;
            AttachmentService = attachmentService;
            VideoRequestService = videoRequestService;
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

        public string GetPrice()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return "0";
            else
                return model.Price.ToString();
        }

        public IActionResult GetDashboardInfo()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent talent = TalentService.GetByUserID(curUser.ID);
            if (talent == null)
                return NotFound("Talent not found");

            string numberFormat = AppData.Configuration.NumberViewStringFormat;

            string requestsTotal = VideoRequestService.GetAllCountByTalent(talent)
                .ToString(numberFormat);
            string requestsWaiting = VideoRequestService.GetWaitingCountByTalent(talent)
                .ToString(numberFormat);
            string requestsCompleted = VideoRequestService.GetCompletedCountByTalent(talent)
                .ToString(numberFormat);
            string requestsPaid = VideoRequestService.GetPaidCountByTalent(talent)
                .ToString(numberFormat);
            //string completenessPercentage = VideoRequestService.GetCompletenessPercentageByTalent(talent) + "%";
            string earned = VideoRequestService.GetEarnedByTalent(talent)
                .ToString(numberFormat) + " сум";

            return Ok(new {
                requestsTotal,
                requestsWaiting,
                requestsCompleted,
                requestsPaid,
                //completenessPercentage,
                earned
            });
        }
    }
}