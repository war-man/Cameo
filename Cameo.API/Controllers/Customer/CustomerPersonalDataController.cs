using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPersonalDataController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly ISocialAreaService SocialAreaService;
        private readonly IAttachmentService AttachmentService;

        public CustomerPersonalDataController(
            ICustomerService customerService,
            ISocialAreaService socialAreaService,
            IAttachmentService attachmentService,
            ILogger<CustomerPersonalDataController> logger)
        {
            CustomerService = customerService;
            SocialAreaService = socialAreaService;
            AttachmentService = attachmentService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<CustomerShortInfoVM> Details()
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Customer model = CustomerService.GetActiveSingleDetailsWithRelatedDataByUserID(curUser.ID);
                if (model == null)
                    throw new Exception("Клиент не найден");

                CustomerShortInfoVM modelVM = new CustomerShortInfoVM(model);

                return modelVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult Save(CustomerEditVM modelVM)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Customer model = CustomerService.GetByUserID(curUser.ID);
                if (model == null)
                    throw new Exception("Данные клиента не найдены");
            
                if (ModelState.IsValid)
                {
                    model.FullName = modelVM.full_name;
                    //model.FirstName = modelVM.FirstName;
                    //model.LastName = modelVM.LastName;
                    model.Bio = modelVM.bio;
                    //model.SocialAreaID = modelVM.SocialAreaID;
                    //model.SocialAreaHandle = modelVM.SocialAreaHandle;

                    CustomerService.Update(model, curUser.ID);

                    return Ok();
                }
                else
                    throw new Exception("Неверные данные");
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}