using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
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
}
