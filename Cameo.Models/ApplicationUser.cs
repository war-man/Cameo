using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CustomTag { get; set; }

        public virtual ICollection<Post> PostsCreated { get; set; }
    }
}
