﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class Invoice : BaseModel
    {
        [ForeignKey("VideoRequest")]
        public int? VideoRequestID { get; set; }
        public virtual VideoRequest VideoRequest { get; set; }

        [StringLength(32)]
        public string card_number { get; set; }
        public DateTime card_expiry { get; set; }

        public string hold_id { get; set; }
        public DateTime hold_till { get; set; }

        public int Amount { get; set; }

        public int StatusID { get; set; } //1 - pending, 2 - success, 3 - cancelled

        public DateTime? DateSuccess { get; set; }
        public DateTime? DateCancelled { get; set; }

        //public int ClickTransID { get; set; }
        //public int ServiceID { get; set; }
        //public int ClickPaydocID { get; set; }
        //public string MerchantTransID { get; set; } //litcevoy schot
        //public float Amount { get; set; }
        //public int Error { get; set; }
        //public string ErrorNote { get; set; }
        //public string SignTime { get; set; }
        //public string SignString { get; set; }
        //public DateTime? DateSuccess { get; set; }
        //public DateTime? DateCancelled { get; set; }
    }
}
