using Cameo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Cameo.Services.Interfaces
{
    public interface IFirebaseRegistrationTokenService : IBaseCRUDService<FirebaseRegistrationToken>
    {
        void SaveToken(string token, string userID, string frontType);
        string GetByUserID(string userID);
        string GetForWebByUserID(string userID);
    }
}