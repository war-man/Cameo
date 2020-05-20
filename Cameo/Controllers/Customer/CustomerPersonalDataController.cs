using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
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

        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Customer model = CustomerService.GetByUserID(curUser.ID);
            if (model == null)
                return NotFound();
            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            CustomerEditVM modelVM = new CustomerEditVM(model);
            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }

        [HttpPost]
        public IActionResult Index(CustomerEditVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Customer model = CustomerService.GetByID(modelVM.ID);
            if (model == null || !model.UserID.Equals(curUser.ID))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.FullName = modelVM.FullName;
                    //model.FirstName = modelVM.FirstName;
                    //model.LastName = modelVM.LastName;
                    model.Bio = modelVM.Bio;
                    //model.SocialAreaID = modelVM.SocialAreaID;
                    //model.SocialAreaHandle = modelVM.SocialAreaHandle;

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

            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            modelVM.Avatar = new AttachmentDetailsVM(model.Avatar);

            ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);

            return View(modelVM);
        }
    }
}