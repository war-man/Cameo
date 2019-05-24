using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        T GetByID(int id);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllAsIQueryable();
    }
}
