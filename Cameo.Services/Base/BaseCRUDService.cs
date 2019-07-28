using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Infrastructure = Cameo.Data.Infrastructure;

namespace Cameo.Services
{
    public class BaseCRUDService<T> where T : BaseModel
    {
        protected IBaseCRUDRepository<T> _repository { get; set; }

        protected Infrastructure.IUnitOfWork _unitOfWork { get; set; }

        public BaseCRUDService(IBaseCRUDRepository<T> repository, Infrastructure.IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public virtual void Add(T entity, string creatorID)
        {
            OnPreUpdate(entity, creatorID);
            _repository.Add(entity);
            Save();
        }

        public virtual void AddCollection(ICollection<T> entities, string creatorID)
        {
            foreach (var item in entities)
            {
                OnPreUpdate(item, creatorID);
            }

            _repository.AddCollection(entities);
            Save();
        }

        public void Update(T entity, string modifierID)
        {
            OnPreUpdate(entity, modifierID);
            _repository.Update(entity);
            Save();
        }

        public void UpdateCollection(ICollection<T> entities, string modifierID)
        {
            foreach (var item in entities)
            {
                OnPreUpdate(item, modifierID);
            }

            _repository.UpdateCollection(entities);
            Save();
        }

        public void Delete(T entity, string modifierID)
        {
            entity.IsDeleted = true;
            Update(entity, modifierID);
        }

        public void DeleteCollection(ICollection<T> entities, string modifierID)
        {
            foreach (var item in entities)
            {
                item.IsDeleted = true;
            }

            UpdateCollection(entities, modifierID);
        }

        //public void DeletePermanently(T entity, string modifierID)
        //{
        //    _repository.Delete(entity);
        //    Save();
        //}

        //public void DeletePermanentlyCollection(ICollection<T> entities, string modifierID)
        //{
        //    _repository.DeleteCollection(entities);
        //    Save();
        //}

        public virtual T GetByID(int id)
        {
            return _repository.GetByID(id);
        }

        public virtual T GetActiveByID(int id)
        {
            T entity = GetByID(id);
            if (entity != null)
                return entity.IsDeleted ? null : entity;

            return entity;
        }

        public virtual IEnumerable<T> GetByIDs(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return new List<T>();

            return GetAsIQueryable()
                .Where(m => ids.Contains(m.ID));
        }

        public virtual IEnumerable<T> GetActiveByIDs(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return new List<T>();

            return GetAsIQueryable()
                .Where(m => ids.Contains(m.ID) && !m.IsDeleted);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual IEnumerable<T> GetAllActive()
        {
            return GetAsIQueryable().Where(m => !m.IsDeleted);
        }

        public virtual IQueryable<T> GetAsIQueryable()
        {
            return _repository.GetAsIQueryable();
        }

        public virtual IQueryable<T> GetWithRelatedDataAsIQueryable()
        {
            return _repository.GetWithRelatedDataAsIQueryable();
        }

        public virtual IQueryable<T> GetActiveAsIQueryable()
        {
            return GetAsIQueryable().Where(m => !m.IsDeleted);
        }

        void OnPreUpdate(T entity, string userID)
        {
            if (entity.ID == 0)
            {
                entity.CreatedBy = userID;
                entity.DateCreated = DateTime.Now;
                entity.IsDeleted = false;
            }

            entity.ModifiedBy = userID;
            entity.DateModified = DateTime.Now;
        }

        void Save()
        {
            _unitOfWork.Commit();
        }
    }

    //public class BaseService<T> where T : class
    //{
    //    protected IBaseRepository<T> _repository { get; set; }

    //    protected Infrastructure.IUnitOfWork _unitOfWork { get; set; }

    //    public BaseService(IBaseRepository<T> repository, Infrastructure.IUnitOfWork unitOfWork)
    //    {
    //        this._repository = repository;
    //        this._unitOfWork = unitOfWork;
    //    }

    //    public virtual void Add(T entity)
    //    {
    //        //if (typeof(T).IsSubclassOf(typeof(BaseModel)))
    //        //{
    //        //}

    //        _repository.Add(entity);
    //        Save();
    //    }        

    //    public virtual void AddCollection(ICollection<T> entities)
    //    {
    //        _repository.AddCollection(entities);
    //        Save();
    //    }

    //    public void Update(T entity)
    //    {
    //        _repository.Update(entity);
    //        Save();
    //    }

    //    public void UpdateCollection(ICollection<T> entities)
    //    {
    //        _repository.UpdateCollection(entities);
    //        Save();
    //    }

    //    public void Delete(T entity)
    //    {
    //        //entity.IsDeleted = true
    //        //Update(entity);
    //    }

    //    public void DeleteCollection(ICollection<T> entities)
    //    {

    //    }

    //    public void DeletePermanently(T entity)
    //    {
    //        _repository.Delete(entity);
    //        Save();
    //    }

    //    public void DeletePermanentlyCollection(ICollection<T> entities)
    //    {
    //        _repository.DeleteCollection(entities);
    //        Save();
    //    }

    //    public virtual T GetByID(int id)
    //    {
    //        return _repository.GetByID(id);
    //    }

    //    public virtual IEnumerable<T> GetAll()
    //    {
    //        return _repository.GetAll();
    //    }

    //    public virtual IQueryable<T> GetAsIQueryable()
    //    {
    //        return _repository.GetAsIQueryable();
    //    }

    //    void Save()
    //    {
    //        _unitOfWork.Commit();
    //    }
    //}
}
