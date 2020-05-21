//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Cameo.Models;
//using Cameo.Services.Interfaces;
//using Cameo.API.ViewModels;
//using Microsoft.AspNetCore.Mvc;

//namespace Cameo.API.Controllers
//{
//    //[CustomerAuthorization] - must be implemented
//    public class CustomerVideoRequestController : BaseController
//    {
//        private readonly ICustomerService CustomerService;
//        private readonly IVideoRequestSearchService SearchService;
//        private readonly IVideoRequestService VideoRequestService;
//        private readonly IVideoRequestTypeService VideoRequestTypeService;

//        public CustomerVideoRequestController(
//            ICustomerService customerService,
//            IVideoRequestSearchService searchService,
//            IVideoRequestService videoRequestService,
//            IVideoRequestTypeService videoRequestTypeService)
//        {
//            CustomerService = customerService;
//            SearchService = searchService;
//            VideoRequestService = videoRequestService;
//            VideoRequestTypeService = videoRequestTypeService;
//        }

//        public IActionResult Index()
//        {
//            AppUserVM curUser = accountUtil.GetCurrentUser(User);
//            ViewBag.userType = curUser.Type;

//            return View("Index2");
//        }

//        public IActionResult Details(int id)
//        {
//            var curUser = accountUtil.GetCurrentUser(User);
//            VideoRequest model = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(id);

//            if (model == null || !VideoRequestService.BelongsToCustomer(model, curUser.ID))
//                return NotFound();

//            VideoRequestDetailsVM modelVM = new VideoRequestDetailsVM(model);

//            VideoRequestEditVM editModelVM = new VideoRequestEditVM(model);
//            ViewBag.editModelVM = editModelVM;
//            ViewData["videoRequestTypes"] = VideoRequestTypeService.GetAsSelectList();

//            return View(modelVM);
//        }
//    }
//}