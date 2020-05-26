using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class CategoryShortInfoVM : BaseDropdownableDetailsVM
    {
        public CategoryShortInfoVM() { }

        public CategoryShortInfoVM(Category model)
            : base(model)
        {
        }
    }

    public class CategoryVM : CategoryShortInfoVM
    {
        public bool selected { get; set; }
        public int number_of_items { get; set; }
    }
}
