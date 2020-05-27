using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    //[CustomerAuthorization] - must be implemented
    [Authorize]
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

        public IActionResult Index()
        {
            AppUserVM curUser = accountUtil.GetCurrentUser(User);
            ViewBag.userType = curUser.Type;

            return View();
        }

        public IActionResult Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);

            if (request == null || !VideoRequestService.BelongsToCustomer(request, curUser.ID))
                return CustomBadRequest("Ваш заказ не найден");

            VideoRequestDetailsForCustomerVM requestVM = new VideoRequestDetailsForCustomerVM(request);
            requestVM.EditBtnIsAvailable = VideoRequestService.IsEditable(request);
            requestVM.CancelBtnIsAvailable = VideoRequestService.IsCancelable(request);

            requestVM.RequestPrice = VideoRequestService.CalculateRequestPrice(request);
            requestVM.RequestPriceToStr();

            requestVM.RemainingPrice = VideoRequestService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
            requestVM.RemainingPriceToStr();

            requestVM.PaymentIsConfirmed = VideoRequestService.IsPaymentConfirmed(request);
            if (requestVM.PaymentIsConfirmed)
                requestVM.Video = new AttachmentDetailsVM(request.Video);

            //VideoRequestEditVM editModelVM = new VideoRequestEditVM(request);
            //ViewBag.editModelVM = editModelVM;
            //ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

            return View(requestVM);
        }
    }
}