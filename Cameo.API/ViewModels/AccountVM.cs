using Cameo.Models;
using Cameo.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class LoginVM
    {
        public string firebase_uid { get; set; }
        public string firebase_token { get; set; }
    }

    public class AuthenticateResponseVM
    {
        public bool registration_is_required { get; set; }
        public string auth_token { get; set; }
        public string user_type { get; set; }

        public AuthenticateResponseVM() { }
        public AuthenticateResponseVM(
            bool registrationIsRequired,
            string authToken,
            string userType)
        {
            registration_is_required = registrationIsRequired;
            auth_token = authToken;
            user_type = userType;
        }
    }

    //public class AccountCreateVM
    public class RegisterVM
    {
        [Required]
        //[Display(Name = "Полное имя")]
        public string full_name { get; set; }

        [Required]
        //[Display(Name = "Имя пользователя")]
        [RegularExpression("^[a-z0-9]*$", ErrorMessage = "Only lowercase Alphabets and Numbers allowed.")]
        public string username { get; set; }

        [Required]
        public string firebase_uid { get; set; }
    }

    public class RegisterResponseVM
    {
        public string auth_token { get; set; }
        public string user_id { get; set; }
        public string user_type { get; set; }
        public int customer_id { get; set; }

        public RegisterResponseVM() { }
        public RegisterResponseVM(
            string authToken,
            string userType)
        {
            auth_token = authToken;
            user_type = userType;
        }

        public RegisterResponseVM(Customer customer, string authToken)
        {
            auth_token = authToken;
            user_id = customer.UserID;
            user_type = UserTypesEnum.customer.ToString();
            customer_id = customer.ID;
        }
        
    }

    public class DeleteVM
    {
        [Required]
        public string phone { get; set; }
    }
}