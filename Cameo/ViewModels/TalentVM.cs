using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class TalentShortInfoVM : PersonShortInfoVM
    {
        public bool IsAvailable { get; set; }
        public TalentShortInfoVM() { }

        public TalentShortInfoVM(Talent model)
            : base (model)
        {
            if (model == null)
                return;

            IsAvailable = model.IsAvailable;
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

    public class TalentsCategorizedVM
    {
        public int Priority { get; set; }
        public CategoryShortInfoVM Category { get; set; }
        public List<TalentGridViewItem> Talents { get; set; }

        public TalentsCategorizedVM() { }
        public TalentsCategorizedVM(Category category, List<Talent> talents)
        {
            if (category == null)
                return;

            Priority = category.ID;
            Category = new CategoryShortInfoVM(category);

            Talents = new List<TalentGridViewItem>();
            if (talents != null && talents.Count > 0)
            {
                foreach (var talent in talents)
                {
                    Talents.Add(new TalentGridViewItem(talent));
                }
            }
        }
    }

    public class TalentDetailsVM : TalentGridViewItem
    {
        public string Bio { get; set; }

        public TalentDetailsVM() { }

        public AttachmentDetailsVM Video { get; set; }
        public int RequestID { get; set; }

        public TalentDetailsVM(Talent model) : base(model)
        {
            if (model == null)
                return;

            this.IsAvailable = model.IsAvailable;
            this.Bio = model.Bio;
        }
    }
}
