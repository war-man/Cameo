using Cameo.Models;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface IVideoRequestService : IBaseCRUDService<VideoRequest>
    {
        VideoRequest GetActiveSingleDetailsWithRelatedDataByID(int id);

        void AnswerDeadlineReaches(VideoRequest model, string userID);

        /// <summary>
        /// this method is used for cancelling both the REQUEST and VIDEO 
        /// by both CUSTOMER and TALENT
        /// </summary>
        void Cancel(VideoRequest model, string userID, string userType);

        void Accept(VideoRequest model, string userID);

        void VideoDeadlineReaches(VideoRequest model, string userID);

        void MakePayment(VideoRequest model, string userID);

        void PaymentDeadlineReaches(VideoRequest model, string userID);

        bool VideoIsUploadable(VideoRequest model);

        void SaveUploadedVideo(VideoRequest model, string userID);

        bool VideoIsAllowedToBeDeleted(VideoRequest model);

        void SaveDetachedVideo(VideoRequest model, string userID);

        void ConfirmVideo(VideoRequest model, string userID);

        VideoRequest GetSinglePublished(int id, string userID);

        bool BelongsToCustomer(VideoRequest model, string userID);
    }
}