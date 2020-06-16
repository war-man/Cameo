using Cameo.Models;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface IVideoRequestService : IBaseCRUDService<VideoRequest>
    {
        void Edit(VideoRequest model, string userID);
        VideoRequest GetActiveSingleDetailsWithRelatedDataByID(int id);

        void AnswerDeadlineReaches(VideoRequest model, string userID);

        /// <summary>
        /// this method is used for cancelling both the REQUEST and VIDEO 
        /// by both CUSTOMER and TALENT
        /// </summary>
        void Cancel(VideoRequest model, string userID, string userType);

        void Accept(VideoRequest model, string userID);

        void VideoDeadlineReaches(VideoRequest model, string userID);

        //void MakePayment(VideoRequest model, string userID);

        //void PaymentDeadlineReaches(VideoRequest model, string userID);

        bool IsVideoUploadable(VideoRequest model);

        void SaveUploadedVideo(VideoRequest model, string userID);

        bool VideoIsAllowedToBeDeleted(VideoRequest model);

        void SaveDetachedVideo(VideoRequest model, string userID);

        void ConfirmVideo(VideoRequest model, string userID);
        //void SendEmailOnceVideoConfirmed(VideoRequest model);

        VideoRequest GetSinglePublished(int id, string userID);
        VideoRequest GetIncompletedVideo(int id, string userID);

        bool BelongsToCustomer(VideoRequest model, string userID);
        bool BelongsToTalent(VideoRequest model, string userID);

        bool IsEditable(VideoRequest model);
        bool IsCancelable(VideoRequest model);
        bool IsPaymentConfirmed(VideoRequest model);
        IQueryable<VideoRequest> GetPublicByTalent(Talent talent, int requestIDToBeExcluded);

        //int CalculateRequestPrice(Talent talent);
        //int CalculateRequestPrice(VideoRequest request);
        //int CalculateRemainingPrice(int price, double websiteCommission);
        bool IsVideoConfirmed(VideoRequest model);
        void SaveUploadedPaymentScreenshot(VideoRequest model, string userID);
        void PaymentConfirmationDeadlineReaches(VideoRequest model, string userID);
        bool IsPaymentScreenshotUploaded(VideoRequest model);
        void ConfirmPayment(VideoRequest model, string userID);
    }
}