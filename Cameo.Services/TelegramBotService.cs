using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Net.Http;

namespace Cameo.Services
{
    public class TelegramBotService
    {
        readonly string _telegramAPI = "https://api.telegram.org/";
        readonly string _botToken = "bot1368214815:AAH5t_xxyE4xPAkwE7EQI4f2HDzkBfB_klY";

        public TelegramBotService()
        {
            try
            {
                //_botToken = System.Configuration.ConfigurationManager.AppSettings["botToken"];
                //_valimaChannelID = System.Configuration.ConfigurationManager.AppSettings["valimaChannelID"];
                //bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["sendingTelegramNotificationIsEnabled"], out _sendingTelegramNotificationIsEnabled);
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Send a message to a Telegram chat/channel
        /// </summary>
        /// <param name="msg">Message text</param>
        /// <param name="sendTo">Recepient</param>
        public bool SendMessage(string msg, string origin)
        {
            string sendTo = "64926501";
            msg += ". ORIGIN: " + origin;

            try
            {
                msg = WebUtility.UrlEncode(msg);

                using (var httpClient = new HttpClient())
                {
                    var res = httpClient.GetAsync($"{_telegramAPI}{_botToken}/sendMessage?chat_id={sendTo}&text={msg}&parse_mode=HTML&disable_web_page_preview=true").Result;
                    if (res.StatusCode != HttpStatusCode.OK)
                    {
                        //string content = res.Content.ReadAsStringAsync().Result;
                        //string status = res.StatusCode.ToString();
                        throw new Exception($"Couldn't send a message via Telegram. Response from Telegram API: {res.Content.ReadAsStringAsync().Result}");
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
    }
}