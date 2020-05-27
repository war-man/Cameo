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
        public ActionResult<VideoRequestDetailsForCustomerVM> Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);

            if (request == null || !VideoRequestService.BelongsToCustomer(request, curUser.ID))
                return CustomBadRequest("Ваш заказ не найден");

            VideoRequestDetailsForCustomerVM requestVM = new VideoRequestDetailsForCustomerVM(request);
            requestVM.edit_btn_is_available = VideoRequestService.IsEditable(request);
            requestVM.cancel_btn_is_available = VideoRequestService.IsCancelable(request);

            requestVM.request_price = VideoRequestService.CalculateRequestPrice(request);
            requestVM.RequestPriceToStr();

            requestVM.remaining_price = VideoRequestService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
            requestVM.RemainingPriceToStr();

            requestVM.payment_is_confirmed = VideoRequestService.IsPaymentConfirmed(request);
            if (requestVM.payment_is_confirmed)
                requestVM.video = new AttachmentDetailsVM(request.Video);

            if (requestVM.edit_btn_is_available)
            {
                requestVM.video_request_edit_vm = new VideoRequestEditVM(request);
                requestVM.video_request_edit_vm.video_request_types = VideoRequestTypeService.GetAsSelectList();
            }

            //VideoRequestEditVM editModelVM = new VideoRequestEditVM(request);

            return requestVM;
        }
    }
}