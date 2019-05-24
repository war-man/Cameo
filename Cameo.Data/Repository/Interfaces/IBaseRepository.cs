﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Cameo.Data.Repository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> GetAny();
        void DeleteCollection(ICollection<T> collection);
    }
}
