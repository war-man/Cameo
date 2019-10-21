using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;

namespace Cameo.Services
{
    public class VideoRequestService : BaseCRUDService<VideoRequest>, IVideoRequestService
    {
        private readonly IEmailService EmailService;

        public VideoRequestService(IVideoRequestRepository repository,
                           IUnitOfWork unitOfWork,
                           IEmailService emailService)
            : base(repository, unitOfWork)
        {
            EmailService = emailService;
        }

        new public void Add(VideoRequest entity, string creatorID)
        {
            entity.RequestStatusID = (int)VideoRequestStatusEnum.waitingForResponse;

            //entity.AnswerDeadline = DateTime.Now.AddMinutes(2880); //2 days
            entity.AnswerDeadline = DateTime.Now.AddMinutes(2);
            base.Add(entity, creatorID);

            //send email to Customer and Talent
            string toCustomer = "cortex91@inbox.ru";
            string subjectCustomer = "Subject - Customer";
            string bodyCustomer = "This is email for Customer";

            EmailService.Send(toCustomer, subjectCustomer, bodyCustomer);

            string toTalent = "xenon1991@inbox.ru";
            string subjectTalent = "Subject - Talent";
            string bodyTalent = "This is email for Talent";

            EmailService.Send(toTalent, subjectTalent, bodyTalent);
        }

        public void AnswerDeadlineReaches(VideoRequest model, string userID)
        {
            //1. set answerStatus = expired
            model.DateRequestExpired = DateTime.Now;
            model.RequestStatusID = (int)VideoRequestStatusEnum.requestExpired;
            Update(model, userID);

            //2. send emails to customer and talent
            string toCustomer = "cortex91@inbox.ru";
            string subjectCustomer = "Subject - Customer";
            string bodyCustomer = "This is email for Customer";

            EmailService.Send(toCustomer, subjectCustomer, bodyCustomer);

            string toTalent = "xenon1991@inbox.ru";
            string subjectTalent = "Subject - Talent";
            string bodyTalent = "This is email for Talent";

            EmailService.Send(toTalent, subjectTalent, bodyTalent);
        }
    }
}