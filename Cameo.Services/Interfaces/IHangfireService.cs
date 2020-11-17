using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IHangfireService
    {
        //string CreateJobForVideoRequestAnswerDeadline(VideoRequest request, string userID);
        string CreateJobForVideoRequestVideoDeadline(VideoRequest request, string userID);
        //string CreateJobForVideoRequestPaymentDeadline(VideoRequest request, string userID);

        //string CreateJobForVideoRequestPaymentConfirmationDeadline(VideoRequest request, string userID);
        //void CreateJobForPaymentReminder(VideoRequest request, string userID);

        void CancelJob(string jobID);
        
        //void CreateTaskForConvertingVideo(int attachmentID, string userID);
        //void StartConvertingVideo(int attachmentID, string userID );
    }
}
