﻿using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class TalentShortInfoVM : PersonShortInfoVM
    {
        public TalentShortInfoVM() { }

        public TalentShortInfoVM(Talent model)
            : base (model)
        {
            if (model == null)
                return;
        }
    }

    public class TalentShortInfoForVideoPageVM : TalentShortInfoVM
    {
        public string ProjectName { get; set; }
        public TalentShortInfoForVideoPageVM() { }

        public TalentShortInfoForVideoPageVM(Talent model)
            : base(model)
        {
            if (model == null)
                return;

            if (model.Projects != null && model.Projects.Count > 0)
                this.ProjectName = model.Projects.FirstOrDefault()?.Name;
        }
    }

    public class TalentGridViewItem : TalentShortInfoVM
    {
        public int Price { get; set; }
        public List<CategoryShortInfoVM> Categories { get; set; }
        public List<TalentProjectShortInfoVM> Projects { get; set; }

        public TalentGridViewItem() { }

        public TalentGridViewItem(Talent model) : base(model)
        {
            if (model == null)
                return;

            this.Price = model.Price;
            this.Categories = model.TalentCategories
                .Select(m => new CategoryShortInfoVM(m.Category))
                .ToList();

            this.Projects = model.Projects
                .Select(m => new TalentProjectShortInfoVM(m))
                .ToList();
        }
    }

    public class TalentDetailsVM : TalentGridViewItem
    {
        public bool IsAvailable { get; set; }
        public string Bio { get; set; }

        public TalentDetailsVM() { }

        public TalentDetailsVM(Talent model) : base(model)
        {
            if (model == null)
                return;

            this.IsAvailable = model.IsAvailable;
            this.Bio = model.Bio;
        }
    }

    public class TalentPersonalDataEditVM : PersonEditVM
    {
        [Display(Name = "Количество подписчиков")]
        public string FollowersCount { get; set; }

        public TalentPersonalDataEditVM() { }

        public TalentPersonalDataEditVM(Talent model) : base(model)
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
