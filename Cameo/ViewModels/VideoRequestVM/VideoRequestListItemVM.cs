using Cameo.Common;
using Cameo.Models;
using Cameo.Models.Enums;
using System;

namespace Cameo.ViewModels
{
    public class VideoRequestListItemVM
    {
        public int ID { get; set; }

        //public CustomerShortInfoVM Customer { get; set; }
        //public TalentShortInfoVM Talent { get; set; }
        ////if curUser.type == customer then person = talent
        ////else person = customer
        //public PersonShortInfoVM Person { get; set; }
        
        public string Deadline { get; set; }
        //public string DeadlineText { get; set; }
        public BaseDropdownableDetailsVM Status { get; set; }

        //public bool ViewedByTalent { get; set; }
        public bool Viewed { get; set; }

        public VideoRequestListItemVM() { }

        public VideoRequestListItemVM(VideoRequest model/*, string curUserType*/)
        {
            if (model == null)
                return;

            ID = model.ID;
            //Customer = new CustomerShortInfoVM(model.Customer);
            //Talent = new TalentShortInfoVM(model.Talent);

            //if (curUserType == UserTypesEnum.customer.ToString())
            //    Person = Talent;
            //else
            //    Person = Customer;

            string dateTextViewStringFormat = AppData.Configuration.DateTextViewStringFormat;

            if (model.PaymentConfirmationDeadline.HasValue)
                Deadline = model.PaymentConfirmationDeadline.Value.ToString(dateTextViewStringFormat);
            else if (model.VideoDeadline.HasValue)
                Deadline = model.VideoDeadline.Value.ToString(dateTextViewStringFormat);
            else
                Deadline = model.RequestAnswerDeadline.ToString(dateTextViewStringFormat);

            //DateTime now = DateTime.Now;
            //DateTime deadlineTmp = DateTime.MinValue;
            //if (model.RequestAnswerDeadline >= now)
            //{
            //    deadlineTmp = model.RequestAnswerDeadline;
            //    DeadlineText = "Ожидает ответа (до " + deadlineTmp.ToShortDateString() + " " + deadlineTmp.ToShortTimeString() + ")";
            //}
            //else
            //    DeadlineText = "Завершено";

            //if (model.VideoDeadline.HasValue && model.VideoDeadline.Value >= now)
            //{
            //    deadlineTmp = model.VideoDeadline.Value;
            //    DeadlineText = "Ожидает видео";
            //}
            //else
            //    DeadlineText = "Завершено";

            //if (deadlineTmp != DateTime.MinValue)
            //    Deadline = deadlineTmp.ToShortDateString() + " " + deadlineTmp.ToShortTimeString();

            Status = new BaseDropdownableDetailsVM(model.RequestStatus);
        }
    }

    public class VideoRequestListItemForCustomerVM : VideoRequestListItemVM
    {
        public TalentShortInfoVM Person { get; set; }

        public VideoRequestListItemForCustomerVM() : base() { }

        public VideoRequestListItemForCustomerVM(VideoRequest model)
            : base(model)
        {
            if (model == null)
                return;

            Person = new TalentShortInfoVM(model.Talent);
            Viewed = model.ViewedByCustomer;
        }
    }

    public class VideoRequestListItemForTalentVM : VideoRequestListItemVM
    {
        public CustomerShortInfoVM Person { get; set; }

        public VideoRequestListItemForTalentVM() : base() { }

        public VideoRequestListItemForTalentVM(VideoRequest model)
            : base(model)
        {
            if (model == null)
                return;

            Person = new CustomerShortInfoVM(model.Customer);
            Viewed = model.ViewedByTalent;
        }
    }
}
