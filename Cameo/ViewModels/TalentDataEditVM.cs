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
        [Display(Name = "MySocialArea", ResourceType = typeof(Resources.ResourceTexts))]
        public int? SocialAreaID { get; set; }

        [Display(Name = "MySocialAreaHandle", ResourceType = typeof(Resources.ResourceTexts))]
        public string SocialAreaHandle { get; set; }

        [Display(Name = "MyNumberOfFollowers", ResourceType = typeof(Resources.ResourceTexts))]
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

        [Display(Name = "MyVideoPrice", ResourceType = typeof(Resources.ResourceTexts))]
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

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name = "MyUzcardNumber", ResourceType = typeof(Resources.ResourceTexts))]
        [StringLength(16 + 3)] // 16 - card numer digits + 3 - whitespaces
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name = "MyUzcardExpire", ResourceType = typeof(Resources.ResourceTexts))]
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

        [Display(Name = "MyProjects", ResourceType = typeof(Resources.ResourceTexts))]
        public List<string> Projects { get; set; }

        [Display(Name = "MyCategories", ResourceType = typeof(Resources.ResourceTexts))]
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
