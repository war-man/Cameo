using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;

namespace Cameo.Services
{
    public class SocialAreaService : BaseDropdownableService<SocialArea>, ISocialAreaService
    {
        public SocialAreaService(ISocialAreaRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}