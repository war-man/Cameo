using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cameo.Controllers
{
    public class BaseController : Controller
    {
        public AccountUtil accountUtil = new AccountUtil();
    }
}