﻿using Cameo.Common;
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
    public class VideoRequestDetailsVM
    {
        public int ID { get; set; }

        [Display(Name = "Имя таланта")]
        public TalentWithCreditCardInfoVM Talent { get; set; }

        [Display(Name = "Видео для")]
        public string To { get; set; }

        [Display(Name = "Заказ от")]
        public string From { get; set; }

        [Display(Name = "Инструкции")]
        public string Instructions { get; set; }

        //public string Email { get; set; }
        public bool IsNotPublic { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }
        public string PriceStr { get; set; }

        public int RequestPrice { get; set; }
        public string RequestPriceStr { get; set; }

        public int RemainingPrice { get; set; }
        public string RemainingPriceStr { get; set; }

        public AttachmentDetailsVM Video { get; set; }

        [Display(Name = "Статус заказа")]
        public VideoRequestStatusDetailsVM Status { get; set; }

        [Display(Name = "Срок")]
        public string Deadline { get; set; }

        public bool EditBtnIsAvailable { get; set; } = false;
        public bool CancelBtnIsAvailable { get; set; } = false;
        public bool VideoIsConfirmed { get; set; }
        public bool PaymentIsConfirmed { get; set; } = false;
        //public bool IsCreditCardInfoVisible { get; set; } = false;
        public AttachmentDetailsVM PaymentScreenshot { get; set; }





        public string VideoDeadline { get; set; }
        public bool Payable { get; set; } = false;
        public bool VideoIsPaid { get; set; } = false;
        public bool UploadVideoBtnIsAvailable { get; set; } = false;
        
        
        //public bool BalanceAllowsToConfirm { get; set; }

        public VideoRequestDetailsVM() { }

        public VideoRequestDetailsVM(VideoRequest model)
        {
            if (model == null)
                return;

            ID = model.ID;
            Talent = new TalentWithCreditCardInfoVM(model.Talent);
            To = model.To;
            From = model.From;
            Instructions = model.Instructions;
            //Email = model.Email;
            IsNotPublic = model.IsNotPublic;
            Price = model.Price;
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            PriceStr = Price.ToString(numberFormat).Trim();
            //Video = new AttachmentDetailsVM(model.Video);
            Status = new VideoRequestStatusDetailsVM(model.RequestStatus);

            string dateTextViewStringFormat = AppData.Configuration.DateTextViewStringFormat;

            if (model.PaymentConfirmationDeadline.HasValue)
                Deadline = model.PaymentConfirmationDeadline.Value.ToString(dateTextViewStringFormat);
            else
            if (model.VideoDeadline.HasValue)
                Deadline = model.VideoDeadline.Value.ToString(dateTextViewStringFormat);
            else
                Deadline = model.RequestAnswerDeadline.ToString(dateTextViewStringFormat);

            PaymentScreenshot = new AttachmentDetailsVM(model.PaymentScreenshot);





            //if (model.VideoDeadline.HasValue)
            //    VideoDeadline = model.VideoDeadline.Value.ToString(AppData.Configuration.DateTextViewStringFormat);

            if (model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo)
            {
                CancelBtnIsAvailable = UploadVideoBtnIsAvailable = EditBtnIsAvailable = true;
            }

            Payable = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;

            VideoIsPaid = model.RequestStatusID == (int)VideoRequestStatusEnum.paid
                && model.VideoID.HasValue;

            
            //VideoIsConfirmed = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;
        }

        public void RequestPriceToStr()
        {
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            RequestPriceStr = RequestPrice.ToString(numberFormat).Trim();
        }

        public void RemainingPriceToStr()
        {
            string numberFormat = AppData.Configuration.NumberViewStringFormat;
            RemainingPriceStr = RemainingPrice.ToString(numberFormat).Trim();
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

            //IsCreditCardInfoVisible = true;
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

            Video = new AttachmentDetailsVM(model.Video);
        }
    }

    public class VideoDetailsVM
    {
        public int RequestID { get; set; }
        public AttachmentDetailsVM Video { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public TalentShortInfoForVideoPageVM Talent { get; set; }

        //TO-DO:
        //public bool Reviewable { get; set; } = false;

        public VideoDetailsVM() { }

        public VideoDetailsVM(VideoRequest model)
        {
            if (model == null)
                return;

            RequestID = model.ID;
            Video = new AttachmentDetailsVM(model.Video);
            To = model.To;
            From = model.From;
            Talent = new TalentShortInfoForVideoPageVM(model.Talent);            
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
