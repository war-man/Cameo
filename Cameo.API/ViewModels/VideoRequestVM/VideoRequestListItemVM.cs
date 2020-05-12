﻿using Cameo.Models;
using Cameo.Models.Enums;
using System;

namespace Cameo.API.ViewModels
{
    public class VideoRequestListItemVM
    {
        public int ID { get; set; }

        public CustomerShortInfoVM Customer { get; set; }
        public TalentShortInfoVM Talent { get; set; }
        //if curUser.type == customer then person = talent
        //else person = customer
        public PersonShortInfoVM Person { get; set; }
        
        public string Deadline { get; set; }
        public string DeadlineText { get; set; }
        public BaseDropdownableDetailsVM Status { get; set; }
        
        public VideoRequestListItemVM() { }

        public VideoRequestListItemVM(VideoRequest model, string curUserType)
        {
            if (model == null)
                return;

            ID = model.ID;
            Customer = new CustomerShortInfoVM(model.Customer);
            Talent = new TalentShortInfoVM(model.Talent);

            if (curUserType == UserTypesEnum.customer.ToString())
                Person = Talent;
            else
                Person = Customer;

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
        }
    }
}