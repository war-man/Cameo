using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IHangfireService
    {
        string CreateJobForVideoRequestAnswer(VideoRequest request);
        void CancelJob(string jobID);
    }
}
