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

        private int tmpPeriodMinutes = 1440;

        public VideoRequestService(IVideoRequestRepository repository,
                           IUnitOfWork unitOfWork,
                           IEmailService emailService)
            : base(repository, unitOfWork)
        {
            EmailService = emailService;
        }

        public VideoRequest GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return _repository.GetActiveSingleDetailsWithRelatedDataByID(id);
        }

        new public void Add(VideoRequest entity, string creatorID)
        {
            entity.RequestStatusID = (int)VideoRequestStatusEnum.waitingForResponse;
#if DEBUG
            entity.RequestAnswerDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
#else
            entity.RequestAnswerDeadline = DateTime.Now.AddMinutes(2880); //2 days
#endif
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
            //1. set status = expired
            model.RequestStatusID = (int)VideoRequestStatusEnum.requestExpired;
            model.DateRequestExpired = DateTime.Now;
            
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

        /// <summary>
        /// this method is used for cancelling both the REQUEST and VIDEO 
        /// by both CUSTOMER and TALENT
        /// </summary>
        public void Cancel(VideoRequest model, string userID, string userType)
        {
            if (!RequestBelongsToUser(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам запрос");

            if (RequestIsAcceptedAndWaitingForVideo(model))
            {
                if (userType == UserTypesEnum.talent.ToString())
                {
                    model.RequestStatusID = (int)VideoRequestStatusEnum.videoCanceledByTalent;
                    model.DateVideoCanceledByTalent = DateTime.Now;
                }
                else
                {
                    model.RequestStatusID = (int)VideoRequestStatusEnum.videoCanceledByCustomer;
                    model.DateVideoCanceledByCustomer = DateTime.Now;
                }
            }
            else if (RequestIsWaitingForResponse(model))
            {
                if (userType == UserTypesEnum.talent.ToString())
                {
                    model.RequestStatusID = (int)VideoRequestStatusEnum.requestCanceledByTalent;
                    model.DateRequestCanceledByTalent = DateTime.Now;
                }
                else
                {
                    model.RequestStatusID = (int)VideoRequestStatusEnum.requestCanceledByCustomer;
                    model.DateRequestCanceledByCustomer = DateTime.Now;
                }
            }
            else
                throw new Exception("Текущий статус запроса не позволяет отменить его");

            Update(model, userID);
        }

        public void Accept(VideoRequest model, string userID)
        {
            if (!RequestBelongsToUser(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам запрос");

            //if (!TalentsCardPeriodIsValid(model.Talent))
            //    throw new Exception("Срок годности Вашей карты скоро истекает. Просим проверить и обновить");

            if (RequestIsWaitingForResponse(model))
            {
                model.RequestStatusID = (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo;
                model.DateRequestAccepted = DateTime.Now;

#if DEBUG
                model.VideoDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
#else
                model.VideoDeadline = DateTime.Now.AddMinutes(10080); //7 days
#endif
            }
            else
                throw new Exception("Текущий статус запроса не позволяет отменить его");

            Update(model, userID);
        }

        public void VideoDeadlineReaches(VideoRequest model, string userID)
        {
            //0. TO-DO: apply some disciplinary penalty that negatively affects talent's reputation
            //for example, вычесть с его счета долю компании.

            //1. set status = expired
            model.DateVideoExpired = DateTime.Now;
            model.RequestStatusID = (int)VideoRequestStatusEnum.videoExpired;
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

        public void MakePayment(VideoRequest model, string userID)
        {

        }

        private bool RequestBelongsToUser(VideoRequest model, string userID)
        {
            return model.Customer.UserID == userID || model.Talent.UserID == userID;
        }

        private bool RequestIsAcceptedAndWaitingForVideo(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo;
        }

        private bool RequestIsWaitingForResponse(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.waitingForResponse;
        }

        public bool VideoIsUploadable(VideoRequest model)
        {
            return (/*!model.DateVideoCompleted.HasValue &&*/ RequestIsAcceptedAndWaitingForVideo(model));
        }

        public void SaveUploadedVideo(VideoRequest model, string userID)
        {
            model.DateVideoUploaded = DateTime.Now;
            Update(model, userID);
        }

        public bool VideoIsAllowedToBeDeleted(VideoRequest model)
        {
            return RequestIsAcceptedAndWaitingForVideo(model);
        }

        public void SaveDetachedVideo(VideoRequest model, string userID)
        {
            model.DateVideoUploaded = null;
            Update(model, userID);
        }

        public void ConfirmVideo(VideoRequest model, string userID)
        {
            model.RequestStatusID = (int)VideoRequestStatusEnum.videoCompleted;
            model.DateVideoCompleted = DateTime.Now;

            Update(model, userID);

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
    }
}