using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;

namespace Cameo.Data.Repository
{
    public class InvoiceRepository : BaseCRUDRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}