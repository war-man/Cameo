﻿using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class PostRepository : BaseCRUDRepository<Post>, IPostRepository
    {
        public PostRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}