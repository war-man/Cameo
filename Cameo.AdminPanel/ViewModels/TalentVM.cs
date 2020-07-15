using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.AdminPanel.ViewModels
{
    public class TalentShortInfoVM
    {
        public int ID { get; set; }
        public UserShortInfoVM User { get; set; }
        public string FullName { get; set; }
        public AttachmentDetailsVM Avatar { get; set; }

        public TalentShortInfoVM() { }

        public TalentShortInfoVM(Talent talent)
        {
            if (talent == null)
                return;

            ID = talent.ID;
            User = new UserShortInfoVM(talent.User);
            FullName = talent.FullName;
            Avatar = new AttachmentDetailsVM(talent.Avatar);
        }
    }

    public class TalentDetailsVM : TalentShortInfoVM
    {
        public BaseDropdownableDetailsVM SocialArea { get; set; }
        public string SocialAreaHandle { get; set; }
        public string FollowersCount { get; set; }
        public int Price { get; set; } = 0;
        public bool IsAvailable { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardExpire { get; set; }
        public string CreditCardHolder { get; set; }
        public string Categories { get; set; }

        public TalentDetailsVM() { }

        public TalentDetailsVM(Talent talent)
            : base(talent)
        {
            if (talent == null)
                return;

            SocialArea = new BaseDropdownableDetailsVM(talent.SocialArea);
            SocialAreaHandle = talent.SocialAreaHandle;
            FollowersCount = talent.FollowersCount;
            Price = talent.Price;
            IsAvailable = talent.IsAvailable;
            CreditCardNumber = talent.CreditCardNumber;
            CreditCardExpire = talent.CreditCardExpire.HasValue ? talent.CreditCardExpire.Value.ToShortDateString() : "";
            CreditCardHolder = talent.CreditCardHolder;

            Categories = "";
            foreach (var item in talent.TalentCategories)
            {
                Categories += item.Category.Name + ", ";
            }
            if (Categories.Length > 0)
                Categories = Categories.Substring(0, Categories.Length - 2);
        }
    }
}
