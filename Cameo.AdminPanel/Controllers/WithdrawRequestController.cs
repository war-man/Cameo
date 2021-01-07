using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.AdminPanel.ViewModels;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.AdminPanel.Controllers
{
    public class WithdrawRequestController : Controller
    {
        private readonly IWithdrawRequestService WithdrawRequestService;
        private readonly IWithdrawRequestStatusService WithdrawRequestStatusService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WithdrawRequestController(
            IWithdrawRequestService withdrawRequestService,
            IWithdrawRequestStatusService withdrawRequestStatusService,
            UserManager<ApplicationUser> userManager)
        {
            WithdrawRequestService = withdrawRequestService;
            WithdrawRequestStatusService = withdrawRequestStatusService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["statuses"] = WithdrawRequestStatusService.GetAsSelectList();

            return View();
        }

        [HttpPost]
        public virtual IActionResult Search(
            int draw, int? start = null, int? length = null,
            int? statusID = null)
        {
            int recordsTotal = 0;
            int recordsFiltered = 0;
            string error = "";

            //var curUser = accountUtil.GetCurrentUser(User);

            IQueryable<WithdrawRequest> dataIQueryable = WithdrawRequestService.Search(
                start,
                length,

                out recordsTotal,
                out recordsFiltered,
                out error,

                statusID,
                null
            );

            dynamic data = dataIQueryable
                .Select(m => new WithdrawRequestListItemForAdminVM(m))
                .ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = data,
                error = error
            });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var request = WithdrawRequestService.GetByID(id);
            if (request == null)
                throw new Exception("Запрос не найден");

            //var curUser = accountUtil.GetCurrentUser(User);

            if (WithdrawRequestService.IsCompleted(request))
                throw new Exception("Запрос ранее уже был выполнен");

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            WithdrawRequestService.MarkAsCompleted(request, user.Id);

            return Ok();
        }
    }
}