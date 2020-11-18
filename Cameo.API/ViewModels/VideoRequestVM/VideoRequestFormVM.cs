using Cameo.Models;
using Cameo.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cameo.API.ViewModels
{
    public class VideoRequestCreateVM
    {
        [Required]
        [Display(Name = "Это видео:")]
        //[JsonPropertyName("typeid")]
        public int type_id { get; set; }

        [Required]
        [Display(Name = "Для")]
        //[JsonPropertyName("to")]
        public string to { get; set; }

        //[Required] //if type == Someone else
        [Remote("ValidateFrom", "VideoRequest", ErrorMessage = "Укажите, от кого", AdditionalFields = "TypeID")]
        [Display(Name = "От")]
        //[JsonPropertyName("from")]
        public string from { get; set; }

        [Required]
        //[JsonPropertyName("instructions")]
        public string instructions { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //[JsonPropertyName("email")]
        //public string Email { get; set; }

        //[JsonPropertyName("isnotpublic")]
        public bool is_not_public { get; set; }

        [Required]
        [JsonPropertyName("price")]
        public int price { get; set; }

        [Required]
        //[JsonPropertyName("talentid")]
        public int talent_id { get; set; }

        public int invoice_id { get; set; }

        [Required]
        public string sms { get; set; }

        public VideoRequest ToModel(Customer customer)
        {
            VideoRequest model = new VideoRequest()
            {
                Customer = customer,
                //Email = this.Email,
                //From = this.from,
                Instructions = this.instructions,
                IsNotPublic = this.is_not_public,
                Price = this.price,
                To = this.to,
                TalentID = this.talent_id,
                TypeID = this.type_id
            };

            if (type_id == (int)VideoRequestTypeEnum.someone)
                model.From = from;
            else
                model.From = null;

            return model;
        }
    }

    public class VideoRequestEditVM : VideoRequestCreateVM
    {
        public List<SelectListItem> video_request_types { get; set; }
        public VideoRequestEditVM() { }

        public VideoRequestEditVM(VideoRequest model)
        {
            if (model == null)
                return;

            type_id = model.TypeID;
            to = model.To;
            from = model.From;
            instructions = model.Instructions;
            //Email = model.Email;
            is_not_public = model.IsNotPublic;
            price = model.Price;
            talent_id = model.TalentID;
        }

        public void UpdateModel(VideoRequest model)
        {
            model.TypeID = type_id;
            model.To = to;

            if (type_id == (int)VideoRequestTypeEnum.someone)
                model.From = from;
            else
                model.From = null;
            model.Instructions = instructions;
            //model.Email = Email;
            model.IsNotPublic = is_not_public;
        }
    }
}
