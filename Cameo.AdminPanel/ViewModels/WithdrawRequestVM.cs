using Cameo.Common;
using Cameo.Common.Utilities;
using Cameo.Models;
using System.ComponentModel.DataAnnotations;

namespace Cameo.AdminPanel.ViewModels
{
    public class WithdrawRequestListItemForExpertAndUserVM
    {
        public int ID { get; set; }

        [Display(Name = "Сумма")]
        public int Amount { get; set; }

        [Display(Name = "Сумма")]
        public string AmountStr { get; set; }

        [Display(Name = "Статус")]
        public BaseDropdownableDetailsVM Status { get; set; }

        [Display(Name = "Дата создания")]
        public string DateCreated { get; set; }

        [Display(Name = "Дата выполнения")]
        public string DateCompleted { get; set; }

        public WithdrawRequestListItemForExpertAndUserVM() { }

        public WithdrawRequestListItemForExpertAndUserVM(WithdrawRequest model)
        {
            if (model == null)
                return;

            ID = model.ID;
            Amount = model.Amount;

            //AppData.Configuration is null. don't know why
            //AmountStr = model.Amount.ToString(AppData.Configuration.NumberViewStringFormat);
            AmountStr = model.Amount.ToString("### ### ### ### ##0");

            Status = new BaseDropdownableDetailsVM(model.Status);
            DateCreated = DateTimeUtils.ConvertToString(model.DateCreated);
            DateCompleted = DateTimeUtils.ConvertToString(model.DateCompleted);
        }
    }

    public class WithdrawRequestListItemForAdminVM : WithdrawRequestListItemForExpertAndUserVM
    {
        public TalentShortInfoVM Talent { get; set; }

        public WithdrawRequestListItemForAdminVM() { }

        public WithdrawRequestListItemForAdminVM(WithdrawRequest model)
            : base(model)
        {
            Talent = new TalentShortInfoVM(model.Talent);
        }
    }
}
