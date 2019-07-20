using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class TalentController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ISocialAreaService SocialAreaService;

        public TalentController(
            ITalentService talentService,
            ISocialAreaService socialAreaService)
        {
            TalentService = talentService;
            SocialAreaService = socialAreaService;
        }

        public IActionResult PersonalData()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();

            TalentEditVM modelVM = new TalentEditVM(model);
            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }

        [HttpPost]
        public IActionResult PersonalData(TalentEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Talent model = TalentService.GetByID(modelVM.ID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.FirstName = modelVM.FirstName;
                    model.LastName = modelVM.LastName;
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

            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }
    }
}