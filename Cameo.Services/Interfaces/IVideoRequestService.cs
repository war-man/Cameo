using Cameo.Models;

namespace Cameo.Services.Interfaces
{
    public interface IVideoRequestService : IBaseCRUDService<VideoRequest>
    {
        void AnswerDeadlineReaches(VideoRequest model);
    }
}