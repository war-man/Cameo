using Cameo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IBaseDropdownableService<T> : IBaseCRUDService<T> 
        where T : BaseModelDropdownable
    {
        List<SelectListItem> GetAsSelectList(int[] selected = null);
    }
}
