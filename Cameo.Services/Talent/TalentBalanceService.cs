﻿using Cameo.Common;
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
        //private readonly IVideoRequestSearchService VideoRequestSearchService;
        private readonly ITalentService TalentService;

        public TalentBalanceService(ITalentService talentService)
        {
            TalentService = talentService;
        }

        public int GetBalance(Talent talent)
        {
            return talent?.Balance ?? 0;
        }

        public void ReplenishBalance(Talent talent, int amount)
        {
            talent.Balance += amount;
        }

        public bool BalanceIsLessThan(Talent talent, int amount)
        {
            return talent.Balance < amount;
        }

        //public int GetBalanceIncludingReservations(Talent talent)
        //{
        //    int clearBalance = GetBalance(talent);
        //    IEnumerable<VideoRequest> videoRequestsReservingBalance =
        //        VideoRequestSearchService.GetTalentVideoRequestsReservingBalance(talent);

        //    int reservedAmount = 0;
        //    foreach (var item in videoRequestsReservingBalance)
        //    {
        //        reservedAmount += CalculateMoneyThatTalentPaysToSystemForCameo(item.Price);
        //    }

        //    return clearBalance - reservedAmount;
        //}

        //public int CalculateMaxAvailablePriceForCameo(Talent talent)
        //{
        //    int price = 1000;

        //    //int balance = GetBalanceIncludingReservations(talent);
        //    int balance = GetBalance(talent);
        //    if (balance == 0)
        //        price = 0;

        //    int k = AppData.Configuration.PaymentSystemCommission;
        //    double priceDouble = (balance * (100 + k) / (25 - 0.75 * k));
        //    if (priceDouble > 1000)
        //    {
        //        price = (int)priceDouble;
        //        price /= 1000;
        //        price *= 1000;
        //        price += 1000;
        //    }

        //    return price;
        //}

        //сумма, которая снимается с биллингового счета продавца после оплаты клиентом
        //и попадает на счет сайта
        //public int CalculateMoneyThatTalentPaysToSystemForCameo(int price, double websiteCommission)
        //{
        //    int d = 0;

        //    double paymentSystemCommission = AppData.Configuration.PaymentSystemCommission;
        //    double mDouble = (price * 100) / (100 + paymentSystemCommission);
        //    int m = 0;
        //    if (mDouble > 0)
        //    {
        //        m = (int)mDouble;
        //        m /= 1000;
        //        m *= 1000;
        //    }

        //    d = m - (int)(((100.0 - websiteCommission) / 100) * price);

        //    return d;
        //}

        ////количество запросов, которые можно обработать исходя из баланса и цены, указанной за Cameo
        //public int CalculateMaxNumberOfPossibleRequests(int balance, int price, double commission)
        //{
        //    if (balance <= 0)
        //        return 0;

        //    int moneyThatTalentPaysToSystemForCameo = CalculateMoneyThatTalentPaysToSystemForCameo(price, commission);
        //    if (moneyThatTalentPaysToSystemForCameo == 0)
        //        return 0;

        //    return balance / moneyThatTalentPaysToSystemForCameo;
        //}

        //public bool BalanceAllowsToAcceptRequest(int balance, int price)
        //{
        //    return CalculateMaxNumberOfPossibleRequests(balance, price) > 0;
        //}

        //public bool BalanceAllowsToConfirmVideo(int balance, int price)
        //{
        //    return CalculateMaxNumberOfPossibleRequests(balance, price) > 0;
        //}

        //public bool BalanceAllowsToUploadVideo(int balance, int price)
        //{
        //    return CalculateMaxNumberOfPossibleRequests(balance, price) > 0;
        //}

        public void TakeOffBalance(Talent talent, int amount)
        {
            talent.Balance -= amount;
        }
    }
}
