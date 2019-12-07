using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using System.Linq;

namespace Cameo.Services
{
    public class LogTalentPriceService : BaseCRUDService<LogTalentPrice>, ILogTalentPriceService
    {
        public LogTalentPriceService(ILogTalentPriceRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}