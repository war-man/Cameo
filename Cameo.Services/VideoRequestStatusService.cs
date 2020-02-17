using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;

namespace Cameo.Services
{
    public class VideoRequestStatusService : BaseDropdownableService<VideoRequestStatus>, IVideoRequestStatusService
    {
        public VideoRequestStatusService(IVideoRequestStatusRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}