using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public abstract class BaseModel
    {
        public int ID { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Creator")]
        public string CreatedBy { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Modifier")]
        public string ModifiedBy { get; set; }
        public virtual ApplicationUser Modifier { get; set; }
        public DateTime DateModified { get; set; }
    }
}
