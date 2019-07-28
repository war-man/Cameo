using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class TalentController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly IAttachmentService AttachmentService;

        public TalentController(
            ITalentService talentService,
            IAttachmentService attachmentService)
        {
            TalentService = talentService;
            AttachmentService = attachmentService;
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
    }
}