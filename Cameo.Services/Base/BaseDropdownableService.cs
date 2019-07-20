using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cameo.Services
{
    public class BaseDropdownableService<T> : BaseCRUDService<T>
        where T : BaseModelDropdownable
    {
        public BaseDropdownableService(
            IBaseCRUDRepository<T> repository, 
            Data.Infrastructure.IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public List<SelectListItem> GetAsSelectList(int selected = 0)
        {
            return GetAllActive().Select(item => new SelectListItem()
            {
                Value = item.ID.ToString(),
                Text = item.Name.ToString(),
                Selected = (item.ID == selected) ? true : false
            }).ToList();
        }
    }
}
