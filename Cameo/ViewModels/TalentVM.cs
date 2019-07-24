using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class TalentShortInfoVM
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set { }
        }
        public AttachmentDetailsVM Avatar { get; set; }

        public TalentShortInfoVM() { }

        public TalentShortInfoVM(Talent model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.FirstName = model.FirstName;
            this.LastName = model.LastName;
            this.Avatar = new AttachmentDetailsVM(model.Avatar);
        }
    }

    public class TalentEditVM : PersonEditVM
    {
        [Display(Name = "Количество подписчиков")]
        public string FollowersCount { get; set; }

        public TalentEditVM() { }

        public TalentEditVM(Talent model) : base(model)
        {
            if (model == null)
                return;

            this.FollowersCount = model.FollowersCount;
        }
    }

    public class TalentPriceEditVM 
    {
        public int ID { get; set; }

        [Display(Name = "Цена Вашего видео")]
        public int Price { get; set; }

        public TalentPriceEditVM() { }

        public TalentPriceEditVM(Talent model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.Price = model.Price;
        }
    }

    public class TalentCreditCardEditVM
    {
        public int ID { get; set; }

        [Display(Name = "Номер Вашей карты Uzcard")]
        [StringLength(32)]
        public string CreditCardNumber { get; set; }

        [Display(Name = "Срок действия (мм/гг)")]
        [StringLength(5)]
        public string CreditCardExpire { get; set; }

        public TalentCreditCardEditVM() { }

        public TalentCreditCardEditVM(Talent model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.CreditCardNumber = model.CreditCardNumber;
            this.CreditCardExpire = model.CreditCardExpire;
        }
    }
}
