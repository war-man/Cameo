using Cameo.Models;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface IVideoRequestSearchService : IBaseCRUDService<VideoRequest>
    {
        IQueryable<VideoRequest> Search(
            string userID,
            string userType,

            int? start,
            int? length,

            out int recordsTotal,
            out int recordsFiltered,
            out string error,

            int? statusID = 0);

        //IEnumerable<VideoRequest> GetTalentVideoRequestsReservingBalance(Talent talent);
    }
}