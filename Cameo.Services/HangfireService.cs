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
        private readonly IFileManagement FileManagement;
        private readonly IAttachmentService AttachmentService;
        private TelegramBotService TelegramBotService = new TelegramBotService();

        public HangfireService(
            IVideoRequestService videoRequestService,
            IFileManagement fileManagement,
            IAttachmentService attachmentService)
        {
            VideoRequestService = videoRequestService;
            FileManagement = fileManagement;
            AttachmentService = attachmentService;
        }

        //public string CreateJobForVideoRequestAnswerDeadline(VideoRequest request, string userID)
        //{
        //    string jobID = BackgroundJob.Schedule(() =>
        //        AnswerDeadlineReaches(request.ID, userID),
        //        new DateTimeOffset(request.RequestAnswerDeadline));

        //    return jobID;
        //}

        //public void AnswerDeadlineReaches(int videoRequestID, string userID)
        //{
        //    //try
        //    //{
        //        VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(videoRequestID);
        //        VideoRequestService.AnswerDeadlineReaches(request, userID);
        //    //}
        //    //catch (Exception ex)
        //    //{ }
        //}

//        public void CreateJobForPaymentReminder(VideoRequest request, string userID)
//        {
//#if DEBUG
//            DateTime reminderDeadline = DateTime.Now.AddMinutes(2);
//#else
//            DateTime reminderDeadline = DateTime.Now.AddDays(1);
//#endif
//            request.PaymentReminderJobID = BackgroundJob.Schedule(() =>
//                PaymentReminderReaches(request.ID, userID),
//                new DateTimeOffset(reminderDeadline));

//            VideoRequestService.Update(request, userID);
//        }

//        public void PaymentReminderReaches(int videoRequestID, string userID)
//        {
//            try
//            {
//                VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(videoRequestID);
//                VideoRequestService.SendEmailOnceVideoConfirmed(request);

//                CreateJobForPaymentReminder(request, userID);
//            }
//            catch (Exception ex)
//            { }
//        }

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
                VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(videoRequestID);
                VideoRequestService.VideoDeadlineReaches(request, userID);
            }
            catch (Exception ex)
            {
                string origin = "Video deadline reached exception";
                TelegramBotService.SendMessage(ex.Message, origin);
            }
        }

        //public string CreateJobForVideoRequestPaymentConfirmationDeadline(VideoRequest request, string userID)
        //{
        //    string jobID = BackgroundJob.Schedule(() =>
        //        PaymentConfirmationDeadlineReaches(request.ID, userID),
        //        new DateTimeOffset(request.PaymentConfirmationDeadline.Value));

        //    return jobID;
        //}

        //public void PaymentConfirmationDeadlineReaches(int videoRequestID, string userID)
        //{
        //    //try
        //    //{
        //        VideoRequest request = VideoRequestService.GetActiveSingleDetailsWithRelatedDataByID(videoRequestID);
        //        VideoRequestService.PaymentConfirmationDeadlineReaches(request, userID);
        //    //}
        //    //catch (Exception ex)
        //    //{ }
        //}

        //public string CreateJobForVideoRequestPaymentDeadline(VideoRequest request, string userID)
        //{
        //    string jobID = BackgroundJob.Schedule(() =>
        //        PaymentDeadlineReaches(request.ID, userID),
        //        new DateTimeOffset(request.PaymentDeadline.Value));

        //    return jobID;
        //}

        //public void PaymentDeadlineReaches(int videoRequestID, string userID)
        //{
        //    try
        //    {
        //        VideoRequest request = VideoRequestService.GetByID(videoRequestID);
        //        VideoRequestService.PaymentDeadlineReaches(request, userID);
        //    }
        //    catch (Exception ex)
        //    { }
        //}

        public void CancelJob(string jobID)
        {
            if (!string.IsNullOrWhiteSpace(jobID))
            {
                //try
                //{
                    BackgroundJob.Delete(jobID);
                //}
                //catch
                //{ }
            }
        }

        //public void CreateTaskForConvertingVideo(int attachmentID, string userID)
        //{
        //    try
        //    {
        //        BackgroundJob.Enqueue(() => StartConvertingVideo(attachmentID, userID));
        //    }
        //    catch (Exception ex)
        //    { }
        //}

        //public void StartConvertingVideo(int attachmentID, string userID)
        //{
        //    try
        //    {
        //        Attachment attachment = AttachmentService.GetByID(attachmentID);
        //        string newVideoName = FileManagement.ConvertVideoToMp4(attachment.Path, attachment.GUID + "." + attachment.Extension);
        //        FileManagement.DeleteFile(attachment.Path + "/" + attachment.GUID + "." + attachment.Extension);

        //        string[] tmp = newVideoName.Split('.');
        //        attachment.GUID = tmp[0];
        //        attachment.Extension = tmp[1];

        //        AttachmentService.Update(attachment, userID);
        //    }
        //    catch (Exception ex)
        //    { }
        //}
    }
}
