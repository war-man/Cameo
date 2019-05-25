using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Models
{
    public abstract class BaseModel
    {
        public int ID { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
