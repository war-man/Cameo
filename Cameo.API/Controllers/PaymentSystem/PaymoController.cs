using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Common;
using Cameo.Models;
using Cameo.Services;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    //[Filter by Ip address()]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymoController : BaseController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IVideoRequestService _videoRequestService;

        private TelegramBotService TelegramBotService = new TelegramBotService();
        private string origin = "PaymoController ";

        public PaymoController(IInvoiceService invoiceService, IVideoRequestService videoRequestService)
        {
            _invoiceService = invoiceService;
            _videoRequestService = videoRequestService;
        }

        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    var curUser = accountUtil.GetCurrentUser(User);

        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[Authorize]
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    var curUser = accountUtil.GetCurrentUser(User);
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public ActionResult<PaymoTransactionResponseVM> Post([FromBody] PaymoTransactionRequestVM requestVM)
        {
            origin += "Post";
            TelegramBotService.SendMessage("Starting post", origin);

            string message = null;
            Invoice existingInvoice = null;

            try
            {
                if (requestVM.store_id != Constants.PAYMO.SETTINGS.STORE_ID)
                    message = "Неверный магазин";
                else
                {
                    var invoice = _invoiceService.GetByID(requestVM.invoice);
                    if (invoice == null)
                        message = "Инвойс с номером " + requestVM.invoice + " отсутствует в системе";
                    else
                    {
                        existingInvoice = invoice;

                        if (invoice.Amount != requestVM.amount)
                            message = "сумма инвойса не совпадает";
                        else
                        {
                            string text = requestVM.store_id
                                + requestVM.transaction_id 
                                + requestVM.invoice
                                + requestVM.amount
                                + Constants.PAYMO.SETTINGS.API_KEY;

                            using (MD5 md5Hash = MD5.Create())
                            {
                                text = GetMd5Hash(md5Hash, text);
                            }
                            if (!text.Equals(requestVM.sign))
                                message = "Подписи не совпадают";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TelegramBotService.SendMessage("1st catch: " + ex.Message, origin);
                message = ex.Message;
                if (ex.InnerException != null)
                    message += "Inner exception: " + ex.InnerException.Message;
            }

            //PaymoTransactionResponseVM responseVM = new PaymoTransactionResponseVM();
            int status = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    status = 1;
                    message = "Успешно";

                    try
                    {
                        existingInvoice.transaction_id = requestVM.transaction_id;
                        existingInvoice.transaction_time = requestVM.transaction_time;

                        _invoiceService.Update(existingInvoice, null);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    //откатить оплату
                    _invoiceService.MarkAsFailedToWithdrawMoney(existingInvoice);

                    var videoRequest = _videoRequestService.GetByID(existingInvoice.VideoRequestID.Value);
                    _videoRequestService.MarkAsFailedToWithdrawMoney(videoRequest);
                }
            }
            catch (Exception ex)
            {
                TelegramBotService.SendMessage("2nd catch: " + ex.Message, origin);
                message = "Системная ошибка";
            }

            TelegramBotService.SendMessage(message, origin);

            PaymoTransactionResponseVM responseVM = new PaymoTransactionResponseVM()
            {
                status = status,
                message = message
            };

            return Ok(responseVM);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        private string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
