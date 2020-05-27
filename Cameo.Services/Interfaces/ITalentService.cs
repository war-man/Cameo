using Cameo.Models;
using Cameo.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface ITalentService : IBaseCRUDService<Talent>
    {
        Talent GetByUserID(string userID);
        Talent GetAvailableByID(int id);
        Talent GetActiveByUsername(string username);
        Talent GetActiveSingleDetailsWithRelatedDataByID(int id);

        IQueryable<Talent> GetFeatured(int? categoryID, int? count = null);
        IQueryable<Talent> GetNew(int? categoryID, int? count = null);
        IQueryable<Talent> GetNewInFeatured(int? count = null);

        IQueryable<Talent> Search(int categoryID, SortTypeEnum sort, int? count = null);
        IQueryable<Talent> SearchBySearchText(string searchText);
        IQueryable<Talent> GetRelated(Talent model, int? count = null);
        void AssignAccountNumber(Talent model);
        void SetAvailability(Talent model, bool availability, string userID);

        void SaveDetachedIntroVideo(Talent model, string userID);
    }
}