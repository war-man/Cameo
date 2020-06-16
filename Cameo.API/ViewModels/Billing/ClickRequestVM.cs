using Cameo.Models;
using Cameo.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.API.ViewModels
{
    public class ClickRequestVM
    {
        public int? click_trans_id { get; set; } //ID платежа в системе CLICK
        public int? service_id { get; set; } //ID сервиса
        public int? click_paydoc_id { get; set; } //Номер платежа в системе CLICK. Отображается в СМС у клиента при оплате.
        public string merchant_trans_id { get; set; } //ID заказа(для Интернет магазинов)/лицевого счета/логина в биллинге поставщика

        //for CompleteRequest
        public long? merchant_prepare_id { get; set; } //ID платежа в биллинг системе поставщика для подтверждения, полученный при запросе «Prepare»

        public float? amount { get; set; } //Сумма оплаты (в сумах)
        public int? action { get; set; } //Выполняемое действие. Для Prepare — 0, Для Complete — 1
        public int? error { get; set; } //Код статуса завершения платежа. 0 – успешно. В случае ошибки возвращается код ошибки.
        public string error_note { get; set; } //Описание кода завершения платежа.
        public string sign_time { get; set; } //Дата платежа. Формат «YYYY-MM-DD HH:mm:ss»
        public string sign_string { get; set; } //Проверочная строка, подтверждающая подлинность отправляемого запроса.

        public ClickTransaction toModel(Customer user)
        {
            var model = new ClickTransaction();
            model.StatusID = (int)ClickTransactionStatusEnum.PENDING;

            model.ClickTransID = click_trans_id ?? 0;
            model.ServiceID = service_id ?? 0;
            model.ClickPaydocID = click_paydoc_id ?? 0;
            model.MerchantTransID = merchant_trans_id;
            model.Amount = amount ?? 0f;
            model.Error = error ?? 0;
            model.ErrorNote = error_note;
            model.SignTime = sign_time;
            model.SignString = sign_string;
            model.Customer = user;

            return model;
        }
    }

    public class ClickResponseVM
    {
        public int click_trans_id { get; set; } //ID платежа в системе CLICK
        public string merchant_trans_id { get; set; } //ID заказа(для Интернет магазинов)/лицевого счета/логина в биллинге поставщика
        public long merchant_prepare_id { get; set; } //ID платежа в биллинг системе поставщика для подтверждения

        //for CompleteResponse
        

        public int error { get; set; } //Код статуса завершения платежа. 0 – успешно. В случае ошибки возвращается код ошибки.
        public string error_note { get; set; } //Описание кода завершения платежа.

        public long? merchant_confirm_id { get; set; }//ID транзакции завершения платежа в биллинг системе (может быть NULL)

        public Dictionary<string, string> toMap()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                ["click_trans_id"] = click_trans_id.ToString(),
                ["merchant_trans_id"] = merchant_trans_id.ToString(),
                ["merchant_prepare_id"] = merchant_prepare_id.ToString(),
                ["merchant_confirm_id"] = merchant_confirm_id?.ToString() ?? "0",
                ["error"] = error.ToString(),
                ["error_note"] = error_note.ToString(),
            };

            return dict;
        }
}
}
