using Cameo.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cameo.ViewModels
{
    public class VideoRequestCreateVM
    {
        [Required]
        [Display(Name = "Это видео:")]
        public int TypeID { get; set; }

        [Required]
        [Display(Name = "Для")]
        public string To { get; set; }

        //[Required] //if type == Someone else
        [Remote("ValidateFrom", "VideoRequest", ErrorMessage = "Укажите, от кого", AdditionalFields = "TypeID")]
        [Display(Name = "От")]
        public string From { get; set; }

        [Required]
        public string Instructions { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool IsNotPublic { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int TalentID { get; set; }

        public VideoRequest ToModel(Customer customer)
        {
            VideoRequest model = new VideoRequest()
            {
                Customer = customer,
                Email = this.Email,
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
}
