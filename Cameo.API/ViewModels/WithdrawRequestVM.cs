using Cameo.Common;
using Cameo.Common.Utilities;
using Cameo.Models;
using System.ComponentModel.DataAnnotations;

namespace Cameo.API.ViewModels
{
    public class WithdrawRequestCreatePrepareVM
    {
        public bool talent_has_not_enought_money_for_withdrawal { get; set; }
        public int minimal_amount_in_balance_for_withdrawal { get; set; }
        public int talent_balance { get; set; }

        public WithdrawRequestCreatePrepareVM() { }

        public WithdrawRequestCreatePrepareVM(
            bool talentHasNotEnoughtMoneyForWithdrawal,
            int minimalAmountInBalanceForWithdrawal,
            int talentBalance)
        {
            talent_has_not_enought_money_for_withdrawal = talentHasNotEnoughtMoneyForWithdrawal;
            minimal_amount_in_balance_for_withdrawal = minimalAmountInBalanceForWithdrawal;
            talent_balance = talentBalance;
        }
    }

    public class WithdrawRequestCreateVM// : BaseVM
    {
        [Display(Name = "Сумма")]
        public int amount { get; set; }

        public WithdrawRequestCreateVM() { }

        //public WithdrawRequestCreateVM(Document model)
        //    : base(model)
        //{
        //    if (model == null)
        //        return;

        //    Title = model.Title;
        //}

        public virtual WithdrawRequest ToModel()
        {
            WithdrawRequest model = new WithdrawRequest()
            {
                Amount = this.amount,
                //StatusID = (int)WithdrawRequestStatusesEnum.Pending
            };

            return model;
        }
    }

    public class WithdrawRequestListItemForExpertAndUserVM
    {
        public int id { get; set; }

        [Display(Name = "Сумма")]
        public int amount { get; set; }

        [Display(Name = "Сумма")]
        public string amount_str { get; set; }

        [Display(Name = "Статус")]
        public BaseDropdownableDetailsVM status { get; set; }

        [Display(Name = "Дата создания")]
        public string date_created { get; set; }

        [Display(Name = "Дата выполнения")]
        public string date_completed { get; set; }

        public WithdrawRequestListItemForExpertAndUserVM() { }

        public WithdrawRequestListItemForExpertAndUserVM(WithdrawRequest model)
        {
            if (model == null)
                return;

            id = model.ID;
            amount = model.Amount;
            amount_str = model.Amount.ToString(AppData.Configuration.NumberViewStringFormat);

            status = new BaseDropdownableDetailsVM(model.Status);
            date_created = DateTimeUtils.ConvertToString(model.DateCreated);
            date_completed = DateTimeUtils.ConvertToString(model.DateCompleted);
        }
    }

    //public class WithdrawRequestListItemForAdminVM : WithdrawRequestListItemForExpertAndUserVM
    //{
    //    public AppUserVM User { get; set; }

    //    public WithdrawRequestListItemForAdminVM() { }

    //    public WithdrawRequestListItemForAdminVM(WithdrawRequest model)
    //        : base(model)
    //    {
    //        User = new AppUserVM(model.Creator);
    //    }
    //}
}
