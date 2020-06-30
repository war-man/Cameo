using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class TalentVisibilityInfoVM
    {
        public bool IsAvailable { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public bool HasAvatar { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPriceSet { get; set; }
        public bool IsCreditCardProvided { get; set; }
        public bool IsCategorySelected { get; set; }
    }
}
