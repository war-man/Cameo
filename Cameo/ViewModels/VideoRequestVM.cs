using Cameo.Models;
using Cameo.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

    public class VideoRequestListItemVM
    {
        public int ID { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Instructions { get; set; } //show this in tooltip
        public int Price { get; set; }
        public CustomerShortInfoVM Customer { get; set; }
        public TalentShortInfoVM Talent { get; set; }

        //if curUser.type == customer then person = talent
        //else person = customer
        public PersonShortInfoVM Person { get; set; }

        public BaseDropdownableDetailsVM Type { get; set; }
        public string DateCreated { get; set; }
        public string Deadline { get; set; }
        public string DeadlineText { get; set; }
        public BaseDropdownableDetailsVM Status { get; set; }

        public AttachmentDetailsVM Video { get; set; }
        public bool VideoConfirmed { get; set; }
        public bool VideoPaid { get; set; }

        public bool CancelBtnIsAvailable { get; set; } = false;
        public bool AcceptBtnIsAvailable { get; set; } = false;
        public bool UploadVideoBtnIsAvailable { get; set; } = false;
        public bool ViewVideoBtnIsAvailable { get; set; } = false; //if true, then customer goes to view page and makes payment there is required

        public VideoRequestListItemVM() { }

        public VideoRequestListItemVM(VideoRequest model, string curUserType)
        {
            if (model == null)
                return;

            ID = model.ID;
            To = model.To;
            From = model.From;
            Instructions = model.Instructions;
            Customer = new CustomerShortInfoVM(model.Customer);
            Talent = new TalentShortInfoVM(model.Talent);

            if (curUserType == UserTypesEnum.customer.ToString())
                Person = Talent;
            else
                Person = Customer;
            
            Type = new BaseDropdownableDetailsVM(model.Type);
            DateCreated = model.DateCreated.ToShortDateString() + " " + model.DateCreated.ToShortTimeString();

            DateTime now = DateTime.Now;
            DateTime deadlineTmp = DateTime.MinValue;
            if (model.RequestAnswerDeadline >= now)
            {
                deadlineTmp = model.RequestAnswerDeadline;
                DeadlineText = "Ожидает ответа (до " + deadlineTmp.ToShortDateString() + " " + deadlineTmp.ToShortTimeString() + ")";
            }
            else
                DeadlineText = "Завершено";

            if (model.VideoDeadline.HasValue && model.VideoDeadline.Value >= now)
            {
                deadlineTmp = model.VideoDeadline.Value;
                DeadlineText = "Ожидает видео";
            }
            else
                DeadlineText = "Завершено";

            if (deadlineTmp != DateTime.MinValue)
                Deadline = deadlineTmp.ToShortDateString() + " " + deadlineTmp.ToShortTimeString();
            
            Status = new BaseDropdownableDetailsVM(model.RequestStatus);

            Video = new AttachmentDetailsVM(model.Video);
            VideoConfirmed = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;
            VideoPaid = model.RequestStatusID == (int)VideoRequestStatusEnum.videoPaid;

            CancelBtnIsAvailable = (model.RequestStatusID == (int)VideoRequestStatusEnum.waitingForResponse
                || model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo);

            AcceptBtnIsAvailable = (model.RequestStatusID == (int)VideoRequestStatusEnum.waitingForResponse
                && curUserType == UserTypesEnum.talent.ToString());

            UploadVideoBtnIsAvailable = (model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo
                && curUserType == UserTypesEnum.talent.ToString());

            ViewVideoBtnIsAvailable = (model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted
                || model.RequestStatusID == (int)VideoRequestStatusEnum.videoPaid);
        }
    }
}
