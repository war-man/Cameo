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
    //[CustomerAuthorization] - must be implemented
    public class CustomerVideoRequestController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IVideoRequestSearchService SearchService;
        private readonly IVideoRequestService VideoRequestService;

        public CustomerVideoRequestController(
            ICustomerService customerService,
            IVideoRequestSearchService searchService,
            IVideoRequestService videoRequestService)
        {
            CustomerService = customerService;
            SearchService = searchService;
            VideoRequestService = videoRequestService;
        }

        public IActionResult Index()
        {
            AppUserVM curUser = accountUtil.GetCurrentUser(User);
            ViewBag.userType = curUser.Type;

            return View("Index2");
        }

        public IActionResult Details(int id)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            VideoRequest model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);

            if (model == null || !VideoRequestService.BelongsToCustomer(model, curUser.ID))
                return NotFound();

            VideoRequestDetailsVM modelVM = new VideoRequestDetailsVM(model);

            return View(modelVM);
        }
    }
}