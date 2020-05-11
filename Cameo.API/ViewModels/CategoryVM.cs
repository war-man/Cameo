using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class CategoryShortInfoVM
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public CategoryShortInfoVM() { }

        public CategoryShortInfoVM(Category model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.Name = model.Name;
        }
    }

    public class CategoryVM : CategoryShortInfoVM
    {
        public bool Selected { get; set; }
        public int NumberOfItems { get; set; }
    }
}
