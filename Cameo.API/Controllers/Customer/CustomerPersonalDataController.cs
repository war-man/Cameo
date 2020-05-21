﻿using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Cameo.API.Controllers
{
    [Authorize]
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
            IAttachmentService attachmentService)
        {
            CustomerService = customerService;
            SocialAreaService = socialAreaService;
            AttachmentService = attachmentService;
        }

        [HttpGet]
        public ActionResult<CustomerShortInfoVM> Details()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Customer model = CustomerService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();
            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            CustomerShortInfoVM modelVM = new CustomerShortInfoVM(model);

            return modelVM;
        }

        [HttpPost]
        public ActionResult Save(CustomerEditVM modelVM)
        {
            int statusCode = 200;
            string errorMessage = null;

            var curUser = accountUtil.GetCurrentUser(User);
            Customer model = CustomerService.GetByUserID(curUser.ID);
            if (model != null)
            {
                errorMessage = "Данные клиента не найдены";
                return NotFound(new { errorMessage });
            }

            try
            {
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
                    errorMessage = "Неверные данные";
            }
            catch (Exception ex)
            {
                errorMessage = "Something went wrong while saving data.";
                statusCode = 500;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage)
                && statusCode != 200)
            {
                Response.StatusCode = statusCode;
            }

            return BadRequest(new { errorMessage });
        }
    }
}