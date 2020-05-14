using Cameo.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cameo.API.ViewModels
{
    public class VideoRequestCreateVM
    {
        [Required]
        [Display(Name = "Это видео:")]
        [JsonPropertyName("typeid")]
        public int TypeID { get; set; }

        [Required]
        [Display(Name = "Для")]
        [JsonPropertyName("to")]
        public string To { get; set; }

        //[Required] //if type == Someone else
        [Remote("ValidateFrom", "VideoRequest", ErrorMessage = "Укажите, от кого", AdditionalFields = "TypeID")]
        [Display(Name = "От")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [Required]
        [JsonPropertyName("instructions")]
        public string Instructions { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //[JsonPropertyName("email")]
        //public string Email { get; set; }

        [JsonPropertyName("isnotpublic")]
        public bool IsNotPublic { get; set; }

        [Required]
        [JsonPropertyName("price")]
        public int Price { get; set; }

        [Required]
        [JsonPropertyName("talentid")]
        public int TalentID { get; set; }

        public VideoRequest ToModel(Customer customer)
        {
            VideoRequest model = new VideoRequest()
            {
                Customer = customer,
                //Email = this.Email,
                From = this.From,
                Instructions = this.Instructions,
                IsNotPublic = this.IsNotPublic,
                Price = this.Price,
                To = this.To,
                TalentID = this.TalentID,
                TypeID = this.TypeID
            };

            return model;
        }
    }

    public class VideoRequestEditVM : VideoRequestCreateVM
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
            model.From = From;
            model.Instructions = Instructions;
            //model.Email = Email;
            model.IsNotPublic = IsNotPublic;
        }
    }
}
