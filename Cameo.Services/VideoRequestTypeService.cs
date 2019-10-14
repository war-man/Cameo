using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;

namespace Cameo.Services
{
    public class VideoRequestTypeService : BaseDropdownableService<VideoRequestType>, IVideoRequestTypeService
    {
        public VideoRequestTypeService(IVideoRequestTypeRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}