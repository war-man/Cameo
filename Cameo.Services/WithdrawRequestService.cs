using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Cameo.Services
{
    public class WithdrawRequestService : BaseCRUDService<WithdrawRequest>, IWithdrawRequestService
    {
        private IWithdrawRequestRepository _withdrawRequestRepository;
        private readonly ITalentBalanceService _talentBalanceService;
        //protected int _tmpPeriodMinutes = 5;

        private readonly int MinimalAmountInBalanceForWithdrawal = 1000000;

        //public DocumentTypesEnum _documentType;

        public WithdrawRequestService(IWithdrawRequestRepository repository,
                           IUnitOfWork unitOfWork,
                           ITalentBalanceService talentBalanceService)
            : base(repository, unitOfWork)
        {
            _withdrawRequestRepository = repository;
            _talentBalanceService = talentBalanceService;
        }

        public void Add(WithdrawRequest entity, Talent talent /*string creatorID*/)
        {
            entity.StatusID = (int)WithdrawRequestStatusesEnum.Pending;
            _talentBalanceService.TakeOffBalance(talent, entity.Amount);

            base.Add(entity, talent.UserID /*creatorID*/);
        }

        public IQueryable<WithdrawRequest> GetManyWithRelatedDataAsIQueryable()
        {
            return _withdrawRequestRepository.GetManyWithRelatedDataAsIQueryable();
        }

        public IQueryable<WithdrawRequest> GetAllForAdmin()
        {
            return GetManyWithRelatedDataAsIQueryable();
        }

        public IQueryable<WithdrawRequest> GetAllByCreator(string userID)
        {
            return GetManyWithRelatedDataAsIQueryable()
                .Where(m => m.CreatedBy == userID);
        }

        public IQueryable<WithdrawRequest> Search(
            int? start,
            int? length,

            out int recordsTotal,
            out int recordsFiltered,
            out string error,

            UserTypesEnum curUserType,
            int? statusID,
            string authorID)
        {
            recordsTotal = 0;
            recordsFiltered = 0;
            error = "";

            try
            {
                IQueryable<WithdrawRequest> requests;

                if (curUserType == UserTypesEnum.Admin)
                    requests = GetAllForAdmin();
                else 
                    requests = GetAllByCreator(authorID);

                recordsTotal = requests.Count();

                if (statusID.HasValue)
                    requests = requests.Where(m => m.StatusID == statusID.Value);

                recordsFiltered = requests.Count();

                requests = requests.OrderByDescending(m => m.DateModified);
                if (start.HasValue && start.Value > 0)
                    requests = requests.Skip(start.Value);
                if (length.HasValue && length.Value > 0)
                    requests = requests.Take(length.Value);

                //return documents.OrderByDescending(m => m.DateModified);
                return requests;
            }
            catch (Exception ex)
            {
                error += ex.Message;
                if (ex.InnerException != null)
                    error += ". Inner exception: " + ex.InnerException.Message;

                return Enumerable.Empty<WithdrawRequest>().AsQueryable();
            }
        }

        public bool UserHasNotEnoughtMoneyForWithdrawal(int balance)
        {
            return balance < MinimalAmountInBalanceForWithdrawal;
        }

        public bool AmountIsLessThanMinimum(int amount)
        {
            return amount < MinimalAmountInBalanceForWithdrawal;
        }

        public int GetMinimalAmountInBalanceForWithdrawal()
        {
            return MinimalAmountInBalanceForWithdrawal;
        }

        public bool IsCompleted(WithdrawRequest model)
        {
            return model.StatusID == (int)WithdrawRequestStatusesEnum.Completed;
        }

        public void MarkAsCompleted(WithdrawRequest model, string adminID)
        {
            model.StatusID = (int)WithdrawRequestStatusesEnum.Completed;
            model.DateCompleted = DateTime.Now;

            Update(model, adminID);
        }
    }
}