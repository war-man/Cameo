using Cameo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cameo.Utils
{
    public class AccountUtil
    {
        public AppUserVM GetCurrentUser(ClaimsPrincipal user)
        {
            return new AppUserVM(user);
        }
    }
}
