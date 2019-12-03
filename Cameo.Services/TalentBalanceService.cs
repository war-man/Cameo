using Cameo.Common;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cameo.Services
{
    public class TalentBalanceService : /*BaseCRUDService<Talent>,*/ ITalentBalanceService
    {
        private readonly IVideoRequestSearchService VideoRequestSearchService;

        public TalentBalanceService(IVideoRequestSearchService videoRequestSearchService)
        {
            VideoRequestSearchService = videoRequestSearchService;
        }

        public int GetBalance(Talent talent)
        {
            return talent?.Balance ?? 0;
        }

        public int GetBalanceIncludingReservations(Talent talent)
        {
            int clearBalance = GetBalance(talent);
            int reservedAmount = VideoRequestSearchService.GetTalentVideoRequestsReservingBalance(talent)
                .Sum(m => m.Price);

            return clearBalance - reservedAmount;
        }

        public int CalculateMaxAvailablePriceForCameo(Talent talent)
        {
            int price = 1000;

            int balance = GetBalanceIncludingReservations(talent);
            if (balance == 0)
                price = 0;

            int k = AppData.Configuration.PaymentSystemCommission;
            double priceDouble = (balance * (100 + k) / (25 - 0.75 * k));
            if (priceDouble > 1000)
            {
                price = (int)priceDouble;
                price /= 1000;
                price *= 1000;
                price += 1000;
            }

            return price;
        }

        //сумма, которая снимается с биллингового счета продавца после оплаты клиентом
        //и попадает на счет сайта
        public int CalculateMoneyThatTalentPaysToSystemForCameo(int price)
        {
            int d = 0;

            int k = AppData.Configuration.PaymentSystemCommission;
            double mDouble = (price * 100) / (100 + k);
            int m = 0;
            if (mDouble > 0)
            {
                m = (int)mDouble;
                m /= 1000;
                m *= 1000;
            }

            d = m - (int)(0.75 * price);

            return d;
        }

        //количество запросов, которые можно обработать исходя из баланса и цены, указанной за Cameo
        public int CalculateMaxNumberOfPossibleRequests(int balance, int price)
        {
            int moneyThatTalentPaysToSystemForCameo = CalculateMoneyThatTalentPaysToSystemForCameo(price);
            if (moneyThatTalentPaysToSystemForCameo == 0)
                return 0;

            return balance / moneyThatTalentPaysToSystemForCameo;
        }

        public bool BalanceAllowsToAcceptRequest(int balance, int price)
        {
            return CalculateMaxNumberOfPossibleRequests(balance, price) > 0;
        }
    }
}
