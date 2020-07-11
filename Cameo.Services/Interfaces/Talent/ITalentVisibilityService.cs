using Cameo.Models;
using Cameo.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services.Interfaces
{
    public interface ITalentVisibilityService
    {
        bool IsAvailable(Talent talent);
        bool IsApprovedByAdmin(Talent talent);
        bool HasAvatar(Talent talent);
        bool IsDeleted(Talent talent);
        bool IsPriceSet(Talent talent);
        bool IsCreditCardProvided(Talent talent);
        bool IsCategorySelected(Talent talent);

        List<string> BuildWarningTexts(Talent talent);
    }
}