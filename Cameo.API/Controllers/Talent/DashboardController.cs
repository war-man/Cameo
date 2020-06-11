using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Cameo.API.Controllers
{
    //[TalentAuthorization] - must be implemented
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly IVideoRequestSearchService SearchService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly ITalentBalanceService TalentBalanceService;

        public DashboardController(
            ITalentService talentService,
            IVideoRequestSearchService searchService,
            IVideoRequestService videoRequestService,
            ITalentBalanceService talentBalanceService)
        {
            TalentService = talentService;
            SearchService = searchService;
            VideoRequestService = videoRequestService;
            TalentBalanceService = talentBalanceService;
        }

        //public IActionResult Index(int? status = 0)
        //{
        //    AppUserVM curUser = accountUtil.GetCurrentUser(User);
        //    ViewBag.userType = curUser.Type;
        //    //ViewBag.status = status;

        //    ViewData["statusID"] = status;

        //    return View("Index2");
        //}

        [HttpGet("{id}")]
        public ActionResult<VideoRequestDetailsForTalentVM> Details(int id)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);

                if (request == null || !VideoRequestService.BelongsToTalent(request, curUser.ID))
                    throw new Exception("Ваш заказ не найден");

                if (request.ViewedByTalent == false)
                {
                    request.ViewedByTalent = true;
                    VideoRequestService.Update(request, curUser.ID);
                }

                VideoRequestDetailsForTalentVM requestVM = new VideoRequestDetailsForTalentVM(request);
                requestVM.cancel_btn_is_available = VideoRequestService.IsCancelable(request);

                requestVM.request_price = VideoRequestService.CalculateRequestPrice(request);
                requestVM.RequestPriceToStr();

                requestVM.remaining_price = VideoRequestService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
                requestVM.RemainingPriceToStr();

                return requestVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}