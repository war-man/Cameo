using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using FirebaseAdmin.Messaging;
using System.Collections.Generic;
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

        public void RefreshToken(string userID, string oldToken, string newToken, string frontType)
        {
            var oldTokenObj = GetAsIQueryable()
                .Where(m => m.UserID == userID
                    && m.Token.Equals(oldToken)
                    && m.FrontType.Equals(frontType))
                .FirstOrDefault();

            if (oldTokenObj != null)
                DeletePermanently(oldTokenObj, userID);

            SaveToken(newToken, userID, frontType);
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

        public void SendNotification(string userID, string title, string body, Dictionary<string, string> data = null)
        {
            var registrationTokens = GetManyByUserID(userID)
                .Select(m => m.Token)
                .ToList();

            if (registrationTokens.Count == 0)
                return;

            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = new Dictionary<string, string>()
                {
                    ["title"] = title,
                    ["body"] = body,
                },
            };

            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    if (!message.Data.ContainsKey(item.Key))
                        message.Data.Append(item);
                }
            }

            var response = FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
        }

        public IQueryable<FirebaseRegistrationToken> GetManyByUserID(string userID, string frontType = null)
        {
            var userTokens = GetAsIQueryable().Where(m => m.UserID.Equals(userID));
            if (!string.IsNullOrWhiteSpace(frontType))
                userTokens = userTokens.Where(m => m.FrontType.Equals(frontType));

            return userTokens;
        }
    }
}