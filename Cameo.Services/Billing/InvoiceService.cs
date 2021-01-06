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
            entity.StatusID = (int)PaymoInvoiceStatusEnum.PENDING;
            entity.hold_till = DateTime.Now.AddDays(5); // value of hold_till must be taken from AppSettings

            base.Add(entity, creatorID);
        }

        public void AssignHoldID(Invoice entity, string hold_id, string creatorID)
        {
            entity.hold_id = hold_id;
            Update(entity, creatorID);
        }

        public void MarkAsSuccess(Invoice entity)
        {
            entity.StatusID = (int)PaymoInvoiceStatusEnum.CANCELLED;
            entity.DateSuccess = DateTime.Now;

            Update(entity, null);
        }

        public void MarkAsCanceled(Invoice entity)
        {
            entity.StatusID = (int)PaymoInvoiceStatusEnum.CANCELLED;
            entity.DateCancelled = DateTime.Now;

            Update(entity, null);
        }

        

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