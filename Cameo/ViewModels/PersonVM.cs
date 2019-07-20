﻿using Cameo.Models;
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
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(256)]
        public string LastName { get; set; }

        [Display(Name = "О себе")]
        public string Bio { get; set; }

        [Display(Name = "Меня можно найти в")]
        public int? SocialAreaID { get; set; }

        [Display(Name = "Моё имя пользователя в соц. сети")]
        public string SocialAreaHandle { get; set; }

        public PersonEditVM() { }

        public PersonEditVM(Person model)
        {
            if (model == null)
                return;

            this.ID = model.ID;
            this.FirstName = model.FirstName;
            this.LastName = model.LastName;
            this.Bio = model.Bio;
            this.SocialAreaID = model.SocialAreaID;
            this.SocialAreaHandle = model.SocialAreaHandle;
        }
    }
}
