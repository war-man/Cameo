using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cameo.API.Utils;
using Cameo.Common;

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
            ICustomerBalanceService customerBalanceService)
        {
            CustomerService = customerService;
            AttachmentService = attachmentService;
            CustomerBalanceService = customerBalanceService;
        }

        [HttpGet("GenerateClickPaymentButtonUrl")]
        public IActionResult GenerateClickPaymentButtonUrl(int amount, string returnUrl)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            if (!AccountUtil.IsUserCustomer(curUser))
                return CustomBadRequest("Вы не являетесь клиентом");

            var customer = CustomerService.GetByUserID(curUser.ID);
            if (customer == null)
                return CustomBadRequest("Вы не являетесь клиентом");

            string url = CustomerBalanceService
                .GenerateClickPaymentButtonUrl(customer.AccountNumber, amount, returnUrl);

            return Ok(url);
        }
    }
}