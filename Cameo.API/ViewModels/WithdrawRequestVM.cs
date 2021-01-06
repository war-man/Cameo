using Cameo.Common;
using Cameo.Common.Utilities;
using Cameo.Models;
using System.ComponentModel.DataAnnotations;

namespace Cameo.API.ViewModels
{
    public class WithdrawRequestCreatePrepareVM
    {
        public bool TalentHasNotEnoughtMoneyForWithdrawal { get; set; }
        public int MinimalAmountInBalanceForWithdrawal { get; set; }
        public int TalentBalance { get; set; }

        public WithdrawRequestCreatePrepareVM() { }

        public WithdrawRequestCreatePrepareVM(
            bool talentHasNotEnoughtMoneyForWithdrawal,
            int minimalAmountInBalanceForWithdrawal,
            int talentBalance)
        {
            TalentHasNotEnoughtMoneyForWithdrawal = talentHasNotEnoughtMoneyForWithdrawal;
            MinimalAmountInBalanceForWithdrawal = minimalAmountInBalanceForWithdrawal;
            TalentBalance = TalentBalance;
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
            AmountStr = model.Amount.ToString(AppData.Configuration.NumberViewStringFormat);

            Status = new BaseDropdownableDetailsVM(model.Status);
            DateCreated = DateTimeUtils.ConvertToString(model.DateCreated);
            DateCompleted = DateTimeUtils.ConvertToString(model.DateCompleted);
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
