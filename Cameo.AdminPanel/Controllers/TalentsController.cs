using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.AdminPanel.ViewModels;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.AdminPanel.Controllers
{
    public class TalentsController : Controller
    {
        private readonly ITalentService TalentService;

        public TalentsController(
            ITalentService talentService)
        {
            TalentService = talentService;
        }

        public IActionResult Index()
        {
            List<TalentShortInfoVM> talentsVM = new List<TalentShortInfoVM>();

            var talents = TalentService
                .GetWithRelatedDataAsIQueryable()
                .OrderByDescending(m => m.ID);
            foreach (var talent in talents)
            {
                talentsVM.Add(new TalentShortInfoVM(talent));
            }

            foreach (var talent in talentsVM)
            {
                if (talent.Avatar.ID == 0)
                    talent.Avatar.Url = TalentService.GetRandomPhotoUrl();
            }

            return View(talentsVM);
        }

        public IActionResult Details(int id)
        {
            var talent = TalentService.GetActiveSingleDetailsWithRelatedDataByID(id);
            if (talent == null)
                return NotFound();

            TalentDetailsVM talentVM = new TalentDetailsVM(talent);

            if (talentVM.Avatar.ID == 0)
                talentVM.Avatar.Url = TalentService.GetRandomPhotoUrl();

            return View(talentVM);
        }
    }
}