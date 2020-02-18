using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services
{
    public class VideoRequestService : BaseCRUDService<VideoRequest>, IVideoRequestService
    {
        private readonly IEmailService EmailService;
        private readonly ITalentBalanceService TalentBalanceService;

        private int tmpPeriodMinutes = 1440;

        public VideoRequestService(IVideoRequestRepository repository,
                           IUnitOfWork unitOfWork,
                           IEmailService emailService,
                           ITalentBalanceService talentBalanceService)
            : base(repository, unitOfWork)
        {
            EmailService = emailService;
            TalentBalanceService = talentBalanceService;
        }

        public VideoRequest GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return _repository.GetActiveSingleDetailsWithRelatedDataByID(id);
        }

        new public void Add(VideoRequest entity, string creatorID)
        {
            //entity.RequestStatusID = (int)VideoRequestStatusEnum.waitingForResponse;
            entity.RequestStatusID = (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo;
#if DEBUG
            //entity.RequestAnswerDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
            entity.VideoDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
#else
            //entity.RequestAnswerDeadline = DateTime.Now.AddMinutes(2880); //2 days
            entity.VideoDeadline = DateTime.Now.AddMinutes(10080); //7 days
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

        //public void AnswerDeadlineReaches(VideoRequest model, string userID)
        //{
        //    //1. set status = expired
        //    model.RequestStatusID = (int)VideoRequestStatusEnum.requestExpired;
        //    model.DateRequestExpired = DateTime.Now;
            
        //    Update(model, userID);

        //    //2. send emails to customer and talent
        //    string toCustomer = "cortex91@inbox.ru";
        //    string subjectCustomer = "Subject - Customer";
        //    string bodyCustomer = "This is email for Customer";

        //    EmailService.Send(toCustomer, subjectCustomer, bodyCustomer);

        //    string toTalent = "xenon1991@inbox.ru";
        //    string subjectTalent = "Subject - Talent";
        //    string bodyTalent = "This is email for Talent";

        //    EmailService.Send(toTalent, subjectTalent, bodyTalent);
        //}

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
            //else if (RequestIsWaitingForResponse(model))
            //{
            //    if (userType == UserTypesEnum.talent.ToString())
            //    {
            //        model.RequestStatusID = (int)VideoRequestStatusEnum.requestCanceledByTalent;
            //        model.DateRequestCanceledByTalent = DateTime.Now;
            //    }
            //    else
            //    {
            //        model.RequestStatusID = (int)VideoRequestStatusEnum.requestCanceledByCustomer;
            //        model.DateRequestCanceledByCustomer = DateTime.Now;
            //    }
            //}
            else
                throw new Exception("Текущий статус запроса не позволяет отменить его");

            Update(model, userID);
        }

//        public void Accept(VideoRequest model, string userID)
//        {
//            CheckIfRequestIsAcceptable(model, userID);

//            model.RequestStatusID = (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo;
//            model.DateRequestAccepted = DateTime.Now;
//#if DEBUG
//            model.VideoDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
//#else
//            model.VideoDeadline = DateTime.Now.AddMinutes(10080); //7 days
//#endif

//            Update(model, userID);
//        }

        //private void CheckIfRequestIsAcceptable(VideoRequest model, string userID)
        //{
        //    if (!RequestBelongsToUser(model, userID))
        //        throw new Exception("Вы обрабатываете не принадлежащий Вам запрос");

        //    //if (!TalentsCardPeriodIsValid(model.Talent))
        //    //    throw new Exception("Срок годности Вашей карты скоро истекает. Просим проверить и обновить");

        //    if (!TalentBalanceService.BalanceAllowsToAcceptRequest(model.Talent.Balance, model.Price))
        //        throw new Exception("Текущий баланс не позволяет принять запрос");

        //    if (!RequestIsWaitingForResponse(model))
        //        throw new Exception("Текущий статус запроса не позволяет отменить его");
        //}

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
            if (!RequestBelongsToUser(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам запрос");

            model.DatePaid = DateTime.Now;
            model.RequestStatusID = (int)VideoRequestStatusEnum.videoPaid;
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

        //public void PaymentDeadlineReaches(VideoRequest model, string userID)
        //{
        //    //1. set status = expired
        //    model.RequestStatusID = (int)VideoRequestStatusEnum.videoPaymentExpired;
        //    model.DatePaymentExpired = DateTime.Now;

        //    Update(model, userID);

        //    //2. send emails to customer and talent
        //    string toCustomer = "cortex91@inbox.ru";
        //    string subjectCustomer = "Subject - Customer";
        //    string bodyCustomer = "This is email for Customer";

        //    EmailService.Send(toCustomer, subjectCustomer, bodyCustomer);

        //    string toTalent = "xenon1991@inbox.ru";
        //    string subjectTalent = "Subject - Talent";
        //    string bodyTalent = "This is email for Talent";

        //    EmailService.Send(toTalent, subjectTalent, bodyTalent);
        //}

        private bool RequestBelongsToUser(VideoRequest model, string userID)
        {
            return model.Customer.UserID == userID || model.Talent.UserID == userID;
        }

        private bool RequestIsAcceptedAndWaitingForVideo(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo;
        }

        //private bool RequestIsWaitingForResponse(VideoRequest model)
        //{
        //    return model.RequestStatusID == (int)VideoRequestStatusEnum.waitingForResponse;
        //}

        private bool RequestIsPaid(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.videoPaid;
        }

        public bool VideoIsUploadable(VideoRequest model)
        {
            return (/*!model.DateVideoCompleted.HasValue &&*//*RequestIsWaitingForResponse(model) 
                || */RequestIsAcceptedAndWaitingForVideo(model));
        }

        public void SaveUploadedVideo(VideoRequest model, string userID)
        {
            model.DateVideoUploaded = DateTime.Now;
            Update(model, userID);
        }

        public bool VideoIsAllowedToBeDeleted(VideoRequest model)
        {
            //return RequestIsAcceptedAndWaitingForVideo(model);
            return VideoIsUploadable(model);
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

#if DEBUG
            model.PaymentDeadline = DateTime.Now.AddMinutes(2);
#else
            model.PaymentDeadline = DateTime.Now.AddMinutes(10080); //7 days
#endif

            Update(model, userID);

            SendEmailOnceVideoConfirmed(model);
        }

        public void SendEmailOnceVideoConfirmed(VideoRequest model)
        {
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

        public VideoRequest GetSinglePublished(int id, string userID)
        {
            var model = GetActiveSingleDetailsWithRelatedDataByID(id);
            if (model == null)
                return null;

            if (RequestIsPaid(model))
            {
                //if request belongs to current Talent
                if (model.Talent.UserID == userID)
                    return model;
                else
                {
                    //if unauthorized
                    if (string.IsNullOrWhiteSpace(userID))
                    {
                        if (!model.IsNotPublic)
                            return model;
                    }
                    //if request belongs to current Customer
                    else if (model.Customer.UserID == userID)
                        return model;
                }
            }
            
            return null;
        }

        public VideoRequest GetIncompletedVideo(int id, string userID)
        {
            var model = GetActiveSingleDetailsWithRelatedDataByID(id);
            if (model == null)
                return null;

            //if request belongs to current Talent
            if (model.Talent.UserID == userID)
                return model;
            else
                return null;
        }

        public bool BelongsToCustomer(VideoRequest model, string userID)
        {
            return model?.Customer?.UserID?.Equals(userID) ?? false;
        }

        public bool BelongsToTalent(VideoRequest model, string userID)
        {
            return model?.Talent?.UserID?.Equals(userID) ?? false;
        }

        public bool RequestIsAllowedToBeEdited(VideoRequest model)
        {
            return RequestIsAcceptedAndWaitingForVideo(model);
        }

        public int GetAllCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable().Count(m => m.TalentID == talent.ID);
        }

        public int GetCompletedCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Count(m => m.TalentID == talent.ID
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted
                        || m.RequestStatusID == (int)VideoRequestStatusEnum.videoPaid));
        }

        public int GetCompletenessPercentageByTalent(Talent talent)
        {
            int requestsTotal = GetAllCountByTalent(talent);
            int requestsCompleted = GetCompletedCountByTalent(talent);

            return (requestsCompleted * 100) / requestsTotal;
        }

        public int GetPaidCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Count(m => m.TalentID == talent.ID
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.videoPaid));
        }

        public IQueryable<VideoRequest> GetAllPaidByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Where(m => m.TalentID == talent.ID
                    && m.RequestStatusID == (int)VideoRequestStatusEnum.videoPaid);
        }

        //later siteStavka and amount, that talent earns will be saved for each request
        public int GetEarnedByTalent(Talent talent)
        {
            int totalPaid = GetAllPaidByTalent(talent).Sum(m => m.Price);
            int siteStavka = 25; //25%
            return totalPaid * (100 - siteStavka) / 100;
        }

        public int GetWaitingCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Count(m => m.TalentID == talent.ID
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo));
        }

        public VideoRequest GetRandomSinglePublishedByTalent(Talent talent, string userID)
        {
            List<int> ids = GetAllPaidByTalent(talent)
                .Where(m => !m.IsNotPublic)
                .Select(m => m.ID)
                .ToList();

            if (ids.Count == 0)
                return null;

            var rand = new Random();
            int randomIndex = rand.Next(0, ids.Count);
            return GetSinglePublished(ids[randomIndex], userID);
        }
    }
}