using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Models.Enums
{
    public enum VideoRequestStatusEnum
    {
        //waitingForResponse = 1,
        //requestCanceledByCustomer = 2,
        //requestExpired = 3,
        //requestCanceledByTalent = 4,
        requestAcceptedAndWaitingForVideo = 5,
        //videoCanceledByCustomer = 6,
        //videoCanceledByTalent = 7,
        videoExpired = 8,
        
        canceledByCustomer = 12,
        canceledByTalent = 13,

        videoCompleted = 9, //video uploaded and confirmed by talent
        //paymentScreenshotUploaded = 10,
        //videoPaymentExpired = 11,
        //paymentConfirmed = 14,
        //paymentConfirmationExpired = 15,

        //statuses that will be used as filter in list of requests:
        //non db statuses:
        all = 0,
        notCompleted = 100, //all canceled and expired statuses
        //db statuses:
        //1, 5, 9, 10, 14
        failedToWithdrawMoney = 16
    }
}
