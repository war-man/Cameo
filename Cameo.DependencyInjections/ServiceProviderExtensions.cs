﻿using Cameo.Data.Infrastructure;
using Cameo.Data.Repository;
using Cameo.Data.Repository.Interfaces;
using Cameo.Services;
using Cameo.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.DependencyInjections
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
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ITalentRepository, TalentRepository>();
            services.AddTransient<ISocialAreaRepository, SocialAreaRepository>();
            services.AddTransient<IAttachmentRepository, AttachmentRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IVideoRequestRepository, VideoRequestRepository>();
            services.AddTransient<IVideoRequestTypeRepository, VideoRequestTypeRepository>();
            services.AddTransient<ILogTalentPriceRepository, LogTalentPriceRepository>();
            services.AddTransient<IVideoRequestStatusRepository, VideoRequestStatusRepository>();
            services.AddTransient<IFirebaseRegistrationTokenRepository, FirebaseRegistrationTokenRepository>();
            services.AddTransient<IClickTransactionRepository, ClickTransactionRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IWithdrawRequestRepository, WithdrawRequestRepository>();
            services.AddTransient<IWithdrawRequestStatusRepository, WithdrawRequestStatusRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IHangfireService, HangfireService>();
            services.AddTransient<IFirebaseService, FirebaseService>();

            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ITalentService, TalentService>();
            services.AddTransient<ISocialAreaService, SocialAreaService>();
            services.AddTransient<IAttachmentService, AttachmentService>();
            services.AddTransient<IFileManagement, FileManagement>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IVideoRequestService, VideoRequestService>();
            services.AddTransient<IVideoRequestTypeService, VideoRequestTypeService>();
            services.AddTransient<IVideoRequestSearchService, VideoRequestSearchService>();
            services.AddTransient<ITalentBalanceService, TalentBalanceService>();
            services.AddTransient<ILogTalentPriceService, LogTalentPriceService>();
            services.AddTransient<IVideoRequestStatusService, VideoRequestStatusService>();
            services.AddTransient<IFirebaseRegistrationTokenService, FirebaseRegistrationTokenService>();
            services.AddTransient<ICustomerBalanceService, CustomerBalanceService>();
            services.AddTransient<IVideoRequestStatisticsService, VideoRequestStatisticsService>();
            services.AddTransient<IVideoRequestPriceCalculationsService, VideoRequestPriceCalculationsService>();
            services.AddTransient<IClickTransactionService, ClickTransactionService>();
            services.AddTransient<ITalentVisibilityService, TalentVisibilityService>();
            services.AddTransient<ITalentSearchService, TalentSearchService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IPaymoService, PaymoService>();
            services.AddTransient<IWithdrawRequestService, WithdrawRequestService>();
            services.AddTransient<IWithdrawRequestStatusService, WithdrawRequestStatusService>();
        }
    }
}
