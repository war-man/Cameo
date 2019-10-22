using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services
{
    public class VideoRequestSearchService : BaseCRUDService<VideoRequest>, IVideoRequestSearchService
    {
        private readonly ICustomerService CustomerService;
        private readonly ITalentService TalentService;

        public VideoRequestSearchService(
            IVideoRequestRepository repository,
            IUnitOfWork unitOfWork,
            ICustomerService customerService,
            ITalentService talentService)
            : base(repository, unitOfWork)
        {
            CustomerService = customerService;
            TalentService = talentService;
        }

        public IQueryable<VideoRequest> Search(
            string userID,
            string userType,

            int start, 
            int length, 
            
            out int recordsTotal, 
            out int recordsFiltered,
            out string error)
        {
            recordsTotal = 0;
            recordsFiltered = 0;
            error = "";
            
            try
            {
                IQueryable<VideoRequest> videoRequests = GetActiveAsIQueryable();

                recordsTotal = videoRequests.Count();
                recordsFiltered = videoRequests.Count();

                IQueryable<VideoRequest> data = videoRequests.Skip(start).Take(length);

                return data;
            }
            catch (Exception ex)
            {
                error += ex.Message;
                if (ex.InnerException != null)
                    error += ". Inner exception: " + ex.InnerException.Message;

                return Enumerable.Empty<VideoRequest>().AsQueryable();
            }
        }
    }
}