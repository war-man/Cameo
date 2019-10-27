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
        private readonly IVideoRequestService VideoRequestService;

        public HangfireService(IVideoRequestService videoRequestService)
        {
            VideoRequestService = videoRequestService;
        }

        public string CreateJobForVideoRequestAnswerDeadline(VideoRequest request, string userID)
        {
            string jobID = BackgroundJob.Schedule(() => 
                AnswerDeadlineReaches(request.ID, userID), 
                new DateTimeOffset(request.RequestAnswerDeadline));

            return jobID;
        }

        public void AnswerDeadlineReaches(int videoRequestID, string userID)
        {
            try
            {
                VideoRequest request = VideoRequestService.GetByID(videoRequestID);
                VideoRequestService.AnswerDeadlineReaches(request, userID);
            }
            catch (Exception ex)
            { }
        }

        public string CreateJobForVideoRequestVideoDeadline(VideoRequest request, string userID)
        {
            string jobID = BackgroundJob.Schedule(() => 
                VideoDeadlineReaches(request.ID, userID), 
                new DateTimeOffset(request.VideoDeadline.Value));

            return jobID;
        }

        public void VideoDeadlineReaches(int videoRequestID, string userID)
        {
            try
            {
                VideoRequest request = VideoRequestService.GetByID(videoRequestID);
                VideoRequestService.VideoDeadlineReaches(request, userID);
            }
            catch (Exception ex)
            { }
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
