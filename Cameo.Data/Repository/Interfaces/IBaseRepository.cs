using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Cameo.Data.Repository.Interfaces
{
    public interface IBaseCRUDRepository<T> where T : BaseModel
    {
        void Add(T entity);
        void AddCollection(ICollection<T> entities);

        void Update(T entity);
        void UpdateCollection(ICollection<T> entities);

        void Delete(T entity);
        void DeleteCollection(ICollection<T> entities);

        T GetByID(int id);
        //T GetByID(string id);

        IEnumerable<T> GetAll();
        IQueryable<T> GetAsIQueryable();

        IQueryable<T> GetWithRelatedDataAsIQueryable();
        T GetActiveSingleDetailsWithRelatedDataByID(int id);

        #region potentially unused methods
        //IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        //void Delete(Expression<Func<T, bool>> where);
        #endregion
    }

    //public interface IBaseRepository<T> where T : class
    //{
    //    void Add(T entity);
    //    void AddCollection(ICollection<T> entities);

    //    void Update(T entity);
    //    void UpdateCollection(ICollection<T> entities);

    //    void Delete(T entity);
    //    void DeleteCollection(ICollection<T> entities);

    //    T GetByID(int id);
    //    //T GetByID(string id);
        
    //    IEnumerable<T> GetAll();
    //    IQueryable<T> GetAsIQueryable();

    //    #region potentially unused methods
    //    //IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
    //    //void Delete(Expression<Func<T, bool>> where);
    //    #endregion
    //}
}
