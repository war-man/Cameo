using Cameo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Cameo.Services.Interfaces
{
    public interface IAttachmentService : IBaseCRUDService<Attachment>
    {
        //void AddAndSaveFile(Attachment model, FileStream stream, string creatorID);
    }
}