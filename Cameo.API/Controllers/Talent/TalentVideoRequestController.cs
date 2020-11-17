using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class TalentVideoRequestController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly IVideoRequestSearchService SearchService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly ITalentBalanceService TalentBalanceService;
        private readonly IVideoRequestPriceCalculationsService VideoRequestPriceCalculationsService;

        public TalentVideoRequestController(
            ITalentService talentService,
            IVideoRequestSearchService searchService,
            IVideoRequestService videoRequestService,
            ITalentBalanceService talentBalanceService,
            IVideoRequestPriceCalculationsService videoRequestPriceCalculationsService,
            ILogger<TalentVideoRequestController> logger)
        {
            TalentService = talentService;
            SearchService = searchService;
            VideoRequestService = videoRequestService;
            TalentBalanceService = talentBalanceService;
            VideoRequestPriceCalculationsService = videoRequestPriceCalculationsService;
            _logger = logger;
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

                //requestVM.accept_btn_is_available = VideoRequestService.IsAcceptable(request);
                requestVM.cancel_btn_is_available = VideoRequestService.IsCancelable(request);

                //requestVM.request_price = VideoRequestPriceCalculationsService.CalculateRequestPrice(request);
                //requestVM.RequestPriceToStr();

                //requestVM.remaining_price = VideoRequestPriceCalculationsService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
                //requestVM.RemainingPriceToStr();

                return requestVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}