using Cameo.Common;
using Cameo.Models;
using Cameo.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class VideoRequestDetailsVM
    {
        public int id { get; set; }

        [Display(Name = "Имя таланта")]
        public PersonShortInfoVM talent { get; set; }

        [Display(Name = "Видео для")]
        public string to { get; set; }

        [Display(Name = "Заказ от")]
        public string from { get; set; }

        [Display(Name = "Инструкции")]
        public string instructions { get; set; }

        //public string Email { get; set; }
        public bool is_not_public { get; set; }

        [Display(Name = "Цена")]
        public int price { get; set; }
        public string price_str { get; set; }

        public int request_price { get; set; }
        public string request_price_str { get; set; }

        public int remaining_price { get; set; }
        public string remaining_price_str { get; set; }

        public AttachmentDetailsVM video { get; set; }

        [Display(Name = "Статус заказа")]
        public VideoRequestStatusDetailsVM status { get; set; }

        public string deadline { get; set; }

        public bool edit_btn_is_available { get; set; } = false;
        public bool cancel_btn_is_available { get; set; } = false;
        public VideoRequestEditVM video_request_edit_vm { get; set; }





        public string VideoDeadline { get; set; }
        public bool Payable { get; set; } = false;
        public bool VideoIsPaid { get; set; } = false;
        public bool UploadVideoBtnIsAvailable { get; set; } = false;

        public bool VideoConfirmed { get; set; }
        //public bool BalanceAllowsToConfirm { get; set; }

        public VideoRequestDetailsVM() { }

        public VideoRequestDetailsVM(VideoRequest model)
        {
            if (model == null)
                return;

            id = model.ID;
            talent = new PersonShortInfoVM(model.Talent);
            to = model.To;
            from = model.From;
            instructions = model.Instructions;
            //Email = model.Email;
            is_not_public = model.IsNotPublic;
            price = model.Price;
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            price_str = price.ToString(numberFormat).Trim();
            video = new AttachmentDetailsVM(model.Video);
            status = new VideoRequestStatusDetailsVM(model.RequestStatus);
            //videoRequestEditVM = new VideoRequestEditVM(model);







            if (model.VideoDeadline.HasValue)
                VideoDeadline = model.VideoDeadline.Value.ToString(AppData.Configuration.DateTextViewStringFormat);

            if (model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo)
            {
                cancel_btn_is_available = UploadVideoBtnIsAvailable = edit_btn_is_available = true;
            }

            Payable = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;

            VideoIsPaid = model.RequestStatusID == (int)VideoRequestStatusEnum.paid
                && model.VideoID.HasValue;


            VideoConfirmed = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;
        }

        public void RequestPriceToStr()
        {
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            request_price_str = request_price.ToString(numberFormat).Trim();
        }

        public void RemainingPriceToStr()
        {
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            remaining_price_str = remaining_price.ToString(numberFormat).Trim();
        }
    }

    public class VideoDetailsVM
    {
        public int request_id { get; set; }
        public AttachmentDetailsVM video { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public TalentShortInfoForVideoPageVM talent { get; set; }

        //TO-DO:
        //public bool Reviewable { get; set; } = false;

        public VideoDetailsVM() { }

        public VideoDetailsVM(VideoRequest model)
        {
            if (model == null)
                return;

            request_id = model.ID;
            video = new AttachmentDetailsVM(model.Video);
            to = model.To;
            from = model.From;
            talent = new TalentShortInfoForVideoPageVM(model.Talent);
        }
    }

    public class VideoRequestStatusDetailsVM : BaseDropdownableDetailsVM
    {
        public VideoRequestStatusDetailsVM() { }

        public VideoRequestStatusDetailsVM(VideoRequestStatus model)
            : base(model)
        {
            if (model == null)
                return;
        }
    }
}
