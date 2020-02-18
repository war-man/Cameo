using Cameo.Models;
using Cameo.Models.Enums;
using System.Collections.Generic;

namespace Cameo.Services.Interfaces
{
    public interface ITalentService : IBaseCRUDService<Talent>
    {
        Talent GetByUserID(string userID);
        Talent GetAvailableByID(int id);
        Talent GetActiveByUsername(string username);
        Talent GetActiveSingleDetailsWithRelatedDataByID(int id);

        IEnumerable<Talent> Search(int categoryID, SortTypeEnum sort);
        IEnumerable<Talent> SearchBySearchText(string searchText);
        IEnumerable<Talent> GetRelated(Talent model);
        void AssignAccountNumber(Talent model);
        void SetAvailability(Talent model, bool availability, string userID);
    }
}