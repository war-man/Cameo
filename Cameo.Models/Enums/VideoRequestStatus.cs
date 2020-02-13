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
        requestAcceptedAndwaitingForVideo = 5,
        videoCanceledByCustomer = 6,
        videoCanceledByTalent = 7,
        videoExpired = 8,
        videoCompleted = 9,
        videoPaid = 10,
        //videoPaymentExpired = 11,
    }
}
