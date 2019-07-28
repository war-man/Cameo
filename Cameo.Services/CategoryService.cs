using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;

namespace Cameo.Services
{
    public class CategoryService : BaseDropdownableService<Category>, ICategoryService
    {
        public CategoryService(ICategoryRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}