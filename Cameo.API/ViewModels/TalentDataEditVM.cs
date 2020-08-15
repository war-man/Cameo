using Cameo.Common;
using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class TalentPersonalDataEditVM : PersonEditVM
    {
        [Display(Name = "Меня можно найти в")]
        public int? social_area_id { get; set; }

        [Display(Name = "Моё имя пользователя в соц. сети")]
        public string social_area_handle { get; set; }

        [Display(Name = "Количество подписчиков")]
        public string followers_count { get; set; }

        public AttachmentDetailsVM intro_video { get; set; }

        public bool is_available { get; set; }
        public int price { get; set; }
        public string price_str { get; set; }

        public TalentPersonalDataEditVM() { }

        public TalentPersonalDataEditVM(Talent model) : base(model)
        {
            if (model == null)
                return;

            this.social_area_id = model.SocialAreaID;
            this.social_area_handle = model.SocialAreaHandle;
            this.followers_count = model.FollowersCount;
            this.intro_video = AttachmentDetailsVM.ToVM(model.IntroVideo);

            this.is_available = model.IsAvailable;
            this.price = model.Price;

            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            this.price_str = model.Price.ToString(numberFormat).Trim();
        }
    }

    public class TalentPriceEditVM 
    {
        public int id { get; set; }

        [Display(Name = "Цена Вашего видео")]
        //[Range(50000, 2000000)]
        public int price { get; set; }

        public TalentPriceEditVM() { }

        public TalentPriceEditVM(Talent model)
        {
            if (model == null)
                return;

            this.id = model.ID;
            this.price = model.Price;
        }
    }

    public class TalentCreditCardEditVM
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Номер Вашей карты Uzcard")]
        [StringLength(16 + 3)] // 16 - card numer digits + 3 - whitespaces
        public string credit_card_number { get; set; }

        [Required]
        [Display(Name = "Имя владельца карты Uzcard")]
        [StringLength(128)]
        public string credit_card_holder { get; set; }

        [Required]
        [Display(Name = "Срок действия (мм/гг)")]
        [StringLength(5)]
        public string credit_card_expire { get; set; }

        public TalentCreditCardEditVM() { }

        public TalentCreditCardEditVM(Talent model)
        {
            if (model == null)
                return;

            this.id = model.ID;
            this.credit_card_number = model.CreditCardNumber;
            this.credit_card_holder = model.CreditCardHolder;

            if (model.CreditCardExpire.HasValue)
            {
                this.credit_card_expire = model.CreditCardExpire.Value.ToString("MM");
                this.credit_card_expire += "/" + (model.CreditCardExpire.Value.Year - 2000);
            }
        }
    }

    public class TalentProjectsAndCategoriesEditVM
    {
        public int talent_id { get; set; }

        [Display(Name = "Мои проекты")]
        public List<string> projects { get; set; }

        [Display(Name = "Категории")]
        public List<int> categories { get; set; }

        public TalentProjectsAndCategoriesEditVM() { }

        public TalentProjectsAndCategoriesEditVM(Talent model)
        {
            if (model == null)
                return;

            this.talent_id = model.ID;

            this.projects = model.Projects?
                .Select(m => m.Name)
                .ToList()
                ?? new List<string>();

            this.categories = model.TalentCategories?
                .Select(m => m.CategoryId)
                .ToList()
                ?? new List<int>();
        }
    }
}
