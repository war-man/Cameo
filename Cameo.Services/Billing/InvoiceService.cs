using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System;

namespace Cameo.Services
{
    public class InvoiceService : BaseCRUDService<Invoice>, IInvoiceService
    {
        public InvoiceService(IInvoiceRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public override void Add(Invoice entity, string creatorID)
        {
            entity.StatusID = 1; //pending; must be refactored to enum
            entity.hold_till = DateTime.Now.AddDays(5); // value of hold_till must be taken from AppSettings

            base.Add(entity, creatorID);
        }

        public void AssignHoldID(Invoice entity, string hold_id, string creatorID)
        {
            entity.hold_id = hold_id;
            Update(entity, creatorID);
        }

        //public void MarkTransactionAsCanceled(ClickTransaction transaction)
        //{
        //    transaction.StatusID = (int)ClickTransactionStatusEnum.CANCELLED;
        //    transaction.DateCancelled = DateTime.Now;

        //    Update(transaction, null);
        //}

        //public void MarkTransactionAsPaid(ClickTransaction transaction)
        //{
        //    transaction.StatusID = (int)ClickTransactionStatusEnum.SUCCESS;
        //    transaction.DateSuccess = DateTime.Now;

        //    Update(transaction, null);
        //}

        //public bool IsTransactionPaid(ClickTransaction transaction)
        //{
        //    return transaction.StatusID == (int)ClickTransactionStatusEnum.SUCCESS;
        //}

        //public bool IsTransactionCancelled(ClickTransaction transaction)
        //{
        //    return transaction.StatusID == (int)ClickTransactionStatusEnum.CANCELLED;
        //}
    }
}