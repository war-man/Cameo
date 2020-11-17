//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using Cameo.API.ViewModels;
//using Cameo.Common;
//using Cameo.Models;
//using Cameo.Services.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace Cameo.API.Controllers
//{
//    //[AllowAnonymous()]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ClickController : BaseController
//    {
//        private readonly ICustomerService CustomerService;
//        private readonly ICustomerBalanceService CustomerBalanceService;
//        private readonly IClickTransactionService ClickTransactionService;

//        public ClickController(
//            ICustomerService customerService,
//            ICustomerBalanceService customerBalanceService,
//            IClickTransactionService clickTransactionService,
//            ILogger<ClickController> logger)
//        {
//            CustomerService = customerService;
//            CustomerBalanceService = customerBalanceService;
//            ClickTransactionService = clickTransactionService;
//            _logger = logger;
//        }

//        [HttpPost("prepare")]
//        public ActionResult<Dictionary<string, string>> prepare([FromForm] ClickRequestVM requestVM)
//        {
//            ClickResponseVM responseVM = new ClickResponseVM();
//            try
//            {
//                ClickResponseVM result = checkPrepare(requestVM, responseVM);
//                return result.toMap();
//            }
//            catch (Exception ex)
//            {
//                responseVM.error = Constants.CLICK.ERRORS.Error_in_request_from_click.code;
//                responseVM.error_note = Constants.CLICK.ERRORS.Error_in_request_from_click.note;

//                return responseVM.toMap();
//            }
//        }

//        private ClickResponseVM checkPrepare(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            //Akbar's version
//            if (!checkRequest(requestDTO, responseDTO)) ;
//            else if (!checkSign(requestDTO, responseDTO)) ;
//            else if (!checkUserExistence(requestDTO, responseDTO)) ;
//            else if (!checkAction(requestDTO, responseDTO)) ;
//            else userUpdatePrepare(requestDTO, responseDTO);

//            responseDTO.click_trans_id = requestDTO.click_trans_id.Value;
//            responseDTO.merchant_trans_id = requestDTO?.merchant_trans_id;

//            return responseDTO;
//        }

//        [HttpPost("complete")]
//        public ActionResult<Dictionary<string, string>> complete([FromForm] ClickRequestVM requestVM)
//        {
//            ClickResponseVM responseVM = new ClickResponseVM();
//            try
//            {
//                ClickResponseVM result = checkComplete(requestVM, responseVM);

//                return result.toMap();
//            }
//            catch (Exception ex)
//            {
//                responseVM.error = Constants.CLICK.ERRORS.Error_in_request_from_click.code;
//                responseVM.error_note = Constants.CLICK.ERRORS.Error_in_request_from_click.note;

//                return responseVM.toMap();
//            }
//        }

//        private ClickResponseVM checkComplete(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//    {
//            //Akbar's version
//            if (!checkRequest(requestDTO, responseDTO)) ;
//            else if (!checkSign(requestDTO, responseDTO)) ;
//            else if (!checkUserExistence(requestDTO, responseDTO)) ;
//            else if (!checkTransactionExistence(requestDTO, responseDTO)) ;
//            else if (!checkTransactionPaid(requestDTO, responseDTO)) ;
//            else if (!checkAmount(requestDTO, responseDTO)) ;
//            else if (!checkRequestError(requestDTO, responseDTO)) ;
//            else if (!checkAction(requestDTO, responseDTO)) ;
//            else if (!checkTransactionCancelled(requestDTO, responseDTO)) ;
//            else userUpdateComplete(requestDTO, responseDTO);

//            responseDTO.click_trans_id = requestDTO.click_trans_id ?? 0;
//            responseDTO.merchant_trans_id = requestDTO.merchant_trans_id;

//            return responseDTO;
//        }


//        private bool checkRequest(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            bool checkResult = true;

//            if (intIsNullOrLessThanOrEqualToZero(requestDTO.click_trans_id))
//                checkResult = false;
//            if (requestDTO.service_id == null || requestDTO.service_id != Constants.CLICK.SETTINGS.SERVICE_id)
//                checkResult = false;
//            if (intIsNullOrLessThanOrEqualToZero(requestDTO.click_paydoc_id))
//                checkResult = false;
//            if (string.IsNullOrWhiteSpace(requestDTO.merchant_trans_id))
//                checkResult = false;

//            if (requestDTO.action == Constants.CLICK.ACTIONS.COMPLETE)
//            {
//                if (longIsNullOrLessThanOrEqualToZero(requestDTO.merchant_prepare_id))
//                    checkResult = false;
//            }

//            if (floatIsNullOrLessThanOrEqualToZero(requestDTO.amount))
//                checkResult = false;
//            if (string.IsNullOrWhiteSpace(requestDTO.sign_time))
//                checkResult = false;

//            try
//            {
//                var tmp = requestDTO.sign_time?.Split(' ');
//                DateTime.ParseExact(tmp[0], "yyyy-MM-dd", null);
//                DateTime.ParseExact(tmp[1], "HH:mm:ss", null);
//            }
//            catch (Exception exception)
//            {
//                checkResult = false;
//            }

//            if (string.IsNullOrWhiteSpace(requestDTO.sign_string))
//                checkResult = false;
    
//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Error_in_request_from_click.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Error_in_request_from_click.note;
//            }

//            return checkResult;
//        }

//        private bool checkAction(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            bool checkResult = (requestDTO.action == Constants.CLICK.ACTIONS.PREPARE
//                || requestDTO.action == Constants.CLICK.ACTIONS.COMPLETE);

//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Action_not_found.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Action_not_found.note;
//            }

//            return checkResult;
//        }

//        private bool checkSign(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            string text = ("" + requestDTO.click_trans_id
//                    + requestDTO.service_id
//                    + Constants.CLICK.SETTINGS.SECRET_KEY
//                    + requestDTO.merchant_trans_id);

//            if (requestDTO.action == Constants.CLICK.ACTIONS.COMPLETE)
//                text += requestDTO.merchant_prepare_id;

//            text += ("" + (int)requestDTO.amount
//                + requestDTO.action
//                + requestDTO.sign_time);

//            using (MD5 md5Hash = MD5.Create())
//            {
//                text = GetMd5Hash(md5Hash, text);
//            }
//            var checkResult = (text.Equals(requestDTO.sign_string));

//            //checkResult = true

//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.SIGN_CHECK_FAILED.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.SIGN_CHECK_FAILED.note;
//            }

//            return checkResult;
//        }

//        private bool checkAmount(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            ClickTransaction transaction = ClickTransactionService.GetByID((int?)(requestDTO.merchant_prepare_id) ?? 0);
//            bool checkResult = transaction.Amount == requestDTO.amount;
//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Incorrect_parameter_amount.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Incorrect_parameter_amount.note;
//            }

//            return checkResult;
//        }

//        private bool checkUserExistence(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            bool checkResult = (CustomerService.GetByAccountNumber(requestDTO.merchant_trans_id) != null);
//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.User_does_not_exist.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.User_does_not_exist.note;
//            }

//            return checkResult;
//        }

//        private bool checkTransactionExistence(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            bool checkResult = ClickTransactionService.GetByID((int?)(requestDTO.merchant_prepare_id) ?? 0) != null;
//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Transaction_does_not_exist.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Transaction_does_not_exist.note;
//            }

//            return checkResult;
//        }

//        private bool checkRequestError(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            bool checkResult = !(requestDTO.error == null || requestDTO.error < 0);
//            if (!checkResult)
//            {
//                if (!longIsNullOrLessThanOrEqualToZero(requestDTO.merchant_prepare_id))
//                {
//                    ClickTransaction transaction = ClickTransactionService.GetByID((int?)(requestDTO.merchant_prepare_id) ?? 0);
//                    ClickTransactionService.MarkTransactionAsCanceled(transaction);
//                }

//                responseDTO.error = Constants.CLICK.ERRORS.Request_has_error.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Request_has_error.note;
//            }

//            return checkResult;
//        }

//        private bool checkTransactionPaid(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            ClickTransaction transaction = ClickTransactionService.GetByID((int?)(requestDTO.merchant_prepare_id) ?? 0);

//            bool checkResult = !ClickTransactionService.IsTransactionPaid(transaction);
//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Already_paid.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Already_paid.note;
//            }

//            return checkResult;
//        }

//        private bool checkTransactionCancelled(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            ClickTransaction transaction = ClickTransactionService.GetByID((int?)(requestDTO.merchant_prepare_id) ?? 0);

//            bool checkResult = !ClickTransactionService.IsTransactionCancelled(transaction);
//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Transaction_cancelled.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Transaction_cancelled.note;
//            }

//            return checkResult;
//        }

//        private void userUpdatePrepare(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            Customer user = CustomerService.GetByAccountNumber(requestDTO.merchant_trans_id);
//            ClickTransaction transaction = requestDTO.toModel(user);
//            ClickTransactionService.Add(transaction, null);

//            long merchant_prepare_id = transaction.ID;

//            if (merchant_prepare_id == 0L)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Failed_to_update_user.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Failed_to_update_user.note;
//            }
//            else
//            {
//                responseDTO.merchant_prepare_id = merchant_prepare_id;
//                responseDTO.error = Constants.CLICK.ERRORS.Success.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Success.note;
//            }
//        }

//        private void userUpdateComplete(ClickRequestVM requestDTO, ClickResponseVM responseDTO)
//        {
//            //update transaction and user's balance
//            Customer user = CustomerService.GetByAccountNumber(requestDTO.merchant_trans_id);
//            CustomerBalanceService.ReplenishBalance(user, (int?)(requestDTO.amount) ?? 0);
//            bool checkResult = true;
//            if (checkResult)
//            {
//                ClickTransaction transaction = ClickTransactionService.GetByID((int?)(requestDTO.merchant_prepare_id) ?? 0);
//                checkResult = ClickTransactionService.IsTransactionCancelled(transaction);
//                if (checkResult)
//                {
//                    responseDTO.error = Constants.CLICK.ERRORS.Transaction_cancelled.code;
//                    responseDTO.error_note = Constants.CLICK.ERRORS.Transaction_cancelled.note;

//                    return;
//                }

//                ClickTransactionService.MarkTransactionAsPaid(transaction);
//                checkResult = true;
//                if (!checkResult)
//                {
//                    CustomerBalanceService.TakeOffBalance(user, (int?)(requestDTO.amount) ?? 0);
//                }
//            }

//            if (!checkResult)
//            {
//                responseDTO.error = Constants.CLICK.ERRORS.Failed_to_update_user.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Failed_to_update_user.note;
//            }
//            else
//            {
//                responseDTO.merchant_confirm_id = requestDTO.merchant_prepare_id;
//                responseDTO.error = Constants.CLICK.ERRORS.Success.code;
//                responseDTO.error_note = Constants.CLICK.ERRORS.Success.note;
//            }
//        }

//        [HttpGet("/generate_button_url")]
//        public ActionResult<string> generateButtonUrl(int amount, string account_number, string return_url)
//        {
//            var result = "https://my.click.uz/services/pay?service_id=" + Constants.CLICK.SETTINGS.SERVICE_id + "&merchant_id=" + Constants.CLICK.SETTINGS.MERCHANT_ID + "&amount=" + amount + ",00&transaction_param=" + account_number;

//            if (!string.IsNullOrWhiteSpace(return_url))
//                    result += "&return_url=$return_url";

//            return result;
//        }

//        private bool intIsNullOrLessThanOrEqualToZero(int? value)
//        {
//            if (value == null || value <= 0)
//                return true;
//            return false;
//        }

//        private bool longIsNullOrLessThanOrEqualToZero(long? value)
//        {
//            if (value == null || value <= 0)
//                return true;
//            return false;
//        }

//        private bool floatIsNullOrLessThanOrEqualToZero(float? value)
//        {
//            if (value == null || value <= 0)
//                return true;
//            return false;
//        }

//        private string GetMd5Hash(MD5 md5Hash, string input)
//        {

//            // Convert the input string to a byte array and compute the hash.
//            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

//            // Create a new Stringbuilder to collect the bytes
//            // and create a string.
//            StringBuilder sBuilder = new StringBuilder();

//            // Loop through each byte of the hashed data
//            // and format each one as a hexadecimal string.
//            for (int i = 0; i < data.Length; i++)
//            {
//                sBuilder.Append(data[i].ToString("x2"));
//            }

//            // Return the hexadecimal string.
//            return sBuilder.ToString();
//        }
//    }
//}
