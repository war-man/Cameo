using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IAttachmentService AttachmentService;

        public CustomerController(
            ICustomerService customerService,
            IAttachmentService attachmentService)
        {
            CustomerService = customerService;
            AttachmentService = attachmentService;
        }

        public IActionResult Index()
        {
            //var curUser = accountUtil.GetCurrentUser(User);
            //Customer model = CustomerService.GetByUserID(curUser.ID);
            //if (model == null)
            //    return NotFound();

            //if (model.AvatarID.HasValue)
            //    model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            //CustomerShortInfoVM modelVM = new CustomerShortInfoVM(model);

            //return View(modelVM);
            return RedirectToAction("Index", "CustomerPersonalData");
        }
    }
}