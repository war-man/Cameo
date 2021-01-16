using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class PaymoTransaction : BaseModel
    {
        public int store_id { get; set; }
        public int transaction_id { get; set; }
        public DateTime transaction_time { get; set; }
        public int amount { get; set; }
        public int invoice { get; set; }
    }
}
