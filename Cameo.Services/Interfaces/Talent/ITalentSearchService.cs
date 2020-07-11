using Cameo.Models;
using Cameo.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface ITalentSearchService : IBaseCRUDService<Talent>
    {
        IQueryable<Talent> GetFeatured(int? categoryID, int? count = null);
        IQueryable<Talent> GetNew(int? categoryID, int? count = null);
        IQueryable<Talent> GetNewInFeatured(int? count = null);

        IQueryable<Talent> Search(int categoryID, SortTypeEnum sort, int? count = null);
        IQueryable<Talent> SearchBySearchText(string searchText);
        IQueryable<Talent> GetRelated(Talent model, int? count = null);
    }
}