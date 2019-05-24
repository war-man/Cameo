using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
