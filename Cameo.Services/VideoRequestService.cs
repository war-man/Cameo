using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
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
            //entity.AnswerDeadline = DateTime.Now.AddDays(2);

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

            //deadline for request is set using hangfire (2 days)

        }
    }
}