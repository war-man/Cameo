using System;
using System.Collections.Generic;
using System.Text;
using Cameo.Models;

namespace Cameo.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        ApplicationDbContext Get();
    }
}
