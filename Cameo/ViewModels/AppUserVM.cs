using Cameo.Utils;
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
        public bool EmailConfirmed { get; set; } = false;
        public bool TalentApprovedByAdmin { get; set; }

        public AppUserVM() { }

        public AppUserVM(ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
                return;

            ID = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            //Type = user.FindFirst(ClaimTypes.UserData)?.Value;
            Type = user.FindFirst(CustomClaimTypes.UserType)?.Value;

            bool tmpBool = false;
            bool.TryParse(user.FindFirst(CustomClaimTypes.EmailConfirmed)?.Value, out tmpBool);
            EmailConfirmed = tmpBool;

            tmpBool = false;
            bool.TryParse(user.FindFirst(CustomClaimTypes.TalentApprovedByAdmin)?.Value, out tmpBool);
            TalentApprovedByAdmin = tmpBool;
        }
    }
}
