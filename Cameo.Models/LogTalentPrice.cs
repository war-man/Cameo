using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class LogTalentPrice : BaseModel
    {
        [Required]
        [ForeignKey("Talent")]
        public int TalentID { get; set; }
        public Talent Talent { get; set; }

        public int Price { get; set; }
    }
}
