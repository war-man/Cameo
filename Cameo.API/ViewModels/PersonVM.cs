using Cameo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class PersonEditVM
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        [StringLength(256)]
        public string full_name { get; set; }

        //[Required]
        //[Display(Name = "Имя")]
        //[StringLength(256)]
        //public string FirstName { get; set; }

        ////[Required]
        //[Display(Name = "Фамилия")]
        //[StringLength(256)]
        //public string LastName { get; set; }

        [Display(Name = "О себе")]
        public string bio { get; set; }

        //[Display(Name = "Меня можно найти в")]
        //public int? SocialAreaID { get; set; }

        //[Display(Name = "Моё имя пользователя в соц. сети")]
        //public string SocialAreaHandle { get; set; }

        public AttachmentDetailsVM avatar { get; set; }

        public PersonEditVM() { }

        public PersonEditVM(Person model)
        {
            if (model == null)
                return;

            this.id = model.ID;
            this.full_name = model.FullName;
            //this.FirstName = model.FirstName;
            //this.LastName = model.LastName;
            this.bio = model.Bio;
            //this.SocialAreaID = model.SocialAreaID;
            //this.SocialAreaHandle = model.SocialAreaHandle;
            this.avatar = new AttachmentDetailsVM(model.Avatar);
        }
    }

    public class PersonShortInfoVM
    {
        public int id { get; set; }
        public string full_name { get; set; }
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
        public string username { get; set; }
        public string bio { get; set; }
        public AttachmentDetailsVM avatar { get; set; }

        public PersonShortInfoVM() { }

        public PersonShortInfoVM(Person model)
        {
            if (model == null)
                return;

            this.id = model.ID;
            this.full_name = model.FullName;
            //this.FirstName = model.FirstName;
            //this.LastName = model.LastName;
            this.username = model.User?.UserName;
            this.bio = model.Bio;
            this.avatar = new AttachmentDetailsVM(model.Avatar);
        }
    }
}
