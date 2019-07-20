﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.Utils;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly ISocialAreaService SocialAreaService;

        public CustomerController(
            ICustomerService customerService,
            ISocialAreaService socialAreaService)
        {
            CustomerService = customerService;
            SocialAreaService = socialAreaService;
        }

        public IActionResult PersonalData()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Customer model = CustomerService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();

            CustomerEditVM modelVM = new CustomerEditVM(model);
            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }

        [HttpPost]
        public IActionResult PersonalData(CustomerEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Customer model = CustomerService.GetByID(modelVM.ID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.FirstName = modelVM.FirstName;
                    model.LastName = modelVM.LastName;
                    model.Bio = modelVM.Bio;
                    model.SocialAreaID = modelVM.SocialAreaID;
                    model.SocialAreaHandle = modelVM.SocialAreaHandle;

                    CustomerService.Update(model, curUser.ID);

                    ViewData["successfullySaved"] = true;
                }
                catch (Exception ex)
                {
                    throw new SystemException("Something went wrong while saving data.", ex);
                }
            }
            else
                ModelState.AddModelError("", "Неверные данные");

            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }
    }
}