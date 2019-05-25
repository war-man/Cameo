using Cameo.Data.Infrastructure;
using Cameo.Data.Repository;
using Cameo.Data.Repository.Interfaces;
using Cameo.Services;
using Cameo.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo
{
    public static class ServiceProviderExtensions
    {
        public static void AddCommonDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDatabaseFactory, DatabaseFactory>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPostRepository, PostRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
        }
    }
}
