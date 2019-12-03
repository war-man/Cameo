using Cameo.Models;
using Cameo.Models.Enums;
using System.Collections.Generic;

namespace Cameo.Services.Interfaces
{
    public interface ITalentBalanceService// : IBaseCRUDService<Talent>
    {
        int GetBalance(Talent talent);
        int GetBalanceIncludingReservations(Talent talent);
        int CalculateMaxAvailablePriceForCameo(Talent talent);
        int CalculateMaxNumberOfPossibleRequests(int balance, int price);
        bool BalanceAllowsToAcceptRequest(int balance, int price);
    }
}