using Cameo.Common;
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
    public class VideoRequestStatisticsService : IVideoRequestStatisticsService
    {
        private readonly IVideoRequestService VideoRequestService;

        public VideoRequestStatisticsService(IVideoRequestRepository repository,
            IUnitOfWork unitOfWork,
            IVideoRequestService videoRequestService)
        {
            VideoRequestService = videoRequestService;
        }

        private IQueryable<VideoRequest> GetAllActiveAsIQueryable()
        {
            return VideoRequestService.GetActiveAsIQueryable();
        }

        public int GetAllCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable().Count(m => m.TalentID == talent.ID);
        }

        public int GetNotCompletedCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Count(m => m.TalentID == talent.ID
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.videoExpired
                        || m.RequestStatusID == (int)VideoRequestStatusEnum.canceledByCustomer
                        || m.RequestStatusID == (int)VideoRequestStatusEnum.canceledByTalent));
        }

        //public int GetWaitingForAnswerCountByTalent(Talent talent)
        //{
        //    return GetAllActiveAsIQueryable()
        //        .Count(m => m.TalentID == talent.ID
        //            && (m.RequestStatusID == (int)VideoRequestStatusEnum.waitingForResponse));
        //}

        public int GetWaitingForVideoCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Count(m => m.TalentID == talent.ID
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo));
        }

        public int GetCompletedCountByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Count(m => m.TalentID == talent.ID
                    && (m.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted));
        }

        //public int GetWaitingForPaymentConfirmationCountByTalent(Talent talent)
        //{
        //    return GetAllActiveAsIQueryable()
        //        .Count(m => m.TalentID == talent.ID
        //            && (m.RequestStatusID == (int)VideoRequestStatusEnum.paymentScreenshotUploaded));
        //}

        //public int GetPaymentConfirmedCountByTalent(Talent talent)
        //{
        //    return GetAllActiveAsIQueryable()
        //        .Count(m => m.TalentID == talent.ID
        //            && (m.RequestStatusID == (int)VideoRequestStatusEnum.paymentConfirmed));
        //}

        public IQueryable<VideoRequest> GetAllCompletedByTalent(Talent talent)
        {
            return GetAllActiveAsIQueryable()
                .Where(m => m.TalentID == talent.ID
                    && m.RequestStatusID == (int)VideoRequestStatusEnum.videoCompleted);
        }

        //later siteStavka and amount, that talent earns will be saved for each request
        public int GetEarnedByTalent(Talent talent)
        {
            //int totalPaid = (int)GetAllCompletedByTalent(talent)
            //    .Sum(m => (m.Price * (100 - m.WebsiteCommission)) / 100);

            int totalPaid = (int)GetAllCompletedByTalent(talent).Sum(m => m.Price);

            return totalPaid;
            //int siteStavka = 25; //25%
            //return totalPaid * (100 - siteStavka) / 100;
        }
    }
}