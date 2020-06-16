using Cameo.Models;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Data.Repository.Interfaces
{
    public interface IVideoRequestRepository : IBaseCRUDRepository<VideoRequest>
    {
        IQueryable<VideoRequest> GetRequestsByTalent(Talent talent);
    }
}