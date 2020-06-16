using Cameo.Models;
using System.Collections.Generic;

namespace Cameo.Services.Interfaces
{
    public interface IVideoRequestStatusService : IBaseDropdownableService<VideoRequestStatus>
    {
        List<BaseModelDropdownable> GetAsSelectListForFilter();
    }
}