using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class Talent : Person
    {
        public string FollowersCount { get; set; }

        public int Price { get; set; } = 0;

        [StringLength(32)]
        public string CreditCardNumber { get; set; }
        [StringLength(5)]
        public string CreditCardExpire { get; set; }

        public bool IsAvailable { get; set; }

        public int ResponseTime { get; set; }

        public int Balance { get; set; }

        [InverseProperty("Talent")]
        public virtual ICollection<TalentProject> Projects { get; set; }

        //public virtual ICollection<TalentCategory> Categories { get; set; }
        public ICollection<TalentCategory> TalentCategories { get; set; }

        [InverseProperty("Talent")]
        public virtual ICollection<VideoRequest> VideoRequests { get; set; }
    }
}
