using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class AppUserVM
    {
        public string ID { get; set; }
        public string Type { get; set; }

        public AppUserVM() { }

        public AppUserVM(ClaimsPrincipal user)
        {
            if (user == null)
                return;

            ID = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            Type = user.FindFirst(ClaimTypes.UserData)?.Value;
        }
    }
}
