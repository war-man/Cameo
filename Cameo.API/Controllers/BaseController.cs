using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cameo.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public AccountUtil accountUtil = new AccountUtil();
    }
}