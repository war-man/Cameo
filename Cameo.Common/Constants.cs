using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.Common
{
    public static class Constants
    {
        public static class FileTypes
        {
            public static readonly string CUSTOMER_AVATAR = "customer_avatar";
            public static readonly string TALENT_AVATAR = "talent_avatar";
            public static readonly string TALENT_INTRO_VIDEO = "talent_intro_video";
            public static readonly string VIDEO_REQUEST_VIDEO = "video_request_video";
            public static readonly string VIDEO_REQUEST_PAYMENT_SCREENSHOT = "video_request_payment_screenshot";
        }

        public static class PAYMO
        {
            public static class SETTINGS
            {
                public static readonly string CONSUMER_KEY = "12345";
                public static readonly string CONSUMER_SECRET = "54321";
                public static readonly int STORE_ID = 11111;
                public static readonly string API_KEY = "54321";
            }

            public static class URLS
            {
                public static readonly string TOKEN_GENERATOR = "https://api.paymo.uz/token";
                public static readonly string HOLD = "https://api.paymo.uz/hold";
            }
        }

        public static class CLICK
        {
            public static class SETTINGS
            {
                public static readonly string MERCHANT_ID = "12345";
                public static readonly int SERVICE_id = 12345;
                public static readonly string SECRET_KEY = "someKey";
            }

            public static class ACTIONS
            {
                public static readonly int PREPARE = 0;
                public static readonly int COMPLETE = 1;
            }

            public static class ERRORS
            {
                public static class Success //Успешный запрос
                {
                    public static readonly int code = 0;
                    public static readonly string note = "Success";
                }
                public static class SIGN_CHECK_FAILED //Ошибка проверки подписи
                {
                    public static readonly int code = -1;
                    public static readonly string note = "SIGN CHECK FAILED!";
                }
                public static class Incorrect_parameter_amount //Неверная сумма оплаты
                {
                    public static readonly int code = -2;
                    public static readonly string note = "Incorrect parameter amount";
                }
                public static class Action_not_found //Запрашиваемое действие не найдено
                {
                    public static readonly int code = -3;
                    public static readonly string note = "Action not found";
                }
                public static class Already_paid //Транзакция ранее была подтверждена (при попытке подтвердить или отменить ранее подтвержденную транзакцию)
                {
                    public static readonly int code = -4;
                    public static readonly string note = "Already paid";
                }
                public static class User_does_not_exist //Не найдет пользователь/заказ (проверка параметра merchant_trans_id)
                {
                    public static readonly int code = -5;
                    public static readonly string note = "User does not exist";
                }
                public static class Transaction_does_not_exist //Не найдена транзакция (проверка параметра merchant_prepare_id)
                {
                    public static readonly int code = -6;
                    public static readonly string note = "Transaction does not exist";
                }
                public static class Failed_to_update_user //Ошибка при изменении данных пользователя (изменение баланса счета и т.п.)
                {
                    public static readonly int code = -7;
                    public static readonly string note = "Failed to update user";
                }
                public static class Error_in_request_from_click //Ошибка в запросе от CLICK (переданы не все параметры и т.п.)
                {
                    public static readonly int code = -8;
                    public static readonly string note = "Error in request from click";
                }
                public static class Transaction_cancelled //Транзакция ранее была отменена (При попытке подтвердить или отменить ранее отмененную транзакцию)
                {
                    public static readonly int code = -9;
                    public static readonly string note = "Transaction cancelled";
                }
                public static class Request_has_error //Транзакция ранее была отменена (При попытке подтвердить или отменить ранее отмененную транзакцию)
                {
                    public static readonly int code = -9;
                    public static readonly string note = "Request has error";
                }
            }
        }
    }
}
