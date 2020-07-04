using System;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.Utils;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    public class CustomerPersonalDataController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly ISocialAreaService SocialAreaService;
        private readonly IAttachmentService AttachmentService;
        private readonly IFirebaseRegistrationTokenService FirebaseRegistrationTokenService;

        public CustomerPersonalDataController(
            ICustomerService customerService,
            ISocialAreaService socialAreaService,
            IAttachmentService attachmentService,
            IFirebaseRegistrationTokenService firebaseRegistrationTokenService)
        {
            CustomerService = customerService;
            SocialAreaService = socialAreaService;
            AttachmentService = attachmentService;
            FirebaseRegistrationTokenService = firebaseRegistrationTokenService;
        }

        public IActionResult Index()
        {
            var curUser = accountUtil.GetCurrentUser(User);
            Customer model = CustomerService.GetActiveSingleDetailsWithRelatedDataByUserID(curUser.ID);
            if (model == null)
                return CustomBadRequest("Клиент не найден");

            CustomerEditVM modelVM = new CustomerEditVM(model);

            ViewBag.firebaseUid = curUser.FirebaseUid;
            ViewBag.firebaseToken = FirebaseRegistrationTokenService.GetForWebByUserID(curUser.ID);

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
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
                ModelState.AddModelError("", "Неверные данные");

            if (model.AvatarID.HasValue)
                model.Avatar = AttachmentService.GetByID(model.AvatarID.Value);

            modelVM.Avatar = new AttachmentDetailsVM(model.Avatar);

            //ViewData["socialAreas"] = SocialAreaService.GetAsSelectList(/*modelVM.SocialAreaID ?? 0*/);
            ViewBag.firebaseUid = curUser.FirebaseUid;
            ViewBag.firebaseToken = FirebaseRegistrationTokenService.GetForWebByUserID(curUser.ID);

            return View(modelVM);
        }
    }
}