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
    //[CustomerAuthorization] - must be implemented
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVideoRequestController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IVideoRequestSearchService SearchService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly IVideoRequestTypeService VideoRequestTypeService;

        public CustomerVideoRequestController(
            ICustomerService customerService,
            IVideoRequestSearchService searchService,
            IVideoRequestService videoRequestService,
            IVideoRequestTypeService videoRequestTypeService)
        {
            CustomerService = customerService;
            SearchService = searchService;
            VideoRequestService = videoRequestService;
            VideoRequestTypeService = videoRequestTypeService;
        }

        [HttpGet("{id}")]
        public ActionResult<VideoRequestDetailsVM> Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);

            if (request == null || !VideoRequestService.BelongsToCustomer(request, curUser.ID))
                return NotFound();

            VideoRequestDetailsVM modelVM = new VideoRequestDetailsVM(request);
            modelVM.EditBtnIsAvailable = VideoRequestService.IsEditable(request);
            modelVM.CancelBtnIsAvailable = VideoRequestService.IsCancelable(request);

            modelVM.RequestPrice = VideoRequestService.CalculateRequestPrice(request);
            modelVM.RequestPriceToStr();

            modelVM.RemainingPrice = VideoRequestService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
            modelVM.RemainingPriceToStr();

            //VideoRequestEditVM editModelVM = new VideoRequestEditVM(request);

            if (modelVM.EditBtnIsAvailable)
            {
                modelVM.videoRequestEditVM = new VideoRequestEditVM(request);
                modelVM.videoRequestEditVM.videoRequestTypes = VideoRequestTypeService.GetAsSelectList();
            }

            return modelVM;
        }
    }
}