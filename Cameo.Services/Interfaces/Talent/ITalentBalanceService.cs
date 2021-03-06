﻿using Cameo.Models;
using Cameo.Models.Enums;
using System.Collections.Generic;

namespace Cameo.Services.Interfaces
{
    public interface ITalentBalanceService// : IBaseCRUDService<Talent>
    {
        int GetBalance(Talent talent);
        void ReplenishBalance(Talent talent, int amount);
        bool BalanceIsLessThan(Talent talent, int amount);
        //int GetBalanceIncludingReservations(Talent talent);
        //int CalculateMaxAvailablePriceForCameo(Talent talent);
        //int CalculateMoneyThatTalentPaysToSystemForCameo(int price, double websiteCommission);
        //int CalculateMaxNumberOfPossibleRequests(int balance, int price, double commission);
        //bool BalanceAllowsToAcceptRequest(int balance, int price);
        //bool BalanceAllowsToConfirmVideo(int balance, int price);
        //bool BalanceAllowsToUploadVideo(int balance, int price);
        void TakeOffBalance(Talent talent, int amount);
    }
}