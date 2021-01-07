using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class WithdrawRequest : BaseModel
    {
        [ForeignKey("Talent")]
        public int TalentID { get; set; }
        public virtual Talent Talent { get; set; }

        public int Amount { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public virtual WithdrawRequestStatus Status { get; set; }

        public DateTime? DateCompleted { get; set; }
    }
}
