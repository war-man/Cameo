using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.AdminPanel.ViewModels;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.AdminPanel.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITalentService TalentService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ITalentService talentService)
        {
            _userManager = userManager;
            TalentService = talentService;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users
                .Where(m => m.UserType == UserTypesEnum.talent.ToString())
                .ToList();

            var talents = TalentService.GetActiveAsIQueryable()
                .Where(m => m.User != null && users.Select(n => n.Id).Contains(m.UserID))
                .ToDictionary(m => m.UserID, m => m);

            List<TalentAccountListItemVM> modelsVM = new List<TalentAccountListItemVM>();
            foreach (var item in users)
            {
                if (talents.ContainsKey(item.Id))
                    modelsVM.Add(new TalentAccountListItemVM(item, talents[item.Id]));
            }

            return View(modelsVM);
        }

        public IActionResult Details(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user == null || user.Result == null)
                return NotFound();

            var talent = TalentService.GetByUserID(user.Result.Id);
            if (talent == null)
                return NotFound();

            TalentAccountDetailsVM modelVM = new TalentAccountDetailsVM(user.Result, talent);

            return View(modelVM);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            try
            {
                user.TalentApprovedByAdmin = true;
                user.DateTalentApprovedByAdmin = DateTime.Now;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return Ok();
                else
                    return StatusCode(500, "Error saving");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}