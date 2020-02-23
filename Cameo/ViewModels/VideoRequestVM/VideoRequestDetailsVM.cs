using Cameo.Common;
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

        public string To { get; set; }

        public string From { get; set; }

        public string Instructions { get; set; }

        public string Email { get; set; }
        public bool IsNotPublic { get; set; }
        public int Price { get; set; }

        public string VideoDeadline { get; set; }

        public bool CancelBtnIsAvailable { get; set; } = false;
        public bool Payable { get; set; } = false;
        public bool VideoIsPaid { get; set; } = false;
        public bool UploadVideoBtnIsAvailable { get; set; } = false;
        public bool EditBtnIsAvailable { get; set; } = false;

        public VideoRequestStatusDetailsVM Status { get; set; }

        public AttachmentDetailsVM Video { get; set; }
        public bool VideoConfirmed { get; set; }
        public bool BalanceAllowsToConfirm { get; set; }

        public VideoRequestDetailsVM() { }

        public VideoRequestDetailsVM(VideoRequest model)
        {
            if (model == null)
                return;

            ID = model.ID;
            To = model.To;
            From = model.From;
            Instructions = model.Instructions;
            Email = model.Email;
            IsNotPublic = model.IsNotPublic;
            Price = model.Price;

            if (model.VideoDeadline.HasValue)
                VideoDeadline = model.VideoDeadline.Value.ToString(AppData.Configuration.DateTextViewStringFormat);

            Status = new VideoRequestStatusDetailsVM(model.RequestStatus);

            if (model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo)
            {
                CancelBtnIsAvailable = UploadVideoBtnIsAvailable = EditBtnIsAvailable = true;
            }

            Payable = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;

            VideoIsPaid = model.RequestStatusID == (int)VideoRequestStatusEnum.videoPaid
                && model.VideoID.HasValue;

            Video = new AttachmentDetailsVM(model.Video);
            VideoConfirmed = model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted;
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
