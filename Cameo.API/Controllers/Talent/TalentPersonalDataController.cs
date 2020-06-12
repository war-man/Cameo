using System;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cameo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet]
        public ActionResult<TalentPersonalDataEditVM> Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return CustomBadRequest("Талант не найден");
            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            TalentPersonalDataEditVM modelVM = new TalentPersonalDataEditVM(model);

            return Ok(modelVM);
        }

        [HttpPost]
        public ActionResult Index([FromBody] TalentPersonalDataEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.id);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return CustomBadRequest("Талант не найден");

            if (ModelState.IsValid)
            {
                try
                {
                    model.FullName = modelVM.full_name;
                    //model.FirstName = modelVM.FirstName;
                    //model.LastName = modelVM.LastName;
                    model.Bio = modelVM.bio;
                    model.SocialAreaID = modelVM.SocialAreaID;
                    model.SocialAreaHandle = modelVM.SocialAreaHandle;
                    model.FollowersCount = modelVM.FollowersCount;

                    TalentService.Update(model, curUser.ID);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return CustomBadRequest(ex);
                }
            }
            else
                return CustomBadRequest("Указаны некорректные данные");
        }
    }
}