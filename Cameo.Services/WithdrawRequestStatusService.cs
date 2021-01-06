using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services
{
    public class WithdrawRequestStatusService : BaseDropdownableService<WithdrawRequestStatus>, IWithdrawRequestStatusService
    {
        public WithdrawRequestStatusService(IWithdrawRequestStatusRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}