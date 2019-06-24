using Cameo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cameo.Services
{
    public class EmailService : IEmailService
    {
        public bool Send(string to, string subject, string body, List<Tuple<Stream, string>> attachments = null, string cc = null)
        {
            bool result = false;

            //to = "rmasimov@wiut.uz";

            try
            {
                //MailMessage message = new MailMessage();
                ////"tpedorchenko@wiut.uz"
                //message.To.Add(to);
                //message.Subject = subject + " | WIUT INTRANET";
                //message.Body = body;
                //message.IsBodyHtml = true;
                //if (cc != null)
                //{
                //    MailAddress copy = new MailAddress(cc);
                //    message.CC.Add(copy);
                //}
                //message.From = new System.Net.Mail.MailAddress("intranet2@wiut.uz");
                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("192.168.33.124", 25);
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential("intranet@wiut.uz", "K&5ygTB}Z2nC*2x_");
                //smtp.EnableSsl = false;
                ////attachments
                //if (attachments != null && attachments.Count > 0)
                //{
                //    foreach (var item in attachments)
                //    {
                //        Attachment att = new Attachment(item.Item1, item.Item2);
                //        message.Attachments.Add(att);
                //    }
                //}

                //smtp.Send(message);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
