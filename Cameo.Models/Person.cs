using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public abstract class Person : BaseModel
    {
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string LastName { get; set; }

        public string Bio { get; set; }

        [ForeignKey("SocialArea")]
        public int? SocialAreaID { get; set; }
        public virtual SocialArea SocialArea { get; set; }

        public string SocialAreaHandle { get; set; }

        [ForeignKey("Avatar")]
        public int? AvatarID { get; set; }
        public virtual Attachment Avatar { get; set; }
    }
}
