using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class PersonEditVM
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Имя")]
        [StringLength(256)]
        public string FullName { get; set; }

        //[Required]
        //[Display(Name = "Имя")]
        //[StringLength(256)]
        //public string FirstName { get; set; }

        ////[Required]
        //[Display(Name = "Фамилия")]
        //[StringLength(256)]
        //public string LastName { get; set; }

        [Display(Name = "О себе")]
        public string Bio { get; set; }

        public AttachmentDetailsVM Avatar { get; set; }

        public PersonEditVM() { }

        public PersonEditVM(Person model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.FullName = model.FullName;
            //this.FirstName = model.FirstName;
            //this.LastName = model.LastName;
            this.Bio = model.Bio;
            this.Avatar = new AttachmentDetailsVM(model.Avatar);
        }
    }

    public class PersonShortInfoVM
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string FullName
        //{
        //    get
        //    {
        //        return FirstName + " " + LastName;
        //    }
        //    set { }
        //}
        public string Username { get; set; }
        public AttachmentDetailsVM Avatar { get; set; }

        public PersonShortInfoVM() { }

        public PersonShortInfoVM(Person model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.FullName = model.FullName;
            //this.FirstName = model.FirstName;
            //this.LastName = model.LastName;
            this.Username = model.User?.UserName;
            this.Avatar = new AttachmentDetailsVM(model.Avatar);
        }
    }
}
