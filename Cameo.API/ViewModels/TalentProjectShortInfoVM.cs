using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class TalentProjectShortInfoVM
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public TalentProjectShortInfoVM() { }

        public TalentProjectShortInfoVM(TalentProject model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.Name = model.Name;
        }
    }
}
