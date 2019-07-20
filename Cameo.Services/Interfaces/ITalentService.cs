using Cameo.Models;

namespace Cameo.Services.Interfaces
{
    public interface ITalentService : IBaseCRUDService<Talent>
    {
        Talent GetByUserID(string userID);
    }
}