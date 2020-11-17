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
            return RedirectToAction("Index", "CustomerPersonalData");
        }

        //ajax
        //public IActionResult GetBalance()
        //{
        //    try
        //    {
        //        var curUser = accountUtil.GetCurrentUser(User);
        //        if (!AccountUtil.IsUserCustomer(curUser))
        //            throw new Exception("Вы не являетесь клиентом");

        //        var customer = CustomerService.GetByUserID(curUser.ID);
        //        if (customer == null)
        //            throw new Exception("Вы не являетесь клиентом");

        //        int customerBalance = CustomerBalanceService.GetBalance(customer);
        //        string numberFormat = AppData.Configuration.NumberViewStringFormat;
        //        string customerBalanceFormatted = customerBalance.ToString(numberFormat).Trim() + " сум";

        //        return Ok(customerBalanceFormatted);
        //    }
        //    catch (Exception ex)
        //    {
        //        return CustomBadRequest(ex);
        //    }
        //}
    }
}