using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class FirebaseRegistrationTokenRepository : BaseCRUDRepository<FirebaseRegistrationToken>, IFirebaseRegistrationTokenRepository
    {
        public FirebaseRegistrationTokenRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}