using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cameo.API.Utils
{
    public class AccountUtil
    {
        public AppUserVM GetCurrentUser(ClaimsPrincipal user)
        {
            return new AppUserVM(user);
        }

        public static bool IsUserCustomer(AppUserVM userVM)
        {
            return userVM.Type == UserTypesEnum.customer.ToString();
        }

        public static bool IsUserTalent(AppUserVM userVM)
        {
            return userVM.Type == UserTypesEnum.talent.ToString();
        }
    }

    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {

        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.UserType))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(CustomClaimTypes.UserType, user.UserType),
                    //new Claim(CustomClaimTypes.EmailConfirmed, user.EmailConfirmed.ToString()),
                    //new Claim(CustomClaimTypes.TalentApprovedByAdmin, user.TalentApprovedByAdmin.ToString())
                });
            }

            return principal;
        }
    }

    public static class CustomClaimTypes
    {
        public const string UserType = "UserType";
        //public const string EmailConfirmed = "EmailConfirmed";
        //public const string TalentApprovedByAdmin = "TalentApprovedByAdmin";
    }
}
