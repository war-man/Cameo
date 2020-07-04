using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.Utils;
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

        [HttpGet]
        public ActionResult<object> Index(int? start = null, int? length = null, int? status_id = 0)
        {
            try
            {
                int recordsTotal = 0;
                int recordsFiltered = 0;
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

                    status_id
                );

                if (!string.IsNullOrWhiteSpace(error))
                    throw new Exception("Ошибка при получении списка заказов");

                dynamic data = null;
                if (AccountUtil.IsUserTalent(curUser))
                {
                    //data = new List<VideoRequestListItemForTalentVM>();
                    data = dataIQueryable
                        .Select(m => new VideoRequestListItemForTalentVM(m))
                        .ToList();
                }
                else
                {
                    //data = new List<VideoRequestListItemForTalentVM>();
                    data = dataIQueryable
                        .Select(m => new VideoRequestListItemForCustomerVM(m))
                        .ToList();
                }

                return new
                {
                    //draw = draw,
                    records_total = recordsTotal,
                    //recordsFiltered = recordsFiltered,
                    data = data,
                    //error = error
                };
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}