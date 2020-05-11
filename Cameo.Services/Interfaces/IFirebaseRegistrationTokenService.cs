using Cameo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Cameo.Services.Interfaces
{
    public interface IFirebaseRegistrationTokenService : IBaseCRUDService<FirebaseRegistrationToken>
    {
    }
}