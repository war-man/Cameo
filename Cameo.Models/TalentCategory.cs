using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Models
{
    public class TalentCategory
    {
        public int TalentId { get; set; }
        public Talent Talent { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
