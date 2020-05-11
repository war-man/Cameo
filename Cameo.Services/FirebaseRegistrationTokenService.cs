using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System.IO;
using System.Linq;

namespace Cameo.Services
{
    public class FirebaseRegistrationTokenService : BaseCRUDService<FirebaseRegistrationToken>, IFirebaseRegistrationTokenService
    {
        public FirebaseRegistrationTokenService(IFirebaseRegistrationTokenRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}