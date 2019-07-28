using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Models
{
    public class Category : BaseModelDropdownable
    {
        public ICollection<TalentCategory> TalentCategories { get; set; }
    }
}
