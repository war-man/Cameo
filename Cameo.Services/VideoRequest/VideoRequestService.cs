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
        //private readonly IEmailService EmailService;
        private readonly ITalentBalanceService TalentBalanceService;
        private readonly ICustomerBalanceService CustomerBalanceService;
        private readonly IVideoRequestPriceCalculationsService VideoRequestPriceCalculationsService;
        private readonly IFirebaseRegistrationTokenService FirebaseRegistrationTokenService;

        private int tmpPeriodMinutes = 2;

        public VideoRequestService(IVideoRequestRepository repository,
            IUnitOfWork unitOfWork,
            ITalentBalanceService talentBalanceService,
            ICustomerBalanceService customerBalanceService,
            IVideoRequestPriceCalculationsService videoRequestPriceCalculationsService,
            IFirebaseRegistrationTokenService firebaseRegistrationTokenService)
            : base(repository, unitOfWork)
        {
            TalentBalanceService = talentBalanceService;
            CustomerBalanceService = customerBalanceService;
            VideoRequestPriceCalculationsService = videoRequestPriceCalculationsService;
            FirebaseRegistrationTokenService = firebaseRegistrationTokenService;
        }

        public VideoRequest GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return _repository.GetActiveSingleDetailsWithRelatedDataByID(id);
        }

        new public void Add(VideoRequest entity, string creatorID)
        {
            entity.ViewedByCustomer = true;
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
            entity.RequestAnswerDeadline = RoundToUp(DateTime.Now.AddMinutes(tmpPeriodMinutes));
            //entity.VideoDeadline = RoundToUp(DateTime.Now.AddMinutes(tmpPeriodMinutes));
#else
            entity.RequestAnswerDeadline = RoundToUp(DateTime.Now.AddMinutes(2880)); //2 days
            //entity.VideoDeadline = RoundToUp(DateTime.Now.AddMinutes(10080)); //7 days
#endif
            base.Add(entity, creatorID);

            //TO-DO: send firebase notification to Talent
            string title = "Новый заказ";
            string body = entity.Instructions;
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = entity.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(entity.Talent.UserID, title, body, data);
        }

        public void AnswerDeadlineReaches(VideoRequest model, string userID)
        {
            //1. set status = expired
            if (!IsWaitingForResponse(model))
                return;

            //if (model.RequestStatusID != (int)VideoRequestStatusEnum.waitingForResponse)
            //    return;

            model.RequestStatusID = (int)VideoRequestStatusEnum.requestExpired;
            model.DateRequestExpired = DateTime.Now;

            int requestPrice = VideoRequestPriceCalculationsService.CalculateRequestPrice(model);
            CustomerBalanceService.ReplenishBalance(model.Customer, requestPrice);

            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            string title = model.Talent.FullName;
            string body = "Срок ожидания ответа истёк";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Customer.UserID, title, body, data);
        }

        public void Edit(VideoRequest model, string userID)
        {
            Update(model, userID);

            //TO-DO: send firebase notification to Talent
            string title = model.Customer.FullName;
            string body = "Клиент внес изменения в заказ";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Talent.UserID, title, body, data);
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

                model.ViewedByCustomer = false;
                model.ViewedByTalent = true;
            }
            else
            {
                model.RequestStatusID = (int)VideoRequestStatusEnum.canceledByCustomer;
                //model.DateVideoCanceledByCustomer = DateTime.Now;
                model.DateRequestCanceledByCustomer = DateTime.Now;

                model.ViewedByCustomer = true;
                model.ViewedByTalent = false;
            }

            Update(model, userID);

            string title = "";
            string body = "";
            string adresatUserID = "";
            if (userType == UserTypesEnum.talent.ToString())
            {
                //TO-DO: send firebase notification to Customer
                title = model.Talent.FullName;
                body = "Талант отменил Ваш заказ";
                adresatUserID = model.Customer.UserID;
            }
            else
            {
                //TO-DO: send firebase notification to Talent
                title = model.Customer.FullName;
                body = "Клиент отменил Ваш заказ";
                adresatUserID = model.Talent.UserID;
            }

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(adresatUserID, title, body, data);
        }

        public void Accept(VideoRequest model, string userID)
        {
            if (!BelongsToTalent(model, userID))
                throw new Exception("Вы обрабатываете не принадлежащий Вам заказ");

            if (!IsAcceptable(model, userID))
                throw new Exception("Текущий статус заказа не позволяет принять его");

            model.ViewedByCustomer = false;
            model.ViewedByTalent = true;

            model.RequestStatusID = (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo;
            model.DateRequestAccepted = DateTime.Now;
#if DEBUG
            tmpPeriodMinutes = 2880;
            model.VideoDeadline = RoundToUp(DateTime.Now.AddMinutes(tmpPeriodMinutes));
#else
            model.VideoDeadline = RoundToUp(DateTime.Now.AddMinutes(10080)); //7 days
#endif
            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            string title = model.Talent.FullName;
            string body = "Талант принял Ваш заказ";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Customer.UserID, title, body, data);
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

            if (!IsRequestAcceptedAndWaitingForVideo(model))
                return;

            model.DateVideoExpired = DateTime.Now;
            model.RequestStatusID = (int)VideoRequestStatusEnum.videoExpired;

            int requestPrice = VideoRequestPriceCalculationsService.CalculateRequestPrice(model);
            CustomerBalanceService.ReplenishBalance(model.Customer, requestPrice);

            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            string title = model.Talent.FullName;
            string body = "Срок ожидания видео истёк";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Customer.UserID, title, body, data);

            //TO-DO: send firebase notification to Talent
            title = model.Customer.FullName;
            body = "Срок ожидания видео истёк";
            //body += "Убедительно просим больше так не поступать";
            FirebaseRegistrationTokenService.SendNotification(model.Talent.UserID, title, body, data);
        }

        public void PaymentConfirmationDeadlineReaches(VideoRequest model, string userID)
        {
            if (!IsPaymentScreenshotUploaded(model))
                return;

            model.DatePaymentConfirmationExpired = DateTime.Now;
            model.RequestStatusID = (int)VideoRequestStatusEnum.paymentConfirmationExpired;

            int requestPrice = VideoRequestPriceCalculationsService.CalculateRequestPrice(model);
            CustomerBalanceService.ReplenishBalance(model.Customer, requestPrice);

            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            string title = model.Talent.FullName;
            string body = "Срок ожидания подтверждения оплаты истёк";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Customer.UserID, title, body, data);

            //TO-DO: send firebase notification to Talent
            title = model.Customer.FullName;
            body = "Срок ожидания подтверждения оплаты истёк";
            //body += "Убедительно просим больше так не поступать";
            FirebaseRegistrationTokenService.SendNotification(model.Talent.UserID, title, body, data);
        }

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

        private bool IsNotPublic(VideoRequest model)
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
            model.ViewedByCustomer = true;
            model.ViewedByTalent = false;

            model.RequestStatusID = (int)VideoRequestStatusEnum.paymentScreenshotUploaded;
            model.DatePaymentScreenshotUploaded = DateTime.Now;
#if DEBUG
            tmpPeriodMinutes = 2880;
            model.PaymentConfirmationDeadline = RoundToUp(DateTime.Now.AddMinutes(tmpPeriodMinutes));
#else
            model.PaymentConfirmationDeadline = RoundToUp(DateTime.Now.AddMinutes(2880)); //2 days
            //model.PaymentConfirmationDeadline = RoundToUp(DateTime.Now.AddMinutes(10080)); //7 days
#endif
            Update(model, userID);

            //TO-DO: send firebase notification to Talent
            string title = model.Customer.FullName;
            string body = "Клиент загрузил скрин, подтверждающий оплату. Пожалуйста, подтвердите.";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Talent.UserID, title, body, data);
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

            model.ViewedByCustomer = false;
            model.ViewedByTalent = true;

            model.RequestStatusID = (int)VideoRequestStatusEnum.videoCompleted;
            model.DateVideoCompleted = DateTime.Now;

            //#if DEBUG
            //            model.PaymentDeadline = RoundToUp(DateTime.Now.AddMinutes(2));
            //#else
            //            model.PaymentDeadline = RoundToUp(DateTime.Now.AddMinutes(10080)); //7 days
            //#endif

            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            string title = model.Talent.FullName;
            string body = "Талант загрузил видео! Пожалуйсте, переведите оставшуюся сумму на карту " + model.Talent.CreditCardNumber + " " + model.Talent.CreditCardHolder +" и загрузите скрин оплаты.";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Customer.UserID, title, body, data);
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

            model.ViewedByCustomer = false;
            model.ViewedByTalent = true;

            model.RequestStatusID = (int)VideoRequestStatusEnum.paymentConfirmed;
            model.DatePaymentConfirmed = DateTime.Now;

            Update(model, userID);

            //TO-DO: send firebase notification to Customer
            string title = model.Talent.FullName;
            string body = "Видео ГОТОВО!";
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["request_id"] = model.ID.ToString()
            };
            FirebaseRegistrationTokenService.SendNotification(model.Customer.UserID, title, body, data);
        }

        private bool IsPaymentConfirmable(VideoRequest model)
        {
            return model.VideoID.HasValue;
        }

        //public void SendEmailOnceVideoConfirmed(VideoRequest model)
        //{
        //    //send email to Customer
        //    string toCustomer = "cortex91@inbox.ru";
        //    string subjectCustomer = "Subject - Customer";
        //    string bodyCustomer = "This is email for Customer";

        //    EmailService.Send(toCustomer, subjectCustomer, bodyCustomer);
        //}

        public bool IsPaymentConfirmed(VideoRequest model)
        {
            return model.RequestStatusID == (int)VideoRequestStatusEnum.paymentConfirmed;
        }

        public VideoRequest GetSinglePublished(int id, string userID)
        {
            var model = GetActiveSingleDetailsWithRelatedDataByID(id);
            if (model == null)
                return null;

            if (IsPaymentConfirmed(model))
            {
                if (BelongsToTalent(model, userID))
                    return model;
                else if (BelongsToCustomer(model, userID))
                    return model;
                else if (!IsNotPublic(model))
                    return model;
            }
            
            return null;
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

        /// <summary>
        /// Get all requests payment for which is cnfirmed
        /// </summary>
        /// <param name="talent"></param>
        /// <returns></returns>
        private IQueryable<VideoRequest> GetAllPaidByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Where(m => m.TalentID == talent.ID
                    && m.RequestStatusID == (int)VideoRequestStatusEnum.paymentConfirmed);
        }

        public IQueryable<VideoRequest> GetPublicByTalent(Talent talent, int requestIDToBeExcluded = 0)
        {
            return GetAllPaidByTalent(talent)
                .Where(m => !m.IsNotPublic && m.ID != requestIDToBeExcluded)
                .OrderByDescending(m => m.ID);
        }

        private DateTime RoundToUp(DateTime inputDateTime)
        {
            return inputDateTime.Date.AddDays(1).AddSeconds(-1);
        }
    }
}