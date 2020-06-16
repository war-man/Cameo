using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;

namespace Cameo.Services
{
    public class ClickTransactionService : BaseCRUDService<ClickTransaction>, IClickTransactionService
    {
        public ClickTransactionService(IClickTransactionRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public void MarkTransactionAsCanceled(ClickTransaction transaction)
        {
            transaction.StatusID = (int)ClickTransactionStatusEnum.CANCELLED;
            transaction.DateCancelled = DateTime.Now;

            Update(transaction, null);
        }

        public void MarkTransactionAsPaid(ClickTransaction transaction)
        {
            transaction.StatusID = (int)ClickTransactionStatusEnum.SUCCESS;
            transaction.DateSuccess = DateTime.Now;

            Update(transaction, null);
        }

        public bool IsTransactionPaid(ClickTransaction transaction)
        {
            return transaction.StatusID == (int)ClickTransactionStatusEnum.SUCCESS;
        }

        public bool IsTransactionCancelled(ClickTransaction transaction)
        {
            return transaction.StatusID == (int)ClickTransactionStatusEnum.CANCELLED;
        }
    }
}