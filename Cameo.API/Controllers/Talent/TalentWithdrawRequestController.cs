using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cameo.API.Controllers
{
    [Authorize(Policy = "TalentOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class TalentWithdrawRequestController : BaseController
    {
        private readonly IWithdrawRequestService WithdrawRequestService;
        private readonly IWithdrawRequestStatusService WithdrawRequestStatusService;
        //private readonly IUserBalanceService UserBalanceService;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITalentService TalentService;
        private readonly ITalentBalanceService TalentBalanceService;

        public TalentWithdrawRequestController(
            IWithdrawRequestService withdrawRequestService,
            IWithdrawRequestStatusService withdrawRequestStatusService,
            //IUserBalanceService userBalanceService,
            //UserManager<ApplicationUser> userManager,
            ITalentService talentService,
            ITalentBalanceService talentBalanceService,
            ILogger<TalentWithdrawRequestController> logger)
        {
            WithdrawRequestService = withdrawRequestService;
            WithdrawRequestStatusService = withdrawRequestStatusService;
            //UserBalanceService = userBalanceService;
            //_userManager = userManager;
            TalentService = talentService;
            TalentBalanceService = talentBalanceService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<object> Index(int draw, int? start = null, int? length = null,
            int? statusID = null)
        {
            try
            {
                int recordsTotal = 0;
                int recordsFiltered = 0;
                string error = "";

                var curUser = accountUtil.GetCurrentUser(User);
                var talent = TalentService.GetByUserID(curUser.ID);
                if (talent == null)
                    throw new Exception("Талант не найден");

                IQueryable<WithdrawRequest> dataIQueryable = WithdrawRequestService.Search(
                    start,
                    length,

                    out recordsTotal,
                    out recordsFiltered,
                    out error,

                    statusID,
                    talent.ID
                );

                if (!string.IsNullOrWhiteSpace(error))
                    throw new Exception("Ошибка при получении списка");

                dynamic data = dataIQueryable
                    .Select(m => new WithdrawRequestListItemForExpertAndUserVM(m))
                    .ToList();

                return new
                {
                    //draw = draw,
                    records_total = recordsTotal,
                    //recordsFiltered = recordsFiltered,
                    data = data,
                    //error = error
                };
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpGet("CreatePrepare")]
        public ActionResult<WithdrawRequestCreatePrepareVM> CreatePrepare()
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                var talent = TalentService.GetByUserID(curUser.ID);
                int talentBalance = TalentBalanceService.GetBalance(talent);

                WithdrawRequestCreatePrepareVM createPrepareVM = new WithdrawRequestCreatePrepareVM(
                    WithdrawRequestService.UserHasNotEnoughtMoneyForWithdrawal(talentBalance),
                    WithdrawRequestService.GetMinimalAmountInBalanceForWithdrawal(),
                    talentBalance);

                return createPrepareVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] WithdrawRequestCreateVM modelVM)
        {
            var curUser = accountUtil.GetCurrentUser(User);
            var talent = TalentService.GetByUserID(curUser.ID);

            try
            {
                if (ModelState.IsValid)
                {
                    //var curUser = accountUtil.GetCurrentUser(User);
                    int talentBalance = TalentBalanceService.GetBalance(talent);

                    if (WithdrawRequestService.UserHasNotEnoughtMoneyForWithdrawal(talentBalance))
                        throw new Exception("Ваш баланс не позволяет подать заявку на вывод средств");

                    if (WithdrawRequestService.AmountIsLessThanMinimum(modelVM.amount))
                        throw new Exception("Указанная сумма меньше минимального порога вывода средств");

                    if (TalentBalanceService.BalanceIsLessThan(talent, modelVM.amount))
                        throw new Exception("На Вашем балансе недостаточно средств для снятия суммы " + modelVM.amount);

                    var model = modelVM.ToModel();
                    WithdrawRequestService.Add(model, talent);

                    return Ok(new { id = model.ID });
                }
                else
                    throw new Exception("Что-то не так");
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}