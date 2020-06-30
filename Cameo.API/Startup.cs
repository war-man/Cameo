using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cameo.API.Filters;
using Cameo.Common;
using Cameo.Data;
using Cameo.DependencyInjections;
using Cameo.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Cameo.API
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
#if DEBUG
            string connectionStringName = "DefaultConnection";
#else
            string connectionStringName = "ServerConnection";
            //string connectionStringName = "USAServerConnection";
#endif

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString(connectionStringName),
                   b => b.MigrationsAssembly("Cameo").UseRowNumberForPaging()));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<AppConfiguration>(Configuration.GetSection("MySettings"));

            //Hangfire
#if DEBUG
            string connectionString = "Data Source=.;Initial Catalog=Helloo;User Id=sa;Password=490969;";
#else
            string connectionString = "Data Source=192.168.44.96;Initial Catalog=Helloo;User Id=sa;Password=cloudstack;";
            //string connectionString = "Data Source=209.159.151.3;Initial Catalog=Helloo;User Id=sa;Password=MCGR4ZD4Thnr93V4;";
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

#if DEBUG
            string validIssuer = "https://localhost:44322";
#else
            string validIssuer = "https://cameoapi.wiut.uz";
            //string validIssuer = "https://api.helloo.uz";
#endif
            string tokenSecurityKey = "9mxhkbcmmreh2hsnbqh6lisy21t06eg563txkb9w8t4012tiy1fa9xei4d80hucunvhdwgza0917hkf6b0mr36zyadoxxqhqrottbyuhylelvzhd69uz6znmii9lex1a";
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidIssuer = validIssuer,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurityKey))
                    };
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            ConfigureFirebase();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerOnly", policy => policy.RequireClaim("UserType", "customer"));
                options.AddPolicy("TalentOnly", policy => policy.RequireClaim("UserType", "talent"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<AppConfiguration> appSettings)
        {
            AppData.Configuration = appSettings.Value;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            // global cors policy
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());

            app.UseAuthentication();

            //app.UseHttpsRedirection();
            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                IsReadOnlyFunc = (DashboardContext context) => true
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
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
