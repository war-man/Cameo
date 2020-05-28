using Cameo.Common;
using Cameo.Models;
using Cameo.Models.Enums;
using System;

namespace Cameo.API.ViewModels
{
    public class VideoRequestListItemVM
    {
        public int id { get; set; }

        //public CustomerShortInfoVM customer { get; set; }
        //public TalentShortInfoVM talent { get; set; }
        ////if curUser.type == customer then person = talent
        ////else person = customer
        //public PersonShortInfoVM person { get; set; }
        
        public string deadline { get; set; }
        //public string deadline_text { get; set; }
        public BaseDropdownableDetailsVM status { get; set; }

        public bool viewed { get; set; }

        public VideoRequestListItemVM() { }

        public VideoRequestListItemVM(VideoRequest model/*, string curUserType*/)
        {
            if (model == null)
                return;

            id = model.ID;
            //customer = new CustomerShortInfoVM(model.Customer);
            //talent = new TalentShortInfoVM(model.Talent);

            //if (curUserType == UserTypesEnum.customer.ToString())
            //    person = talent;
            //else
            //    person = customer;

            string dateTextViewStringFormat = AppData.Configuration.DateTextViewStringFormat;

            if (model.PaymentConfirmationDeadline.HasValue)
                deadline = model.PaymentConfirmationDeadline.Value.ToString(dateTextViewStringFormat);
            else if (model.VideoDeadline.HasValue)
                deadline = model.VideoDeadline.Value.ToString(dateTextViewStringFormat);
            else
                deadline = model.RequestAnswerDeadline.ToString(dateTextViewStringFormat);

            //DateTime now = DateTime.Now;
            //DateTime deadlineTmp = DateTime.MinValue;
            //if (model.RequestAnswerDeadline >= now)
            //{
            //    deadlineTmp = model.RequestAnswerDeadline;
            //    deadline_text = "Ожидает ответа (до " + deadlineTmp.ToShortDateString() + " " + deadlineTmp.ToShortTimeString() + ")";
            //}
            //else
            //    deadline_text = "Завершено";

            //if (model.VideoDeadline.HasValue && model.VideoDeadline.Value >= now)
            //{
            //    deadlineTmp = model.VideoDeadline.Value;
            //    deadline_text = "Ожидает видео";
            //}
            //else
            //    deadline_text = "Завершено";

            //if (deadlineTmp != DateTime.MinValue)
            //    deadline = deadlineTmp.ToShortDateString() + " " + deadlineTmp.ToShortTimeString();

            status = new BaseDropdownableDetailsVM(model.RequestStatus);
        }
    }

    public class VideoRequestListItemForCustomerVM : VideoRequestListItemVM
    {
        public TalentShortInfoVM person { get; set; }

        public VideoRequestListItemForCustomerVM() : base() { }

        public VideoRequestListItemForCustomerVM(VideoRequest model)
            : base(model)
        {
            if (model == null)
                return;

            person = new TalentShortInfoVM(model.Talent);
            viewed = model.ViewedByCustomer;
        }
    }

    public class VideoRequestListItemForTalentVM : VideoRequestListItemVM
    {
        public CustomerShortInfoVM person { get; set; }

        public VideoRequestListItemForTalentVM() : base() { }

        public VideoRequestListItemForTalentVM(VideoRequest model)
            : base(model)
        {
            if (model == null)
                return;

            person = new CustomerShortInfoVM(model.Customer);
            viewed = model.ViewedByTalent;
        }
    }
}
