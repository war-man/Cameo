using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class BaseModelDropdownable : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        new public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
