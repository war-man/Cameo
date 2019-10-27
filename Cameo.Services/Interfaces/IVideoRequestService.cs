using Cameo.Models;

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
    }
}