using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cameo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cameo.Models;
using Cameo.Data.Infrastructure;
using Hangfire;
using Cameo.Filters;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Cameo.Common;
using Microsoft.Extensions.Options;
using Cameo.Utils;
using Microsoft.Extensions.Logging;
using Cameo.DependencyInjections;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Cameo
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
#if DEBUG
            string connectionStringName = "DefaultConnection";
            //string connectionStringName = "USAServerConnection";
#else
            string connectionStringName = "USAServerConnection";
#endif

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString(connectionStringName),
                   b => b.MigrationsAssembly("Cameo").UseRowNumberForPaging()));

            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<AppConfiguration>(Configuration.GetSection("MySettings"));

            //Hangfire
#if DEBUG
            string connectionString = "Data Source=.;Initial Catalog=Helloo;User Id=sa;Password=490969;";
            //string connectionString = "Data Source=209.159.151.3;Initial Catalog=Helloo;User Id=sa;Password=MCGR4ZD4Thnr93V4;";
#else
            string connectionString = "Data Source=209.159.151.3;Initial Catalog=Helloo;User Id=sa;Password=MCGR4ZD4Thnr93V4;";
#endif
            services.AddHangfire(config => 
                config.UseSqlServerStorage(
                    connectionString, 
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        UsePageLocksOnDequeue = true,
                        DisableGlobalLocks = true
                    }
                )
            );

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            //add common dependencies
            services.AddCommonDependencies(); //UnitOfWork and DatabaseFactory

            //add repositories
            services.AddRepositories();

            //add services
            services.AddServices();

            var webRoot = _env.ContentRootPath;

            services.AddSingleton<IFileProvider>(
              new PhysicalFileProvider(
                Path.Combine(webRoot, "Uploads")));

            ConfigureFirebase();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerOnly", policy => policy.RequireClaim("UserType", "customer"));
                options.AddPolicy("TalentOnly", policy => policy.RequireClaim("UserType", "talent"));
            });

            services.AddMvc().AddViewLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            IOptions<AppConfiguration> appSettings,
            ILoggerFactory loggerFactory
            /*, IBackgroundJobClient backgroundJobs*/)
        {
            AppData.Configuration = appSettings.Value;

            if (env.IsDevelopment())
            {
                app.UseStatusCodePagesWithReExecute("/Error/Status/{0}");
                app.UseExceptionHandler("/Error/Index");

                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseStatusCodePagesWithReExecute("/Error/Status/{0}");
                //app.UseExceptionHandler("/Error/Index");

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "Content")),
            //    RequestPath = "/Content"
            //});
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
            //    RequestPath = "/Uploads"
            //});

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "blog", 
                    template: "Talents/Details/{username}",
                    defaults: new { controller = "Talents", action = "Details" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Talents}/{action=Index}/{id?}");
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                IsReadOnlyFunc = (DashboardContext context) => true
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging")); //"Logging" from appsettings.json
            loggerFactory.AddDebug();

            //both variants work
            //BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
        }

        private void ConfigureFirebase()
        {
            AppOptions options = new AppOptions()
            {
                Credential = GoogleCredential.FromFile(_env.WebRootPath + "\\firebase\\cameo-uz-firebase-adminsdk-nqyi1-5db0b9990d.json")
            };

            FirebaseApp.Create(options);
        }
    }
}
