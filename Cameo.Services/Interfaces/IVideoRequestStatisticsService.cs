using Cameo.Models;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface IVideoRequestStatisticsService
    {
        int GetAllCountByTalent(Talent talent);

        int GetNotCompletedCountByTalent(Talent talent);

        int GetWaitingForAnswerCountByTalent(Talent talent);

        int GetWaitingForVideoCountByTalent(Talent talent);

        int GetWaitingForPaymentCountByTalent(Talent talent);

        int GetWaitingForPaymentConfirmationCountByTalent(Talent talent);

        int GetPaymentConfirmedCountByTalent(Talent talent);

        IQueryable<VideoRequest> GetAllPaymentConfirmedByTalent(Talent talent);

        int GetEarnedByTalent(Talent talent);
    }
}