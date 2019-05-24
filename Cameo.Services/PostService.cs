using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        public PostService(IPostRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}
