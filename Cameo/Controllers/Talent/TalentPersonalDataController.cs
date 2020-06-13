using System;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cameo.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    public class TalentPersonalDataController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ISocialAreaService SocialAreaService;
        private IAttachmentService AttachmentService;

        public TalentPersonalDataController(
            ITalentService talentService,
            ISocialAreaService socialAreaService,
            IAttachmentService attachmentService)
        {
            TalentService = talentService;
            SocialAreaService = socialAreaService;
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

            TalentPersonalDataEditVM modelVM = new TalentPersonalDataEditVM(model);
            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }

        [HttpPost]
        public IActionResult Index(TalentPersonalDataEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.ID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.FullName = modelVM.FullName;
                    //model.FirstName = modelVM.FirstName;
                    //model.LastName = modelVM.LastName;
                    model.Bio = modelVM.Bio;
                    model.SocialAreaID = modelVM.SocialAreaID;
                    model.SocialAreaHandle = modelVM.SocialAreaHandle;
                    model.FollowersCount = modelVM.FollowersCount;

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

            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            modelVM.Avatar = new AttachmentDetailsVM(model.Avatar);

            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }
    }
}