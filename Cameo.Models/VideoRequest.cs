using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class VideoRequest : BaseModel
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Instructions { get; set; }
        public string Email { get; set; }
        public bool IsPublic { get; set; }
        public int Price { get; set; }

        [ForeignKey("Talent")]
        public int CustomertID { get; set; }
        public Customer Customer { get; set; }
    }
}
