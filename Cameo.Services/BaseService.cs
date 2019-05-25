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

        public virtual void Add(T entity)
        {
            OnPreUpdate(entity);
            _repository.Add(entity);
            Save();
        }

        void OnPreUpdate(T entity)
        {
            if (entity.ID == 0)
            {
                entity.CreatedBy = "asdf";
                entity.DateCreated = DateTime.Now;
            }

            entity.IsDeleted = false;
            entity.ModifiedBy = "asdf";
            entity.DateModified = DateTime.Now;
        }

        public virtual void AddCollection(ICollection<T> entities)
        {
            //first try foreach
            //else try for
            foreach (var item in entities)
            {
                OnPreUpdate(item);
            }

            _repository.AddCollection(entities);
            Save();
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
            Save();
        }

        public void UpdateCollection(ICollection<T> entities)
        {
            _repository.UpdateCollection(entities);
            Save();
        }

        public void Delete(T entity)
        {
            //entity.IsDeleted = true
            //Update(entity);
        }

        public void DeleteCollection(ICollection<T> entities)
        {

        }

        public void DeletePermanently(T entity)
        {
            _repository.Delete(entity);
            Save();
        }

        public void DeletePermanentlyCollection(ICollection<T> entities)
        {
            _repository.DeleteCollection(entities);
            Save();
        }

        public virtual T GetByID(int id)
        {
            return _repository.GetByID(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual IQueryable<T> GetAsIQueryable()
        {
            return _repository.GetAsIQueryable();
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
