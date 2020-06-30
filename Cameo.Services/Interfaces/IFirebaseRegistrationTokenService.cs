using Cameo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface IFirebaseRegistrationTokenService : IBaseCRUDService<FirebaseRegistrationToken>
    {
        void SaveToken(string token, string userID, string frontType);
        void RefreshToken(string userID, string oldToken, string newToken, string frontType);
        string GetByUserID(string userID);
        string GetForWebByUserID(string userID);
        IQueryable<FirebaseRegistrationToken> GetManyByUserID(string userID, string frontType);
        void SendNotification(string userID, string title, string body, Dictionary<string, string> data = null);
    }
}