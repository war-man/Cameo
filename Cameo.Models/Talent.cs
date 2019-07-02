using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class Talent : Person
    {
        [ForeignKey("SocialArea")]
        public int? SocialAreaID { get; set; }
        public virtual SocialArea SocialArea { get; set; }

        public string SocialAreaHandle { get; set; }

        public string FollowersCount { get; set; }
    }
}
