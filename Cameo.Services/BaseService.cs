using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cameo.Data.Repository.Interfaces;
using Infrastructure = Cameo.Data.Infrastructure;

namespace Cameo.Services
{
    public class BaseService<T>
        where T : class
    {
        protected IBaseRepository<T> _repository { get; set; }

        protected Infrastructure.IUnitOfWork _unitOfWork { get; set; }

        public BaseService(IBaseRepository<T> repository, Infrastructure.IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public virtual T GetByID(int id)
        {
            return _repository.GetById(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this._repository.GetAll();
        }

        public virtual IQueryable<T> GetAllAsIQueryable()
        {
            return this._repository.GetAny();
        }

        public virtual void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
