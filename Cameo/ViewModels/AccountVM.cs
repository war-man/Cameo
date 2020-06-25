using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    //public class AccountCreateVM
    public class RegisterVM
    {
        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name = "FullName", ResourceType = typeof(Resources.ResourceTexts))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name = "Username", ResourceType = typeof(Resources.ResourceTexts))]
        [RegularExpression("^[a-z0-9]*$", ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorUsernameValidationMessage")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        public string FirebaseUid { get; set; }
    }

    public class EnrollAsTalentVM : RegisterVM
    {
        [Display(Name = "SocialAreaDisplayName", ResourceType = typeof(Resources.ResourceTexts))]
        public int? SocialAreaID { get; set; }

        [Display(Name = "SocialAreaHandleDisplayName", ResourceType = typeof(Resources.ResourceTexts))]
        public string SocialAreaHandle { get; set; }

        [Display(Name = "FollowersCountDisplayName", ResourceType = typeof(Resources.ResourceTexts))]
        public string FollowersCount { get; set; }
    }
}