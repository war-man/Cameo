using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IBaseCRUDService<T> where T : BaseModel
    {
        void Add(T entity, string creatorID);
        void AddCollection(ICollection<T> entities, string creatorID);

        void Update(T entity, string modifierID);
        void UpdateCollection(ICollection<T> entities, string modifierID);

        void Delete(T entity, string modifierID);
        void DeleteCollection(ICollection<T> entities, string modifierID);

        //uncomment when it is need
        //void DeletePermanently(T entity, string modifierID);
        //void DeletePermanentlyCollection(ICollection<T> entities, string modifierID);

        T GetByID(int id);
        T GetActiveByID(int id);
        IEnumerable<T> GetByIDs(int[] ids);
        IEnumerable<T> GetActiveByIDs(int[] ids);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllActive();
        IQueryable<T> GetAsIQueryable();
        IQueryable<T> GetActiveAsIQueryable();
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
