using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Common;

namespace Cameo.AdminPanel.ViewModels
{
    public class UserShortInfoVM
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public bool TalentApprovedByAdmin { get; set; }
        public string DateTalentApprovedByAdmin { get; set; }

        public UserShortInfoVM() { }

        public UserShortInfoVM(ApplicationUser user)
        {
            if (user == null)
                return;

            ID = user.Id;
            Username = user.UserName;
            PhoneNumber = user.PhoneNumber;
            TalentApprovedByAdmin = user.TalentApprovedByAdmin;
            if (user.DateTalentApprovedByAdmin.HasValue)
            {
                var date = user.DateTalentApprovedByAdmin.Value;
                DateTalentApprovedByAdmin = date.ToLongDateString() + "" + date.ToLongTimeString();
            }
        }
    }

    //public class UserVM : UserShortInfoVM
    //{
    //    public UserVM(ApplicationUser user)
    //        : base(user)
    //    {
    //        if (user == null)
    //            return;

    //        ID = user.Id;
    //        Username = user.UserName;

    //    }
    //}

    public class TalentAccountListItemVM
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public AttachmentDetailsVM Avatar { get; set; }
        public bool TalentApprovedByAdmin { get; set; } = false;
        public string DateTalentApprovedByAdmin { get; set; }

        public TalentAccountListItemVM() { }

        public TalentAccountListItemVM(ApplicationUser user, Talent talent)
        {
            if (user == null || talent == null)
                return;

            ID = user.Id;
            Username = user.UserName;
            FirstName = talent.FirstName;
            LastName = talent.LastName;
            Avatar = new AttachmentDetailsVM(talent.Avatar);
            TalentApprovedByAdmin = user.TalentApprovedByAdmin;
            if (user.DateTalentApprovedByAdmin.HasValue)
            {
                var date = user.DateTalentApprovedByAdmin.Value;
                DateTalentApprovedByAdmin = date.ToLongDateString() + "" + date.ToLongTimeString();
            }
        }
    }

    public class TalentAccountDetailsVM : TalentAccountListItemVM
    {
        public string PhoneNumber { get; set; }
        public virtual string Email { get; set; }
        public BaseDropdownableDetailsVM SocialArea { get; set; }
        public string SocialAreaHandle { get; set; }
        public string FollowersCount { get; set; }
        public int Price { get; set; } = 0;
        public bool IsAvailable { get; set; }
        public int Balance { get; set; }
        public string AccountNumber { get; set; } //лицевой счет

        public TalentAccountDetailsVM() { }

        public TalentAccountDetailsVM(ApplicationUser user, Talent talent)
            : base(user, talent)
        {
            if (user == null || talent == null)
                return;

            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            SocialArea = new BaseDropdownableDetailsVM(talent.SocialArea);
            SocialAreaHandle = talent.SocialAreaHandle;
            FollowersCount = talent.FollowersCount;
            Price = talent.Price;
            IsAvailable = talent.IsAvailable;
            Balance = talent.Balance;
            AccountNumber = talent.AccountNumber;
        }
    }
}
