using Cameo.Common;
using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cameo.Services
{
    public class PaymoService : IPaymoService
    {
        private readonly IInvoiceService InvoiceService;

        public PaymoService(IInvoiceService invoiceService)
        {
            InvoiceService = invoiceService;
        }

        //DONE
        private string GetAuthToken()
        {
            string token = null;

            string key = Constants.PAYMO.SETTINGS.CONSUMER_KEY;
            string secret = Constants.PAYMO.SETTINGS.CONSUMER_SECRET;
            string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(key + ":" + secret));

            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(Constants.PAYMO.URLS.TOKEN_GENERATOR + "?grant_type=client_credentials"),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), "Basic " + encoded },
                    }
                };
                using (HttpResponseMessage res = Task.Run(() => client.SendAsync(httpRequestMessage)).Result)
                //using (HttpResponseMessage res = await client.SendAsync(httpRequestMessage))
                {
                    using (HttpContent content = res.Content)
                    {
                        string dataStr = Task.Run(() => content.ReadAsStringAsync()).Result;
                        //string dataStr = await content.ReadAsStringAsync();

                        if (res.StatusCode != HttpStatusCode.OK)
                            throw new Exception("GetAuthToken: " + dataStr);
                        else if (dataStr != null)
                        {
                            HoldAuthResponse response = JsonConvert.DeserializeObject<HoldAuthResponse>(dataStr);
                            token = response.access_token;
                        }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Не удалось получить токен Paymo");

            return token;
        }

        //DONE
        //POST: https://api.paymo.uz/hold/
        public string ApplyForHold(Invoice invoice)
        {
            string token = GetAuthToken();
            HoldPostRequest holdPostRequestBody = new HoldPostRequest(invoice);

            string hold_id = null;

            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(Constants.PAYMO.URLS.HOLD),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), "Bearer " + token },
                        { HttpRequestHeader.ContentType.ToString(), "application/json" },
                        { HttpRequestHeader.Accept.ToString(), "application/json" },
                    },
                    Content = new StringContent(JsonConvert.SerializeObject(holdPostRequestBody), Encoding.UTF8, "application/json")
                };
                using (HttpResponseMessage res = Task.Run(() => client.SendAsync(httpRequestMessage)).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string dataStr = Task.Run(() => content.ReadAsStringAsync()).Result;

                        if (res.StatusCode != HttpStatusCode.OK)
                            throw new Exception("ApplyForHold: " + dataStr);
                        else if (dataStr != null)
                        {
                            HoldPostResponse response = JsonConvert.DeserializeObject<HoldPostResponse>(dataStr);
                            if (response.result.code != "OK")
                                throw new Exception("ApplyForHold: " + dataStr);

                            hold_id = response.hold_id;
                        }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(hold_id))
                throw new Exception("Не удалось создать заявку на холдирование");

            return hold_id;
        }

        //DONE
        //PUT: https://api.paymo.uz/hold/{hold_id}
        public void ConfirmHold(Invoice invoice, string sms)
        {
            if (string.IsNullOrWhiteSpace(invoice?.hold_id))
                throw new Exception("hold_id не присвоен");

            string token = GetAuthToken();
            HoldPutRequest holdPutRequestBody = new HoldPutRequest(sms);

            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(Constants.PAYMO.URLS.HOLD + "/" + invoice?.hold_id),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), "Bearer " + token },
                        { HttpRequestHeader.ContentType.ToString(), "application/json" },
                        { HttpRequestHeader.Accept.ToString(), "application/json" },
                    },
                    Content = new StringContent(JsonConvert.SerializeObject(holdPutRequestBody), Encoding.UTF8, "application/json")
                };
                using (HttpResponseMessage res = Task.Run(() => client.SendAsync(httpRequestMessage)).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string dataStr = Task.Run(() => content.ReadAsStringAsync()).Result;

                        if (res.StatusCode != HttpStatusCode.OK)
                            throw new Exception("ConfirmHold: " + dataStr);
                        else if (dataStr != null)
                        {
                            HoldPutResponse response = JsonConvert.DeserializeObject<HoldPutResponse>(dataStr);
                            try
                            {
                                if (response.result.code != "OK")
                                    throw new Exception("ConfirmHold: " + dataStr);
                                else
                                {
                                    DateTime tmp;
                                    if (DateTime.TryParse(response.hold_till, out tmp))
                                        invoice.hold_till = tmp;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Не удалось подтвердить холдирование");
                            }
                        }
                    }
                }
            }
        }

        //POST: https://api.pays.uz/hold/{hold_id}
        public void PerformHold(Invoice invoice)
        {
            if (string.IsNullOrWhiteSpace(invoice?.hold_id))
                throw new Exception("hold_id не присвоен");

            string token = GetAuthToken();

            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(Constants.PAYMO.URLS.HOLD + "/" + invoice?.hold_id),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), "Bearer " + token },
                        { HttpRequestHeader.ContentType.ToString(), "application/json" },
                        { HttpRequestHeader.Accept.ToString(), "application/json" },
                    },
                    Content = new StringContent(JsonConvert.SerializeObject(new HoldRequest()), Encoding.UTF8, "application/json")
                };
                using (HttpResponseMessage res = Task.Run(() => client.SendAsync(httpRequestMessage)).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string dataStr = Task.Run(() => content.ReadAsStringAsync()).Result;

                        if (res.StatusCode != HttpStatusCode.OK)
                            throw new Exception("PerformHold: " + dataStr);
                        else if (dataStr != null)
                        {
                            HoldPostPerformResponse response = JsonConvert.DeserializeObject<HoldPostPerformResponse>(dataStr);
                            if (response.result.code != "OK")
                                throw new Exception("PerformHold: " + dataStr);

                            try
                            {
                                invoice.commission_value = float.Parse(response.store_transaction.commission_value);
                                invoice.terminal_id = response.store_transaction.terminal_id;
                                invoice.prepay_time = response.store_transaction.prepay_time;
                                invoice.confirm_time = response.store_transaction.confirm_time;
                                invoice.status_code = response.store_transaction.status_code;
                                invoice.status_message = response.store_transaction.status_message;
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                }
            }

            InvoiceService.MarkAsSuccess(invoice);
        }

        //DONE
        //DELETE: https://api.pays.uz/hold/{hold_id}
        public void CancelHold(Invoice invoice)
        {
            if (string.IsNullOrWhiteSpace(invoice?.hold_id))
                throw new Exception("hold_id не присвоен");

            string token = GetAuthToken();

            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(Constants.PAYMO.URLS.HOLD + "/" + invoice?.hold_id),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), "Bearer " + token },
                        { HttpRequestHeader.ContentType.ToString(), "application/json" },
                        { HttpRequestHeader.Accept.ToString(), "application/json" }
                    }
                };
                using (HttpResponseMessage res = Task.Run(() => client.SendAsync(httpRequestMessage)).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string dataStr = Task.Run(() => content.ReadAsStringAsync()).Result;

                        if (res.StatusCode != HttpStatusCode.OK)
                            throw new Exception("CancelHold: " + dataStr);
                        else if (dataStr != null)
                        {
                            HoldDeleteResponse response = JsonConvert.DeserializeObject<HoldDeleteResponse>(dataStr);
                            try
                            {
                                if (response.result.code != "OK")
                                    throw new Exception("CancelHold: " + dataStr);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Не удалось отменить холдирование");
                            }
                        }
                    }
                }
            }

            InvoiceService.MarkAsCanceled(invoice);
        }
    }

    public class HoldAuthResponse
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }

    #region Hold base classes
    public class HoldRequest
    {
        public string lang { get; set; } //язык ответного сообщения

        public HoldRequest()
        {
            lang = "ru";
        }
    }

    public class HoldResponse
    {
        public HoldResult result { get; set; }
    }

    public class HoldResult
    {
        public string code { get; set; }
        public string description { get; set; }
    }
    #endregion

    #region Hold Post (create zayavka for hold)
    public class HoldPostRequest : HoldRequest
    {
        public string card_number { get; set; } //Номер карты
        public string card_expiry { get; set; } //Дата истечения карты
        public int store_id { get; set; } //ID магазина, предоставленный системой PAYMO
        public string account { get; set; } //Идентификатор инвойса (логин, номер инвойса и т.п.)
        public string payment_details { get; set; } //Дополнительная информация о платеже
        public string amount { get; set; } //Сумма платежа
        public string duration { get; set; } //Длительность холдирования в минутах

        public HoldPostRequest() { }

        public HoldPostRequest(Invoice invoice)
            : base()
        {
            card_number = invoice.card_number.Replace(" ", "");
            //card_expiry = invoice.card_expiry.ToString("MM") + "/" + invoice.card_expiry.ToString("yy");
            card_expiry = invoice.card_expiry.ToString("yy") + invoice.card_expiry.ToString("MM"); //очень интересный подход :)
            store_id = Constants.PAYMO.SETTINGS.STORE_ID;
            account = invoice.ID.ToString();
            payment_details = "холдирование видео";
            amount = invoice.Amount.ToString();
            duration = invoice.duration_in_minutes.ToString();
        }
    }

    public class HoldPostResponse : HoldResponse
    {
        public string hold_id { get; set; }
    }
    #endregion

    #region Hod Put (create hold)
    public class HoldPutRequest : HoldRequest
    {
        public string otp { get; set; } //Код подтверждения, высланный в SMS

        public HoldPutRequest() { }

        public HoldPutRequest(string smsCode)
            : base()
        {
            otp = smsCode;
        }
    }

    public class HoldPutResponse : HoldResponse
    {
        public string hold_id { get; set; } //Идентификатор холдирования
        public string card_token { get; set; } //Токен карты в системе PAYMO
        public string card_pan { get; set; } //Маска карты

        public string hold_till { get; set; } //Время окончания холдирования (YYYY-mm-ddTHH:mm:ii)
    }
    #endregion

    #region Hold Post (perform hold)
    public class HoldPostPerformRequest : HoldRequest
    {
    }

    public class HoldPostPerformResponse : HoldResponse
    {
        public StoreTransaction store_transaction { get; set; } //объект с информацией о транзакции
    }

    public class StoreTransaction //объект с информацией о транзакции
    {
        public string success_trans_id { get; set; } //ID транзакции
        public Store store { get; set; } //объект с информации о поставщике (магазине)
        public string card_id { get; set; } //Токен карты плательщика
        public string account { get; set; } //Идентификатор инвойса
        public string commission_type { get; set; } //Форма комиссии с плательщика (1 – процент, 2 – фиксированная)
        public string commission_value { get; set; } //Размер комиссии
        public string amount { get; set; } //Сумма инвойса
        public string total { get; set; } //Сумма к оплате
        //public string success_trans_id { get; set; } //ID завершенной платежной транзакции в системе PAYMO
        public string terminal_id { get; set; } //ID терминала
        public string prepay_time { get; set; } //Время создания драфта транзакции
        public string confirm_time { get; set; } //Время завершения транзакции
        public string status_code { get; set; } //Код состояния транзакции
        public string status_message { get; set; } //Сообщение состояния транзакции
    }

    public class Store
    {
        public string id { get; set; } //ID поставщика (магазина)
        public string name { get; set; } //Название поставщика (магазина)
        public string logo { get; set; } //Логотип поставщика (магазина)
        public string desc { get; set; } //Описание поставщика (магазина)
    }
    #endregion

    #region Hold Delete
    public class HoldDeleteRequest : HoldRequest
    {
    }

    public class HoldDeleteResponse : HoldResponse
    {
        public string hold_id { get; set; } //Идентификатор холдирования
        public string card_token { get; set; } //Токен карты в системе PAYMO
        public string card_pan { get; set; } //Маска карты

        public string hold_till { get; set; } //Время окончания холдирования (YYYY-mm-ddTHH:mm:ii)
    }
    #endregion
}