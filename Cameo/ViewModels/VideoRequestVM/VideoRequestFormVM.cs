﻿using Cameo.Models;
using Cameo.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cameo.ViewModels
{
    public class VideoRequestFormVM
    {
        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name = "VideoType", ResourceType = typeof(Resources.ResourceTexts))]
        public int TypeID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name = "ToBig", ResourceType = typeof(Resources.ResourceTexts))]
        public string To { get; set; }

        //[Required] //if type == Someone else
        [Remote("ValidateFrom", "VideoRequest", ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ProvideFromWhom", AdditionalFields = "TypeID")]
        [Display(Name = "From", ResourceType = typeof(Resources.ResourceTexts))]
        public string From { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name = "Instructions", ResourceType = typeof(Resources.ResourceTexts))]
        public string Instructions { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        public bool IsNotPublic { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        public int Price { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        public int TalentID { get; set; }

        public int InvoiceID { get; set; }
    }

    public class VideoRequestCreateVM : VideoRequestFormVM
    {
        [Display(Name = "Sms", ResourceType = typeof(Resources.ResourceTexts))]
        [Required(ErrorMessageResourceType = typeof(Resources.ResourceTexts), ErrorMessageResourceName = "ErrorRequiredField")]
        public string Sms { get; set; }

        public VideoRequest ToModel(Customer customer)
        {
            VideoRequest model = new VideoRequest()
            {
                Customer = customer,
                //Email = this.Email,
                //From = this.From,
                Instructions = this.Instructions,
                IsNotPublic = this.IsNotPublic,
                Price = this.Price,
                To = this.To,
                TalentID = this.TalentID,
                TypeID = this.TypeID
            };

            if (TypeID == (int)VideoRequestTypeEnum.someone)
                model.From = From;
            else
                model.From = null;

            return model;
        }
    }

    public class VideoRequestEditVM : VideoRequestFormVM
    {
        public int ID { get; set; }

        public VideoRequestEditVM() { }

        public VideoRequestEditVM(VideoRequest model)
        {
            if (model == null)
                return;

            ID = model.ID;
            TypeID = model.TypeID;
            To = model.To;
            From = model.From;
            Instructions = model.Instructions;
            //Email = model.Email;
            IsNotPublic = model.IsNotPublic;
            Price = model.Price;
            TalentID = model.TalentID;
        }

        public void UpdateModel(VideoRequest model)
        {
            model.TypeID = TypeID;
            model.To = To;

            if (TypeID == (int)VideoRequestTypeEnum.someone)
                model.From = From;
            else
                model.From = null;

            model.Instructions = Instructions;
            //model.Email = Email;
            model.IsNotPublic = IsNotPublic;
        }
    }
}
