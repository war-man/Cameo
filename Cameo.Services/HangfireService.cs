using Cameo.Models;
using Cameo.Services.Interfaces;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services
{
    public class HangfireService : IHangfireService
    {
        //private readonly IBackgroundJobClient BackgroundJobs;

        public HangfireService()
        {

        }

        public string CreateJobForVideoRequestAnswer(VideoRequest request)
        {
            string jobID = BackgroundJob.Schedule(() => FinishAuction(auction.ID), new DateTimeOffset(auction.Deadline));

            throw new NotImplementedException();
        }

        public void CancelJob(string jobID)
        {
            if (!string.IsNullOrWhiteSpace(jobID))
            {
                try
                {
                    BackgroundJob.Delete(jobID);
                }
                catch
                { }
            }
        }
    }
}
