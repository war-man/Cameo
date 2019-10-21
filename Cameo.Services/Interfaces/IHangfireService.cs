using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IHangfireService
    {
        string CreateJobForVideoRequestAnswer(VideoRequest request, string userID);
        void CancelJob(string jobID);
    }
}
