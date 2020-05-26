using Cameo.Common;
using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class TalentShortInfoVM : PersonShortInfoVM
    {
        public bool is_available { get; set; }
        public int price { get; set; }
        public string price_str { get; set; }
        public TalentShortInfoVM() { }

        public TalentShortInfoVM(Talent model)
            : base (model)
        {
            if (model == null)
                return;

            this.is_available = model.IsAvailable;
            this.price = model.Price;

            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            this.price_str = model.Price.ToString(numberFormat);
        }
    }

    public class TalentShortInfoForVideoPageVM : TalentShortInfoVM
    {
        public string project_name { get; set; }
        public TalentShortInfoForVideoPageVM() { }

        public TalentShortInfoForVideoPageVM(Talent model)
            : base(model)
        {
            if (model == null)
                return;

            if (model.Projects != null && model.Projects.Count > 0)
                this.project_name = model.Projects.FirstOrDefault()?.Name;
        }
    }

    public class TalentGridViewItem : TalentShortInfoVM
    {
        //public int Price { get; set; }
        public List<CategoryShortInfoVM> categories { get; set; }
        public List<TalentProjectShortInfoVM> projects { get; set; }

        public TalentGridViewItem() { }

        public TalentGridViewItem(Talent model) 
            : base(model)
        {
            if (model == null)
                return;

            //this.Price = model.Price;
            categories = model.TalentCategories
                .Select(m => new CategoryShortInfoVM(m.Category))
                .ToList();

            projects = model.Projects
                .Select(m => new TalentProjectShortInfoVM(m))
                .ToList();
        }
    }

    public class TalentsCategorizedVM
    {
        public int priority { get; set; }
        public CategoryShortInfoVM category { get; set; }
        public List<TalentGridViewItem> talents { get; set; }

        public TalentsCategorizedVM() { }
        public TalentsCategorizedVM(Category category, List<Talent> talents)
        {
            if (category == null)
                return;

            priority = category.ID;
            this.category = new CategoryShortInfoVM(category);

            this.talents = new List<TalentGridViewItem>();
            if (talents != null && talents.Count > 0)
            {
                foreach (var talent in talents)
                {
                    this.talents.Add(new TalentGridViewItem(talent));
                }
            }
        }
    }

    public class TalentDetailsVM : TalentGridViewItem
    {
        //public string bio { get; set; }

        public int request_price { get; set; }
        public string request_price_str { get; set; }

        public AttachmentDetailsVM intro_video { get; set; }

        public TalentDetailsVM() { }

        public TalentDetailsVM(Talent model) 
            : base(model)
        {
            if (model == null)
                return;

            this.is_available = model.IsAvailable;
            //this.bio = model.Bio;
            intro_video = new AttachmentDetailsVM(model.IntroVideo);
        }

        public void RequestPriceToStr()
        {
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            request_price_str = request_price.ToString(numberFormat).Trim();
        }
    }

    public class TalentRequestInfoVM
    {
        public string full_name { get; set; }
        public int price { get; set; }
        public string price_str { get; set; }

        public int request_price { get; set; }
        public string request_price_str { get; set; }

        public int customer_balance { get; set; }
        public string customer_balance_str { get; set; }

        public TalentRequestInfoVM(Talent model, int customerBalance)
        {
            if (model == null)
                return;

            this.full_name = model.FullName;
            price = model.Price;

            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            price_str = model.Price.ToString(numberFormat).Trim();

            customer_balance = customerBalance;
            customer_balance_str = customerBalance.ToString(numberFormat).Trim();
        }

        public void RequestPriceToStr()
        {
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            request_price_str = request_price.ToString(numberFormat).Trim();
        }
    }
}
