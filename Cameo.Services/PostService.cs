﻿using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;

namespace Cameo.Services
{
    public class PostService : BaseCRUDService<Post>, IPostService
    {
        public PostService(IPostRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}