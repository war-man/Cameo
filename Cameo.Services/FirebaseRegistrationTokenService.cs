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

        public void SaveToken(string token, string userID, string frontType)
        {
            FirebaseRegistrationToken tokenObj = new FirebaseRegistrationToken()
            {
                UserID = userID,
                Token = token,
                FrontType = frontType
            };

            Add(tokenObj, userID);
        }

        public string GetByUserID(string userID)
        {
            var tokenObj = GetAsIQueryable().Where(m => m.UserID == userID).FirstOrDefault();

            return tokenObj?.Token;
        }

        public string GetForWebByUserID(string userID)
        {
            var tokenObj = GetAsIQueryable().Where(m => m.FrontType == "web" && m.UserID == userID).FirstOrDefault();

            return tokenObj?.Token;
        }
    }
}