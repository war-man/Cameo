using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    public class CustomerVideoRequestController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IVideoRequestSearchService SearchService;
        private readonly IVideoRequestService VideoRequestService;
        private readonly IVideoRequestTypeService VideoRequestTypeService;
        private readonly IFirebaseRegistrationTokenService FirebaseRegistrationTokenService;
        private readonly IVideoRequestPriceCalculationsService VideoRequestPriceCalculationsService;

        public CustomerVideoRequestController(
            ICustomerService customerService,
            IVideoRequestSearchService searchService,
            IVideoRequestService videoRequestService,
            IVideoRequestTypeService videoRequestTypeService,
            IFirebaseRegistrationTokenService firebaseRegistrationTokenService,
            IVideoRequestPriceCalculationsService videoRequestPriceCalculationsService,
            ILogger<CustomerVideoRequestController> logger)
        {
            CustomerService = customerService;
            SearchService = searchService;
            VideoRequestService = videoRequestService;
            VideoRequestTypeService = videoRequestTypeService;
            FirebaseRegistrationTokenService = firebaseRegistrationTokenService;
            VideoRequestPriceCalculationsService = videoRequestPriceCalculationsService;
            _logger = logger;
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
                throw new Exception("Ваш заказ не найден");

            if (request.ViewedByCustomer == false)
            {
                request.ViewedByCustomer = true;
                VideoRequestService.Update(request, curUser.ID);
            }

            var requestVM = new VideoRequestDetailsForCustomerVM(request);
            requestVM.EditBtnIsAvailable = VideoRequestService.IsEditable(request);
            requestVM.CancelBtnIsAvailable = VideoRequestService.IsCancelable(request);

            requestVM.RequestPrice = VideoRequestPriceCalculationsService.CalculateRequestPrice(request);
            requestVM.RequestPriceToStr();

            requestVM.RemainingPrice = VideoRequestPriceCalculationsService.CalculateRemainingPrice(request.Price, request.WebsiteCommission);
            requestVM.RemainingPriceToStr();

            requestVM.VideoIsConfirmed = VideoRequestService.IsVideoConfirmed(request);
            requestVM.PaymentScreenshotIsUploaded = VideoRequestService.IsPaymentScreenshotUploaded(request);

            requestVM.PaymentIsConfirmed = VideoRequestService.IsPaymentConfirmed(request);
            if (requestVM.PaymentIsConfirmed)
                requestVM.Video = new AttachmentDetailsVM(request.Video);

            //VideoRequestEditVM editModelVM = new VideoRequestEditVM(request);
            //ViewBag.editModelVM = editModelVM;
            //ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

            ViewBag.firebaseUid = curUser.FirebaseUid;
            ViewBag.firebaseToken = FirebaseRegistrationTokenService.GetForWebByUserID(curUser.ID);

            return View(requestVM);
        }
    }
}