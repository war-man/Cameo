using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class TalentProjectShortInfoVM
    {
        public int id { get; set; }
        public string name { get; set; }

        public TalentProjectShortInfoVM() { }

        public TalentProjectShortInfoVM(TalentProject model)
        {
            if (model == null)
                return;

            this.id = model.ID;
            this.name = model.Name;
        }
    }
}
