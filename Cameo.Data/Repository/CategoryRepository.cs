using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class CategoryRepository : BaseCRUDRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}