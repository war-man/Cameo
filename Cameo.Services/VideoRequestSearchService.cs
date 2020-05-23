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
        private readonly IVideoRequestRepository VideoRequestRepository;

        public VideoRequestSearchService(
            IVideoRequestRepository repository,
            IUnitOfWork unitOfWork,
            ICustomerService customerService,
            ITalentService talentService,
            IVideoRequestRepository videoRequestRepository
            )
            : base(repository, unitOfWork)
        {
            CustomerService = customerService;
            TalentService = talentService;
            VideoRequestRepository = videoRequestRepository;
        }

        public IQueryable<VideoRequest> Search(
            string userID,
            string userType,

            int? start, 
            int? length, 
            
            out int recordsTotal, 
            out int recordsFiltered,
            out string error,

            int? statusID = 0)
        {
            recordsTotal = 0;
            recordsFiltered = 0;
            error = "";
            
            try
            {
                int personID = 0;
                IQueryable<VideoRequest> videoRequests = GetWithRelatedDataAsIQueryable();
                if (userType == UserTypesEnum.talent.ToString())
                {
                    personID = TalentService.GetByUserID(userID)?.ID ?? 0;
                    videoRequests = videoRequests.Where(m => m.TalentID == personID);
                }
                else
                {
                    personID = CustomerService.GetByUserID(userID)?.ID ?? 0;
                    videoRequests = videoRequests.Where(m => m.CustomerID == personID);
                }

                if (statusID.HasValue && statusID > 0)
                {
                    if (statusID.Value == (int)VideoRequestStatusEnum.videoCompleted)
                    {
                        videoRequests = videoRequests
                            .Where(m => m.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted
                                || m.RequestStatusID == (int)VideoRequestStatusEnum.paid);
                    }
                    else
                        videoRequests = videoRequests.Where(m => m.RequestStatusID == statusID);
                }

                recordsTotal = videoRequests.Count();
                recordsFiltered = videoRequests.Count();

                if (start.HasValue && start.Value > 0)
                    videoRequests = videoRequests.Skip(start.Value);

                if (length.HasValue && length.Value > 0)
                    videoRequests = videoRequests.Take(length.Value);

                //IQueryable<VideoRequest> data = videoRequests.Skip(start).Take(length.Value);

                return videoRequests;
            }
            catch (Exception ex)
            {
                error += ex.Message;
                if (ex.InnerException != null)
                    error += ". Inner exception: " + ex.InnerException.Message;

                return Enumerable.Empty<VideoRequest>().AsQueryable();
            }
        }

        //public IEnumerable<VideoRequest> GetTalentVideoRequestsReservingBalance(Talent talent)
        //{
        //    return VideoRequestRepository.GetRequestsByTalent(talent)
        //        .Where(m => m.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndwaitingForVideo
        //            || m.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted);
        //}
    }
}