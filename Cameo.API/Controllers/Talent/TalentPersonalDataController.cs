using System;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "TalentOnly")]
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
            IAttachmentService attachmentService,
            ILogger<TalentPersonalDataController> logger)
        {
            TalentService = talentService;
            SocialAreaService = socialAreaService;
            AttachmentService = attachmentService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<TalentPersonalDataEditVM> Index()
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Talent model = TalentService.GetByUserID(curUser.ID);
                if (model == null)
                    throw new Exception("Талант не найден");
                if (model.AvatarID.HasValue)
                    model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

                TalentPersonalDataEditVM modelVM = new TalentPersonalDataEditVM(model);

                return Ok(modelVM);
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult Index([FromBody] TalentPersonalDataEditVM modelVM)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Talent model = TalentService.GetByID(modelVM.id);
                if (model == null || !model.UserID.Equals(curUser.ID))
                    throw new Exception("Талант не найден");

                if (ModelState.IsValid)
                {
                    model.FullName = modelVM.full_name;
                    //model.FirstName = modelVM.FirstName;
                    //model.LastName = modelVM.LastName;
                    model.Bio = modelVM.bio;
                    model.SocialAreaID = modelVM.social_area_id;
                    model.SocialAreaHandle = modelVM.social_area_handle;
                    model.FollowersCount = modelVM.followers_count;

                    TalentService.TransliterateFullname(model);

                    TalentService.Update(model, curUser.ID);

                    return Ok();
                }
                else
                    throw new Exception("Указаны некорректные данные");
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}