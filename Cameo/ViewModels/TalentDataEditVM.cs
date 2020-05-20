using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class TalentPersonalDataEditVM : PersonEditVM
    {
        [Display(Name = "Меня можно найти в")]
        public int? SocialAreaID { get; set; }

        [Display(Name = "Моё имя пользователя в соц. сети")]
        public string SocialAreaHandle { get; set; }

        [Display(Name = "Количество подписчиков")]
        public string FollowersCount { get; set; }

        public TalentPersonalDataEditVM() { }

        public TalentPersonalDataEditVM(Talent model) : base(model)
        {
            if (model == null)
                return;

            this.SocialAreaID = model.SocialAreaID;
            this.SocialAreaHandle = model.SocialAreaHandle;
            this.FollowersCount = model.FollowersCount;
        }
    }

    public class TalentPriceEditVM 
    {
        public int ID { get; set; }

        [Display(Name = "Цена Вашего видео")]
        [Range(1000, int.MaxValue)]
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

        [Required]
        [Display(Name = "Номер Вашей карты Uzcard")]
        [StringLength(16 + 3)] // 16 - card numer digits + 3 - whitespaces
        public string CreditCardNumber { get; set; }

        [Required]
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
            if (model.CreditCardExpire.HasValue)
            {
                this.CreditCardExpire = model.CreditCardExpire.Value.ToString("MM");
                this.CreditCardExpire += "/" + (model.CreditCardExpire.Value.Year - 2000);
            }
        }
    }

    public class TalentProjectsAndCategoriesEditVM
    {
        public int TalentID { get; set; }

        [Display(Name = "Мои проекты")]
        public List<string> Projects { get; set; }

        [Display(Name = "Категории")]
        public List<int> Categories { get; set; }

        public TalentProjectsAndCategoriesEditVM() { }

        public TalentProjectsAndCategoriesEditVM(Talent model)
        {
            if (model == null)
                return;

            this.TalentID = model.ID;

            this.Projects = model.Projects?
                .Select(m => m.Name)
                .ToList()
                ?? new List<string>();

            this.Categories = model.TalentCategories?
                .Select(m => m.CategoryId)
                .ToList()
                ?? new List<int>();
        }
    }
}
