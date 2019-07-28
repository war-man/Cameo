using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class TalentProject : BaseModel
    {
        [ForeignKey("Talent")]
        public int TalentID { get; set; }
        public virtual Talent Talent { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
    }
}
