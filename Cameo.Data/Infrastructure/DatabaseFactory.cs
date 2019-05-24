using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private ApplicationDbContext _dataContext;

        public DatabaseFactory(ApplicationDbContext context)
        {
            _dataContext = context;
        }

        public ApplicationDbContext Get()
        {
            return _dataContext;
        }
        protected override void DisposeCore()
        {
            _dataContext?.Dispose();
        }
    }
}
