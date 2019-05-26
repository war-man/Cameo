using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CustomTag { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<Post> PostsAuthored { get; set; }

        [InverseProperty("Creator")]
        public virtual ICollection<Post> PostsCreated { get; set; }

        [InverseProperty("Modifier")]
        public virtual ICollection<Post> PostsModified { get; set; }
    }
}
