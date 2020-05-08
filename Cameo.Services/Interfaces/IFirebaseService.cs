using Cameo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Cameo.Services.Interfaces
{
    public interface IFirebaseService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="uid">Firebase user id</param>
        /// <returns>+998901234567</returns>
        Task<string> GetPhoneNumberByUID(string uid);
    }
}