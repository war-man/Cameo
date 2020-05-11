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
        [Required]
        [Display(Name = "Полное имя")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Имя пользователя")]
        [RegularExpression("^[a-z0-9]*$", ErrorMessage = "Only lowercase Alphabets and Numbers allowed.")]
        public string UserName { get; set; }

        [Required]
        public string FirebaseUid { get; set; }
    }

    public class EnrollAsTalentVM : RegisterVM
    {
        [Display(Name = "Where can we find you?")]
        public int? SocialAreaID { get; set; }

        [Display(Name = "Your handle")]
        public string SocialAreaHandle { get; set; }

        [Display(Name = "How many followers do you have?")]
        public string FollowersCount { get; set; }
    }
}