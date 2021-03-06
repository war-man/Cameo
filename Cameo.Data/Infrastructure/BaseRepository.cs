﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cameo.Models;
using Microsoft.EntityFrameworkCore;

namespace Cameo.Data.Infrastructure
{
    public abstract class BaseCRUDRepository<T> where T : BaseModel
    {
        private ApplicationDbContext _dataContext;
        private readonly DbSet<T> _dbset;

        protected BaseCRUDRepository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected ApplicationDbContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }

        public DbSet<T> DbSet
        {
            get { return _dbset; }
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void AddCollection(ICollection<T> entities)
        {
            foreach (var p in entities)
            {
                Add(p);
            }
        }

        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateCollection(ICollection<T> entities)
        {
            foreach (var p in entities)
            {
                Update(p);
            }
        }

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void DeleteCollection(ICollection<T> entities)
        {
            var count = entities.Count();
            for (int i = count; i > 0; i--)
            {
                var item = entities.ElementAt(i - 1);
                _dbset.Attach(item);
                _dataContext.Entry(item).State = EntityState.Deleted;
            }
        }

        public virtual T GetByID(int id)
        {
            return _dbset.Find(id);
        }

        //public virtual T GetByID(string id)
        //{
        //    return _dbset.Find(id);
        //}

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }

        public virtual IQueryable<T> GetAsIQueryable()
        {
            return _dataContext.Set<T>();
        }

        public virtual IQueryable<T> GetWithRelatedDataAsIQueryable()
        {
            return _dataContext.Set<T>();
        }

        public virtual T GetActiveSingleDetailsWithRelatedDataByID(int id)
        {
            return _dataContext.Set<T>().FirstOrDefault(m => m.ID == id);
        }

        public virtual T GetActiveSingleDetailsWithRelatedDataByUserID(string userID)
        {
            return _dataContext.Set<T>().FirstOrDefault(m => m.CreatedBy == userID);
        }

        #region potentially unused methods
        //public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        //{
        //    return _dbset.Where(where).ToList();
        //}

        /// <summary>
        /// Return a paged list of entities
        /// </summary>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="page">Which page to retrieve</param>
        /// <param name="where">Where clause to apply</param>
        /// <param name="order">Order by to apply</param>
        /// <returns></returns>
        //public virtual IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order)
        //{
        //    var results = _dbset.OrderBy(order).Where(where).GetPage(page).ToList();
        //    var total = _dbset.Count(where);
        //    return new StaticPagedList<T>(results, page.PageNumber, page.PageSize, total);
        //}

        //public virtual void Delete(Expression<Func<T, bool>> where)
        //{
        //    IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
        //    foreach (T obj in objects)
        //        _dbset.Remove(obj);
        //}

        //public bool ExistsInSet(T entity)
        //{
        //    return _dbset.Local.Any(e => e == entity);
        //}
        #endregion
    }

    //public abstract class BaseRepository<T> where T : class
    //{
    //    private ApplicationDbContext _dataContext;
    //    private readonly DbSet<T> _dbset;

    //    protected BaseRepository(IDatabaseFactory databaseFactory)
    //    {
    //        DatabaseFactory = databaseFactory;
    //        _dbset = DataContext.Set<T>();
    //    }

    //    protected IDatabaseFactory DatabaseFactory
    //    {
    //        get;
    //        private set;
    //    }

    //    protected ApplicationDbContext DataContext
    //    {
    //        get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
    //    }


    //    public virtual void Add(T entity)
    //    {
    //        _dbset.Add(entity);
    //    }

    //    public virtual void AddCollection(ICollection<T> entities)
    //    {
    //        foreach (var p in entities)
    //        {
    //            Add(p);
    //        }
    //    }

    //    public virtual void Update(T entity)
    //    {
    //        _dbset.Attach(entity);
    //        _dataContext.Entry(entity).State = EntityState.Modified;
    //    }

    //    public virtual void UpdateCollection(ICollection<T> entities)
    //    {
    //        foreach (var p in entities)
    //        {
    //            Update(p);
    //        }
    //    }

    //    public virtual void Delete(T entity)
    //    {
    //        _dbset.Remove(entity);
    //    }

    //    public virtual void DeleteCollection(ICollection<T> entities)
    //    {
    //        var count = entities.Count();
    //        for (int i = count; i > 0; i--)
    //        {
    //            var item = entities.ElementAt(i - 1);
    //            _dbset.Attach(item);
    //            _dataContext.Entry(item).State = EntityState.Deleted;
    //        }
    //    }

    //    public virtual T GetByID(int id)
    //    {
    //        return _dbset.Find(id);
    //    }

    //    //public virtual T GetByID(string id)
    //    //{
    //    //    return _dbset.Find(id);
    //    //}

    //    public virtual IEnumerable<T> GetAll()
    //    {
    //        return _dbset.ToList();
    //    }

    //    public virtual IQueryable<T> GetAsIQueryable()
    //    {
    //        return _dataContext.Set<T>();
    //    }

    //    #region potentially unused methods
    //    //public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
    //    //{
    //    //    return _dbset.Where(where).ToList();
    //    //}

    //    /// <summary>
    //    /// Return a paged list of entities
    //    /// </summary>
    //    /// <typeparam name="TOrder"></typeparam>
    //    /// <param name="page">Which page to retrieve</param>
    //    /// <param name="where">Where clause to apply</param>
    //    /// <param name="order">Order by to apply</param>
    //    /// <returns></returns>
    //    //public virtual IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order)
    //    //{
    //    //    var results = _dbset.OrderBy(order).Where(where).GetPage(page).ToList();
    //    //    var total = _dbset.Count(where);
    //    //    return new StaticPagedList<T>(results, page.PageNumber, page.PageSize, total);
    //    //}

    //    //public virtual void Delete(Expression<Func<T, bool>> where)
    //    //{
    //    //    IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
    //    //    foreach (T obj in objects)
    //    //        _dbset.Remove(obj);
    //    //}

    //    //public bool ExistsInSet(T entity)
    //    //{
    //    //    return _dbset.Local.Any(e => e == entity);
    //    //}
    //    #endregion
    //}
}
