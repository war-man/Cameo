using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoRequestSearchController : BaseController
    {
        private readonly IVideoRequestSearchService SearchService;

        public VideoRequestSearchController(
            IVideoRequestSearchService searchService)
        {
            SearchService = searchService;
        }

        [HttpPost]
        public ActionResult<object> Index(int draw, int? start = null, int? length = null, int? statusID = 0)
        {
            int recordsTotal = 0;
            int recordsFiltered = 0;
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
                out error,

                statusID
            );

            data = dataIQueryable
                .Select(m => new VideoRequestListItemVM(m, curUser.Type))
                .ToList();

            return new
            {
                draw = draw,
                records_total = recordsTotal,
                //recordsFiltered = recordsFiltered,
                data = data,
                error = error
            };
        }
    }
}