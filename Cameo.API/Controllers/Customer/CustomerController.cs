using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cameo.API.Utils;
using Cameo.Common;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    [Route("api/[controller]")]
    [ApiController]
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

        //[HttpGet("GetBalance")]
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

        //        return Ok(new BalanceVM(customerBalance, customerBalanceFormatted));
        //    }
        //    catch (Exception ex)
        //    {
        //        return CustomBadRequest(ex);
        //    }
        //}

        //[HttpGet("GenerateClickPaymentButtonUrl")]
        //public IActionResult GenerateClickPaymentButtonUrl(int amount, string returnUrl)
        //{
        //    try
        //    {
        //        var curUser = accountUtil.GetCurrentUser(User);
        //        if (!AccountUtil.IsUserCustomer(curUser))
        //            throw new Exception("Вы не являетесь клиентом");

        //        var customer = CustomerService.GetByUserID(curUser.ID);
        //        if (customer == null)
        //            throw new Exception("Вы не являетесь клиентом");

        //        string url = CustomerBalanceService
        //            .GenerateClickPaymentButtonUrl(customer.AccountNumber, amount, returnUrl);

        //        return Ok(new { url = url });
        //    }
        //    catch (Exception ex)
        //    {
        //        return CustomBadRequest(ex);
        //    }
        //}
    }
}