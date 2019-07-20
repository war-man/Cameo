using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class Talent : Person
    {
        

        public string FollowersCount { get; set; }
    }
}
