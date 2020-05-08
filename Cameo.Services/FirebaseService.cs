using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using System.Threading.Tasks;

namespace Cameo.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IHostingEnvironment _env;

        public FirebaseService(IHostingEnvironment env)
        {
            _env = env;
        }

        //public void AddAndSaveFile(Attachment model, FileStream stream, string creatorID)
        //{
        //    //TO-DO: save file
        //    var bytes = FileManagement.ToByteArray(stream);
        //    string path = model.Path + "/" + model.GUID + "." + model.Extension;
        //    FileManagement.SaveFile(bytes, path);

        //    base.Add(model, creatorID);
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="uid">Firebase user id</param>
        /// <returns>+998901234567</returns>
        public async Task<string> GetPhoneNumberByUID(string uid)
        {
            //AppOptions options = new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile(_env.WebRootPath + "\\firebase\\cameo-uz-firebase-adminsdk-nqyi1-5db0b9990d.json")
            //};

            //FirebaseApp.Create(options);

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            return userRecord.PhoneNumber;
        }
    }
}