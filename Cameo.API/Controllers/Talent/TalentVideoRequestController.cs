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
    public class TalentVideoRequestController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly IVideoRequestSearchService SearchService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly ITalentBalanceService TalentBalanceService;

        public TalentVideoRequestController(
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
        public ActionResult<VideoRequestDetailsVM> Details(int id)
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

                VideoRequestDetailsVM modelVM = new VideoRequestDetailsVM(request);
                modelVM.cancel_btn_is_available = VideoRequestService.IsCancelable(request);

                modelVM.request_price = VideoRequestService.CalculateRequestPrice(request);
                modelVM.RequestPriceToStr();

                modelVM.remaining_price = VideoRequestService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
                modelVM.RemainingPriceToStr();

                return modelVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}