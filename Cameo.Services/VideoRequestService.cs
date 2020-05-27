using Cameo.Common;
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
        private readonly ICustomerBalanceService CustomerBalanceService;

        private int tmpPeriodMinutes = 2;

        public VideoRequestService(IVideoRequestRepository repository,
                           IUnitOfWork unitOfWork,
                           IEmailService emailService,
                           ITalentBalanceService talentBalanceService,
                           ICustomerBalanceService customerBalanceService)
            : base(repository, unitOfWork)
        {
            EmailService = emailService;
            TalentBalanceService = talentBalanceService;
            CustomerBalanceService = customerBalanceService;
        }

        public VideoRequest GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return _repository.GetActiveSingleDetailsWithRelatedDataByID(id);
        }

        new public void Add(VideoRequest entity, string creatorID)
        {
            entity.ViewedByTalent = false;

            double commission = 0;
            double.TryParse(AppData.Configuration.WebsiteCommission.ToString(), out commission);
            if (commission <= 0)
                commission = 25;

            entity.WebsiteCommission = commission;

            entity.RequestStatusID = (int)VideoRequestStatusEnum.waitingForResponse;
            //entity.RequestStatusID = (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo;
#if DEBUG
            tmpPeriodMinutes = 2880;
            entity.RequestAnswerDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
            //entity.VideoDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
#else
            entity.RequestAnswerDeadline = DateTime.Now.AddMinutes(2880); //2 days
            //entity.VideoDeadline = DateTime.Now.AddMinutes(10080); //7 days
#endif
            base.Add(entity, creatorID);

            //TO-DO: send firebase notification to Talent
        }

        public void AnswerDeadlineReaches(VideoRequest model, string userID)
        {
            //1. set status = expired
            if (model.RequestStatusID != (int)VideoRequestStatusEnum.waitingForResponse)
                return;

            model.RequestStatusID = (int)VideoRequestStatusEnum.requestExpired;
            model.DateRequestExpired = DateTime.Now;

            int requestPrice = CalculateRequestPrice(model);
            CustomerBalanceService.ReplenishBalance(model.Customer, requestPrice);

            Update(model, userID);

            //TO-DO: send firebase notification to Customer
        }

        public void Edit(VideoRequest model, string userID)
        {
            Update(model, userID);

            //send email to Talent
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
            if (!BelongsToUser(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам заказ");

            if (!IsCancelable(model))
                throw new Exception("Текущий статус заказа не позволяет отменить его");

            if (userType == UserTypesEnum.talent.ToString())
            {
                model.RequestStatusID = (int)VideoRequestStatusEnum.canceledByTalent;
                //model.DateVideoCanceledByTalent = DateTime.Now;
                model.DateRequestCanceledByTalent = DateTime.Now;
            }
            else
            {
                model.RequestStatusID = (int)VideoRequestStatusEnum.canceledByCustomer;
                //model.DateVideoCanceledByCustomer = DateTime.Now;
                model.DateRequestCanceledByCustomer = DateTime.Now;
            }

            Update(model, userID);

            if (userType == UserTypesEnum.talent.ToString())
            {
                //TO-DO: send firebase notification to Customer
            }
            else
            {
                //TO-DO: send firebase notification to Talent
            }
        }


        public void Accept(VideoRequest model, string userID)
        {
            if (!BelongsToTalent(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам заказ");

            if (!IsAcceptable(model, userID))
                throw new Exception("Текущий статус заказа не позволяет принять его");

            model.RequestStatusID = (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo;
            model.DateRequestAccepted = DateTime.Now;
#if DEBUG
            tmpPeriodMinutes = 2880;
            model.VideoDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
#else
            model.VideoDeadline = DateTime.Now.AddMinutes(10080); //7 days
#endif
            Update(model, userID);

            //TO-DO: send firebase notification to Customer
        }

        private bool IsAcceptable(VideoRequest model, string userID)
        {
            return IsWaitingForResponse(model);

            //if (!TalentsCardPeriodIsValid(model.Talent))
            //    throw new Exception("Срок годности Вашей карты скоро истекает. Просим проверить и обновить");

            //if (!TalentBalanceService.BalanceAllowsToAcceptRequest(model.Talent.Balance, model.Price))
            //    throw new Exception("Текущий баланс не позволяет принять запрос");
        }


        public void VideoDeadlineReaches(VideoRequest model, string userID)
        {
            //0. TO-DO: apply some disciplinary penalty that negatively affects talent's reputation
            //for example, вычесть с его счета долю компании.

            //1. set status = expired
            model.DateVideoExpired = DateTime.Now;
            model.RequestStatusID = (int)VideoRequestStatusEnum.videoExpired;
            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            //TO-DO: send firebase notification to Talent
        }

        public void PaymentConfirmationDeadlineReaches(VideoRequest model, string userID)
        {
            //1. set status = expired
            model.DatePaymentConfirmationExpired = DateTime.Now;
            model.RequestStatusID = (int)VideoRequestStatusEnum.paymentConfirmationExpired;
            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            //TO-DO: send firebase notification to Talent
        }

        //public void MakePayment(VideoRequest model, string userID)
        //{
        //    if (!BelongsToUser(model, userID))
        //        throw new Exception("Вы обрабатываете не принадлежащий Вам запрос");

        //    int amountToBeTakenOff = TalentBalanceService
        //        .CalculateMoneyThatTalentPaysToSystemForCameo(model.Price, model.WebsiteCommission);

        //    TalentBalanceService.TakeOffBalance(model.Talent, amountToBeTakenOff, userID);

        //    model.DatePaid = DateTime.Now;
        //    model.RequestStatusID = (int)VideoRequestStatusEnum.paymentScreenshotUploaded;
        //    Update(model, userID);

        //    //send emails to customer and talent
        //    string toCustomer = "cortex91@inbox.ru";
        //    string subjectCustomer = "Subject - Customer";
        //    string bodyCustomer = "This is email for Customer";

        //    EmailService.Send(toCustomer, subjectCustomer, bodyCustomer);

        //    string toTalent = "xenon1991@inbox.ru";
        //    string subjectTalent = "Subject - Talent";
        //    string bodyTalent = "This is email for Talent";

        //    EmailService.Send(toTalent, subjectTalent, bodyTalent);
        //}

        private bool BelongsToUser(VideoRequest model, string userID)
        {
            return model.Customer.UserID == userID || model.Talent.UserID == userID;
        }

        private bool IsWaitingForResponse(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.waitingForResponse;
        }

        private bool IsRequestAcceptedAndWaitingForVideo(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo;
        }
        
        public bool IsPaymentScreenshotUploaded(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.paymentScreenshotUploaded;
        }

        private bool RequestIsNotPublic(VideoRequest model)
        {
            return model.IsNotPublic;
        }

        public bool IsVideoUploadable(VideoRequest model)
        {
            return (/*!model.DateVideoCompleted.HasValue &&*//*RequestIsWaitingForResponse(model) 
                || */IsRequestAcceptedAndWaitingForVideo(model));
        }

        public void SaveUploadedVideo(VideoRequest model, string userID)
        {
            model.DateVideoUploaded = DateTime.Now;
            Update(model, userID);
        }

        public void SaveUploadedPaymentScreenshot(VideoRequest model, string userID)
        {
            model.RequestStatusID = (int)VideoRequestStatusEnum.paymentScreenshotUploaded;
            model.DatePaymentScreenshotUploaded = DateTime.Now;
#if DEBUG
            tmpPeriodMinutes = 2880;
            model.PaymentConfirmationDeadline = DateTime.Now.AddMinutes(tmpPeriodMinutes);
#else
            model.PaymentConfirmationDeadline = DateTime.Now.AddMinutes(2880); //2 days
            //model.PaymentConfirmationDeadline = DateTime.Now.AddMinutes(10080); //7 days
#endif
            Update(model, userID);
        }

        public bool VideoIsAllowedToBeDeleted(VideoRequest model)
        {
            //return RequestIsAcceptedAndWaitingForVideo(model);
            //return VideoIsUploadable(model);
            return (model.VideoID.HasValue && IsRequestAcceptedAndWaitingForVideo(model));
        }

        public bool IsVideoConfirmed(VideoRequest model)
        {
            return (model.VideoID.HasValue
                && model.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted);
        }

        public void SaveDetachedVideo(VideoRequest model, string userID)
        {
            model.DateVideoUploaded = null;
            Update(model, userID);
        }

        public void ConfirmVideo(VideoRequest model, string userID)
        {
            if (!BelongsToTalent(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам заказ");

            if (!IsVideoConfirmable(model))
                throw new Exception("Текущий статус заказа не позволяет подтвердить видео");

            model.RequestStatusID = (int)VideoRequestStatusEnum.videoCompleted;
            model.DateVideoCompleted = DateTime.Now;

//#if DEBUG
//            model.PaymentDeadline = DateTime.Now.AddMinutes(2);
//#else
//            model.PaymentDeadline = DateTime.Now.AddMinutes(10080); //7 days
//#endif

            Update(model, userID);

            SendEmailOnceVideoConfirmed(model);
        }

        private bool IsVideoConfirmable(VideoRequest model)
        {
            return (model.VideoID.HasValue
                && model.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo);
        }

        public void ConfirmPayment(VideoRequest model, string userID)
        {
            if (!BelongsToTalent(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам заказ");

            if (!IsPaymentConfirmable(model))
                throw new Exception("Текущий статус заказа не позволяет подтвердить видео");

            model.RequestStatusID = (int)VideoRequestStatusEnum.paymentConfirmed;
            model.DatePaymentConfirmed = DateTime.Now;

            Update(model, userID);

            //TO-DO: send firebase notification to Customer
        }

        private bool IsPaymentConfirmable(VideoRequest model)
        {
            return model.VideoID.HasValue;
        }

        public void SendEmailOnceVideoConfirmed(VideoRequest model)
        {
            //send email to Customer
            string toCustomer = "cortex91@inbox.ru";
            string subjectCustomer = "Subject - Customer";
            string bodyCustomer = "This is email for Customer";

            EmailService.Send(toCustomer, subjectCustomer, bodyCustomer);
        }

        public bool IsPaymentConfirmed(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.paymentConfirmed;
        }

        public VideoRequest GetSinglePublished(int id, string userID)
        {
            var model = GetActiveSingleDetailsWithRelatedDataByID(id);
            if (model == null)
                return null;

            if (BelongsToTalent(model, userID))
                return model;
            else if (IsPaymentConfirmed(model))
            {
                if (BelongsToCustomer(model, userID))
                    return model;
                else if (!RequestIsNotPublic(model))
                    return model;
            }

            return null;

            //if (model.Talent.UserID == userID)
            //    return model;
            //else if (IsPaymentConfirmed(model))
            //{
            //    //if unauthorized
            //    if (string.IsNullOrWhiteSpace(userID))
            //    {
            //        if (!RequestIsNotPublic(model))
            //            return model;
            //    }
            //    //if request belongs to current Customer
            //    else if (model.Customer.UserID == userID)
            //        return model;
            //}
            
            //return null;
        }

        public VideoRequest GetIncompletedVideo(int id, string userID)
        {
            var model = GetActiveSingleDetailsWithRelatedDataByID(id);
            if (model == null)
                return null;

            //if request belongs to current Talent
            //if (model.Talent.UserID == userID)
            if (BelongsToTalent(model, userID))
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

        public bool IsCancelable(VideoRequest model)
        {
            //return RequestIsAcceptedAndWaitingForVideo(model);
            return IsWaitingForResponse(model);
        }

        public bool IsEditable(VideoRequest model)
        {
            //return RequestIsAcceptedAndWaitingForVideo(model);
            return IsWaitingForResponse(model);
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
                        || m.RequestStatusID == (int)VideoRequestStatusEnum.paymentScreenshotUploaded));
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
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.paymentScreenshotUploaded));
        }

        public IQueryable<VideoRequest> GetAllPaidByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Where(m => m.TalentID == talent.ID
                    && m.RequestStatusID == (int)VideoRequestStatusEnum.paymentScreenshotUploaded);
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
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo));
        }

        public VideoRequest GetRandomSinglePublishedByTalent(Talent talent, string userID)
        {
            List<int> ids = GetPublicForTalent(talent)
                .Select(m => m.ID)
                .ToList();

            if (ids.Count == 0)
                return null;

            var rand = new Random();
            int randomIndex = rand.Next(0, ids.Count);
            return GetSinglePublished(ids[randomIndex], userID);
        }

        public IQueryable<VideoRequest> GetPublicForTalent(Talent talent, int requestIDToBeExcluded = 0)
        {
            return GetAllPaidByTalent(talent)
                .Where(m => !m.IsNotPublic && m.ID != requestIDToBeExcluded);
        }

        public int CalculateRequestPrice(Talent talent)
        {
            int price = talent.Price;

            double websiteCommission = 0;
            double.TryParse(AppData.Configuration.WebsiteCommission.ToString(), out websiteCommission);
            if (websiteCommission <= 0)
                websiteCommission = 25;

            return CalculateRequestPrice(talent.Price, websiteCommission);
        }

        public int CalculateRequestPrice(VideoRequest request)
        {
            return CalculateRequestPrice(request.Price, request.WebsiteCommission);
        }

        private int CalculateRequestPrice(int price, double websiteCommission)
        {
            double requestPriceDouble = ((101 * websiteCommission - 100) / 10000) * price;
            double requestPriceDouble2 = (0.25 - (0.75 * 0.01)) * price;

            int requestPriceInt = ((int)(requestPriceDouble / 1000)) * 1000;

            return requestPriceInt;
        }

        public int CalculateRemainingPrice(int price)
        {
            double websiteCommission = 0;
            double.TryParse(AppData.Configuration.WebsiteCommission.ToString(), out websiteCommission);
            if (websiteCommission <= 0)
                websiteCommission = 25;

            return CalculateRemainingPrice(price, websiteCommission);
        }

        public int CalculateRemainingPrice(int price, double websiteCommission)
        {
            int remainingPrice = (int)(((100.0 - websiteCommission) / 100) * price);

            return remainingPrice;
        }
    }
}