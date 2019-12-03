using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IHangfireService
    {
        string CreateJobForVideoRequestAnswerDeadline(VideoRequest request, string userID);
        string CreateJobForVideoRequestVideoDeadline(VideoRequest request, string userID);
        string CreateJobForVideoRequestPaymentDeadline(VideoRequest request, string userID);

        void CancelJob(string jobID);
    }
}
