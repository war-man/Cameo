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
    //[TalentAuthorization] - must be implemented
    public class TalentVideoRequestController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IVideoRequestSearchService SearchService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly ITalentBalanceService TalentBalanceService;

        public TalentVideoRequestController(
            ICustomerService customerService,
            IVideoRequestSearchService searchService,
            IVideoRequestService videoRequestService,
            ITalentBalanceService talentBalanceService)
        {
            CustomerService = customerService;
            SearchService = searchService;
            VideoRequestService = videoRequestService;
            TalentBalanceService = talentBalanceService;
        }

        public IActionResult Index(int? status = 0)
        {
            AppUserVM curUser = accountUtil.GetCurrentUser(User);
            ViewBag.userType = curUser.Type;
            //ViewBag.status = status;

            ViewData["statusID"] = status;

            return View("Index2");
        }

        public IActionResult Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);

            if (model == null || !VideoRequestService.BelongsToTalent(model, curUser.ID))
                return NotFound();

            VideoRequestDetailsVM modelVM = new VideoRequestDetailsVM(model);
            int balance = TalentBalanceService.GetBalance(model.Talent);
            modelVM.BalanceAllowsToConfirm = balance > 0;

            return View(modelVM);
        }
    }
}