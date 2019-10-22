using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Cameo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    [Authorize]
    public class VideoRequestSearchController : BaseController
    {
        
        private readonly IVideoRequestSearchService SearchService;

        public VideoRequestSearchController(
            IVideoRequestSearchService searchService)
        {
            SearchService = searchService;
        }

        [HttpPost]
        public IActionResult Index(int draw, int start, int length)
        {
            int recordsTotal = 2;
            int recordsFiltered = 2;
            List<VideoRequestListItemVM> data = new List<VideoRequestListItemVM>();
            string error = "";

            AppUserVM curUser = accountUtil.GetCurrentUser(User);
            string userID = curUser.ID;
            string userType = curUser.Type;

            IQueryable<VideoRequest> dataIQueryable = SearchService.Search(
                userID,
                userType,

                start,
                length,

                out recordsTotal,
                out recordsFiltered,
                out error
            );

            data = dataIQueryable
                .Select(m => new VideoRequestListItemVM(m))
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
    }
}