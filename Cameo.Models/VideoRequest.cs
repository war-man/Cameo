using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cameo.Models
{
    public class VideoRequest : BaseModel
    {
        [ForeignKey("Type")]
        public int TypeID { get; set; }
        public virtual VideoRequestType Type { get; set; }

        [Required]
        public string To { get; set; }

        public string From { get; set; }

        [Required]
        public string Instructions { get; set; }

        //[Required]
        public string Email { get; set; }
        public bool IsNotPublic { get; set; }
        public int Price { get; set; }
        public double WebsiteCommission { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [Required]
        [ForeignKey("Talent")]
        public int TalentID { get; set; }
        public Talent Talent { get; set; }

        [Required]
        public DateTime RequestAnswerDeadline { get; set; }
        public string RequestAnswerJobID { get; set; }

        public DateTime? VideoDeadline { get; set; }
        public string VideoJobID { get; set; }

        public string PaymentConfirmationJobID { get; set; }
        public DateTime? PaymentConfirmationDeadline { get; set; }

        public string PaymentReminderJobID { get; set; }

        [ForeignKey("RequestStatus")]
        public int RequestStatusID { get; set; }
        public VideoRequestStatus RequestStatus { get; set; }

        [ForeignKey("PaymentScreenshot")]
        public int? PaymentScreenshotID { get; set; }
        public virtual Attachment PaymentScreenshot { get; set; }

        [ForeignKey("Video")]
        public int? VideoID { get; set; }
        public virtual Attachment Video { get; set; }

        public bool ViewedByTalent { get; set; }

        #region RequestStatus change dates
        public DateTime? DateRequestAccepted { get; set; }
        public DateTime? DateRequestExpired { get; set; }
        public DateTime? DateCanceledByCustomer { get; set; }
        public DateTime? DateCanceledByTalent { get; set; }
        public DateTime? DateVideoExpired { get; set; }
        public DateTime? DateVideoUploaded { get; set; }
        public DateTime? DateVideoCompleted { get; set; }
        public DateTime? DatePaymentScreenshotUploaded { get; set; }
        public DateTime? DatePaymentConfirmed { get; set; }
        public DateTime? DatePaymentConfirmationExpired { get; set; }


        //unused dates
        public DateTime? DateRequestCanceledByCustomer { get; set; }
        public DateTime? DateRequestCanceledByTalent { get; set; }
        public DateTime? DateVideoCanceledByCustomer { get; set; }
        public DateTime? DateVideoCanceledByTalent { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? DatePaymentExpired { get; set; }
        #endregion
    }
}
