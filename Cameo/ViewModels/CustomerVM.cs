using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class CustomerPersonalDataVM
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Имя")]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(256)]
        public string LastName { get; set; }

        [Display(Name = "О себе")]
        public string Bio { get; set; }
    }
}
