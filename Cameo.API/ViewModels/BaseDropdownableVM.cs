using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;

namespace Cameo.API.ViewModels
{
    public class BaseDropdownableDetailsVM
    {
        public int id { get; set; }
        public string name { get; set; }

        public BaseDropdownableDetailsVM() { }

        public BaseDropdownableDetailsVM(BaseModelDropdownable model)
        {
            if (model == null)
                return;

            this.id = model.ID;
            this.name = model.Name;
        }
    }
}
