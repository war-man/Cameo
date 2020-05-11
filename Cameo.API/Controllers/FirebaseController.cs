using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cameo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : BaseController
    {
        private readonly IFirebaseRegistrationTokenService FirebaseRegistrationTokenService;

        public FirebaseController(IFirebaseRegistrationTokenService firebaseRegistrationTokenService)
        {
            FirebaseRegistrationTokenService = firebaseRegistrationTokenService;
        }

        ////[Authorize]
        //[HttpGet]
        //public List<SelectListItem> Get(int selected = 0)
        //{
        //    var curUser = accountUtil.GetCurrentUser(User);
        //    var username = User.Identity.Name;
        //    //return CategoryService.GetAsSelectList(new int[1] { selected });
        //    return new List<SelectListItem>();
        //}

        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
    }
}