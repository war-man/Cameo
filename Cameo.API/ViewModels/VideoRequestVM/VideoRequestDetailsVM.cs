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
        public TalentWithCreditCardInfoVM talent { get; set; }

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

        public bool video_is_confirmed { get; set; }

        //public bool is_credit_card_info_visible { get; set; } = false;
        public AttachmentDetailsVM payment_screenshot { get; set; }
        public bool payment_screenshot_is_uploaded { get; set; } = false;

        public bool payment_is_confirmed { get; set; } = false;
        
        






        //public string VideoDeadline { get; set; }
        //public bool Payable { get; set; } = false;
        //public bool VideoIsPaid { get; set; } = false;
        //public bool UploadVideoBtnIsAvailable { get; set; } = false;

        
        //public bool BalanceAllowsToConfirm { get; set; }

        public VideoRequestDetailsVM() { }

        public VideoRequestDetailsVM(VideoRequest model)
        {
            if (model == null)
                return;

            id = model.ID;
            talent = new TalentWithCreditCardInfoVM(model.Talent);
            to = model.To;
            from = model.From;
            instructions = model.Instructions;
            //Email = model.Email;
            is_not_public = model.IsNotPublic;
            price = model.Price;
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            price_str = price.ToString(numberFormat).Trim();
            //video = new AttachmentDetailsVM(model.Video);
            status = new VideoRequestStatusDetailsVM(model.RequestStatus);
            //videoRequestEditVM = new VideoRequestEditVM(model);

            string dateTextViewStringFormat = AppData.Configuration.DateTextViewStringFormat;

            if (model.PaymentConfirmationDeadline.HasValue)
                deadline = model.PaymentConfirmationDeadline.Value.ToString(dateTextViewStringFormat);
            else if (model.VideoDeadline.HasValue)
                deadline = model.VideoDeadline.Value.ToString(dateTextViewStringFormat);
            else
                deadline = model.RequestAnswerDeadline.ToString(dateTextViewStringFormat);

            //payment_screenshot = new AttachmentDetailsVM(model.PaymentScreenshot);
            payment_screenshot = AttachmentDetailsVM.ToVM(model.PaymentScreenshot);





            //if (model.VideoDeadline.HasValue)
            //    VideoDeadline = model.VideoDeadline.Value.ToString(AppData.Configuration.DateTextViewStringFormat);

            //if (model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo)
            //{
            //    cancel_btn_is_available = UploadVideoBtnIsAvailable = edit_btn_is_available = true;
            //}

            //Payable = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;

            //VideoIsPaid = model.RequestStatusID == (int)VideoRequestStatusEnum.paymentScreenshotUploaded
            //    && model.VideoID.HasValue;


            //VideoIsConfirmed = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;
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

    public class VideoRequestDetailsForCustomerVM : VideoRequestDetailsVM
    {
        public VideoRequestDetailsForCustomerVM() { }

        public VideoRequestDetailsForCustomerVM(VideoRequest model)
            : base(model)
        {
            if (model == null)
                return;

            //is_credit_card_info_visible = true;
        }
    }

    public class VideoRequestDetailsForTalentVM : VideoRequestDetailsVM
    {
        public VideoRequestDetailsForTalentVM() { }

        public VideoRequestDetailsForTalentVM(VideoRequest model)
            : base(model)
        {
            if (model == null)
                return;

            //video = new AttachmentDetailsVM(model.Video);
            video = AttachmentDetailsVM.ToVM(model.Video);
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
            //video = new AttachmentDetailsVM(model.Video);
            video = AttachmentDetailsVM.ToVM(model.Video);
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
