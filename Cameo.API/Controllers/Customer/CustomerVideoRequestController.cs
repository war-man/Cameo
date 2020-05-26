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
                return CustomBadRequest("Ваш заказ не найден");

            VideoRequestDetailsVM modelVM = new VideoRequestDetailsVM(request);
            modelVM.edit_btn_is_available = VideoRequestService.IsEditable(request);
            modelVM.cancel_btn_is_available = VideoRequestService.IsCancelable(request);

            modelVM.request_price = VideoRequestService.CalculateRequestPrice(request);
            modelVM.RequestPriceToStr();

            modelVM.remaining_price = VideoRequestService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
            modelVM.RemainingPriceToStr();

            //VideoRequestEditVM editModelVM = new VideoRequestEditVM(request);

            if (modelVM.edit_btn_is_available)
            {
                modelVM.video_request_edit_vm = new VideoRequestEditVM(request);
                modelVM.video_request_edit_vm.video_request_types = VideoRequestTypeService.GetAsSelectList();
            }

            return modelVM;
        }
    }
}