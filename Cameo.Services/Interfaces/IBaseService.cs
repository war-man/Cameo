using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IBaseCRUDService<T> where T : BaseModel
    {
        void Add(T entity);
        void AddCollection(ICollection<T> entities);

        void Update(T entity);
        void UpdateCollection(ICollection<T> entities);

        void Delete(T entity);
        void DeleteCollection(ICollection<T> entities);

        void DeletePermanently(T entity);
        void DeletePermanentlyCollection(ICollection<T> entities);

        T GetByID(int id);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAsIQueryable();
    }

    //public interface IBaseService<T> where T : class
    //{
    //    void Add(T entity);
    //    void AddCollection(ICollection<T> entities);

    //    void Update(T entity);
    //    void UpdateCollection(ICollection<T> entities);

    //    void Delete(T entity);
    //    void DeleteCollection(ICollection<T> entities);

    //    void DeletePermanently(T entity);
    //    void DeletePermanentlyCollection(ICollection<T> entities);

    //    T GetByID(int id);
    //    IEnumerable<T> GetAll();
    //    IQueryable<T> GetAsIQueryable();

    //}
}
