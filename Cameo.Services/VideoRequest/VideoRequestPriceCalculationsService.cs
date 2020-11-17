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
    public class VideoRequestPriceCalculationsService : IVideoRequestPriceCalculationsService
    {
        private readonly ITalentBalanceService TalentBalanceService;
        private readonly ICustomerBalanceService CustomerBalanceService;


        public VideoRequestPriceCalculationsService(
                           ITalentBalanceService talentBalanceService,
                           ICustomerBalanceService customerBalanceService)
        {
            TalentBalanceService = talentBalanceService;
            CustomerBalanceService = customerBalanceService;
        }

        //public int CalculateRequestPrice(Talent talent)
        //{
        //    int price = talent.Price;

        //    double websiteCommission = 0;
        //    double.TryParse(AppData.Configuration.WebsiteCommission.ToString(), out websiteCommission);
        //    if (websiteCommission <= 0)
        //        websiteCommission = 25;

        //    return CalculateRequestPrice(talent.Price, websiteCommission);
        //}

        //public int CalculateRequestPrice(VideoRequest request)
        //{
        //    return CalculateRequestPrice(request.Price, request.WebsiteCommission);
        //}

        //private int CalculateRequestPrice(int price, double websiteCommission)
        //{
        //    double requestPriceDouble = ((101 * websiteCommission - 100) / 10000) * price;
        //    double requestPriceDouble2 = (0.25 - (0.75 * 0.01)) * price;

        //    int requestPriceInt = ((int)(requestPriceDouble / 1000)) * 1000;

        //    return requestPriceInt;
        //}

        //public int CalculateRemainingPrice(int price, double websiteCommission)
        //{
        //    int remainingPrice = (int)(((100.0 - websiteCommission) / 100) * price);

        //    return remainingPrice;
        //}
    }
}