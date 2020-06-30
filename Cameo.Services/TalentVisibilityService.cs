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
    public class TalentVisibilityService : ITalentVisibilityService
    {
        public bool IsAvailable(Talent talent)
        {
            return talent.IsAvailable;
        }

        public bool IsApprovedByAdmin(Talent talent)
        {
            return talent.User.TalentApprovedByAdmin;
        }

        public bool HasAvatar(Talent talent)
        {
            return talent.AvatarID.HasValue && talent.AvatarID > 0;
        }

        public bool IsDeleted(Talent talent)
        {
            return talent.IsDeleted;
        }

        public bool IsPriceSet(Talent talent)
        {
            return talent.Price > 0;
        }

        public bool IsCreditCardProvided(Talent talent)
        {
            return !(talent.CreditCardNumber == null || talent.CreditCardNumber.Trim() == string.Empty)
                && talent.CreditCardExpire.HasValue;
        }

        public bool IsCategorySelected(Talent talent)
        {
            return talent.TalentCategories.Count > 0;
        }

        public List<string> BuildWarningTexts(Talent talent)
        {
            List<string> warningTexts = new List<string>();

            if (!IsAvailable(talent))
                warningTexts.Add("Вы недоступны");
            if (!IsApprovedByAdmin(talent))
                warningTexts.Add("Ваша заявка на таланта еще не одобрена");
            if (!HasAvatar(talent))
                warningTexts.Add("Не загружено фото профиля");
            if (IsDeleted(talent))
                warningTexts.Add("Вы удалены");
            if (!IsPriceSet(talent))
                warningTexts.Add("Не установлена цена за Ваше видео");
            if (!IsCreditCardProvided(talent))
                warningTexts.Add("Не предоставлены данные UZCARD для приема оплаты");
            if (!IsCategorySelected(talent))
                warningTexts.Add("К какой категории вы принадлежите?");

            return warningTexts;
        }
    }
}