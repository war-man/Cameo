using System;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.Utils;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IAttachmentService AttachmentService;
        private readonly ICustomerBalanceService CustomerBalanceService;

        public CustomerController(
            ICustomerService customerService,
            IAttachmentService attachmentService,
            ICustomerBalanceService customerBalanceService,
            ILogger<CustomerController> logger)
        {
            CustomerService = customerService;
            AttachmentService = attachmentService;
            CustomerBalanceService = customerBalanceService;
            _logger = logger;
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

        public IActionResult GetBalance()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            if (!AccountUtil.IsUserCustomer(curUser))
                return CustomBadRequest("Вы не являетесь клиентом");

            var customer = CustomerService.GetByUserID(curUser.ID);
            if (customer == null)
                return CustomBadRequest("Вы не являетесь клиентом");

            int customerBalance = CustomerBalanceService.GetBalance(customer);
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            string customerBalanceFormatted = customerBalance.ToString(numberFormat).Trim() + " сум";

            return Ok(customerBalanceFormatted);
        }

        //public IActionResult GenerateClickPaymentButtonUrl(int amount, string returnUrl)
        //{
        //    var curUser = accountUtil.GetCurrentUser(User);
        //    if (!AccountUtil.IsUserCustomer(curUser))
        //        return CustomBadRequest("Вы не являетесь клиентом");

        //    var customer = CustomerService.GetByUserID(curUser.ID);
        //    if (customer == null)
        //        return CustomBadRequest("Вы не являетесь клиентом");

        //    string url = CustomerBalanceService
        //        .GenerateClickPaymentButtonUrl(customer.AccountNumber, amount, returnUrl);

        //    return Ok("");
        //}
    }
}